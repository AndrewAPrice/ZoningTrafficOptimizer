using UnityEngine;
using System.Collections;

public class MapGUI : MonoBehaviour {

	bool DrawMode = true;

	bool ClearTool = true;
	bool ResidentialTool = false;
	bool CommercialTool = false;
	int Density = 1;

	Vector3 lookAtPoint;
	float rotationY = 0.0f;
	float rotationX = Mathf.PI / 4.0f;
	float zoom = 16.0f;

	float oldMouseX = -1;
	float oldMouseY = -1;

	bool OptimizeJobs = false;
	bool OptimizeZoning = false;

	int lastDrawnX = -1;
	int lastDrawnZ = -1;

	public Material VertexColour;

	int[] congestionValues = new int[7680];

	// Use this for initialization
	void Start () {
		lookAtPoint = new Vector3(16.5f, 0.1f, 16.5f);

		UpdateCamera ();
	}

	void ScrollCamera(float x, float z) {
		x *= zoom;
		z *= zoom;
		// vector from us to the camera
		float xAngle = Mathf.Cos (rotationY);
		float zAngle = Mathf.Sin (rotationY);

		lookAtPoint.x += z * xAngle - x * zAngle;
		lookAtPoint.z += z * zAngle + x * xAngle;


		if(lookAtPoint.x < 0)
			lookAtPoint.x = 0;
		if(lookAtPoint.x > 32)
			lookAtPoint.x = 32;
		if(lookAtPoint.z < 0)
			lookAtPoint.z = 0;
		if(lookAtPoint.z > 32)
			lookAtPoint.z = 32;

		UpdateCamera();

	}

	void RotateCamera(float y, float x) {
		rotationY += y;
		rotationX -= x;
		while(rotationY < 0)
			rotationY += Mathf.PI * 2.0f;
		while(rotationY >= Mathf.PI * 2.0f)
			rotationY -= Mathf.PI * 2.0f;

		if(rotationX > Mathf.PI / 2.0f)
			rotationX = Mathf.PI / 2.0f;
		else if(rotationX < 0)
			rotationX = 0;

		UpdateCamera();
	}

	void Zoom(float amount) {
		if(amount < 0)
			zoom /= -amount + 1.0f;
		else
			zoom *= amount + 1.0f;

		if(zoom < 0.5f)
			zoom = 0.5f;
		else if(zoom > 16.0f)
			zoom = 16.0f;
		UpdateCamera();
	}

	void UpdateCamera() {
		float yAngle = -Mathf.Sin(rotationX);
		float scaleOut = Mathf.Cos(rotationX);

		float xAngle = Mathf.Cos (rotationY) * scaleOut;
		float zAngle = Mathf.Sin (rotationY) * scaleOut;

		Vector3 cameraPos = new Vector3(xAngle, yAngle, zAngle) * zoom;

		this.transform.position = lookAtPoint - cameraPos;
		this.transform.LookAt(lookAtPoint);
	}

	// Update is called once per frame
	void Update () {
		if(DrawMode && Input.GetMouseButton(0)) {
			float mouseX = Input.mousePosition.x;
			float mouseY = Input.mousePosition.y;

			// make sure we clicked outside of the GUI
			if(mouseX > 295 || mouseY < Screen.height - 145) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Plane plane = new Plane(Vector3.up, Vector3.zero);
				float dist;
				if(plane.Raycast(ray, out dist)) {
					// if statment checks to make sure we didn't click the sky
					Vector3 pos = ray.GetPoint (dist);

					// make sure we are in range
					if(pos.x > 0 && pos.z > 0 && pos.x < 32 && pos.z < 32) {
						int blockx = (int)pos.x;
						int blockz = (int)pos.z;

						// check to make sure we clicked the actual block and not the street
						float fractx = pos.x - (float)blockx;
						float fractz = pos.z - (float)blockz;

						if(fractx < 128.0f / (128.0f + 64.0f) && fractz < 128.0f / (128.0f + 64.0f)) {
							if(lastDrawnX != blockx || lastDrawnZ != blockz) {
								if(ResidentialTool)
									MapRenderer.CurrentMap.SetBlock(blockx, blockz, true, Density);
								else if(CommercialTool)
									MapRenderer.CurrentMap.SetBlock(blockx, blockz, false, Density);
								else
									MapRenderer.CurrentMap.SetBlock(blockx, blockz, false, 0);
								lastDrawnX = blockx;
								lastDrawnZ = blockz;
							}

						}
					}
				}
			}
		} else {
			lastDrawnX = -1;
			lastDrawnZ = -1;
		}

		// scroll
		float scrollArea = 15;
		float scrollSpeed = 1;
		var mPosX = Input.mousePosition.x;
		var mPosY = Input.mousePosition.y;
		
		// Do camera movement by mouse position
		if (mPosX < scrollArea || Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.A))
			ScrollCamera(scrollSpeed * Time.deltaTime, 0);
		if (mPosX >= Screen.width-scrollArea || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			ScrollCamera(-scrollSpeed * Time.deltaTime, 0);
		if (mPosY >= Screen.height-scrollArea || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			ScrollCamera(0, scrollSpeed * Time.deltaTime);
		if (mPosY < scrollArea || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			ScrollCamera(0, -scrollSpeed * Time.deltaTime);
		

		// Hold right mouse button down, do rotation
		if (Input.GetMouseButton(1) ) {
			if(oldMouseX != -1) {
				if(Input.mousePosition.x != oldMouseX || Input.mousePosition.y != oldMouseY) {
					RotateCamera((Input.mousePosition.x - oldMouseX) / 200.0f, (Input.mousePosition.y - oldMouseY) / 200.0f);
				}
			}
			oldMouseX = Input.mousePosition.x;
			oldMouseY = Input.mousePosition.y;
		//	this.transform.Translate(-new Vector3(Input.GetAxis("Mouse X")*scrollSpeed, Input.GetAxis("Mouse Y")*scrollSpeed, 0) );
		} else {
			oldMouseX = -1;
			oldMouseY = -1;
		}

		if(Input.GetKey(KeyCode.Q))
			Zoom(-Time.deltaTime);

		if(Input.GetKey(KeyCode.E))
			Zoom(Time.deltaTime);

		if(OptimizeJobs) {
			if(DrawMode || MapRenderer.CurrentMap.Jobs == null || MapRenderer.CurrentMap.JobCount == 0) {
				OptimizeJobs = false;
			} else {
				for(int i = 0; i < 5000; i++)
					MapRenderer.CurrentMap.OptimizeJobs();
			}
		}
		if(OptimizeZoning) {
			if(DrawMode || MapRenderer.CurrentMap.Jobs == null || MapRenderer.CurrentMap.JobCount == 0) {
				OptimizeZoning = false;
			} else {
				OptimizerGoal g = MapRenderer.CurrentMap.Goal;
				for(int i = 0; i < ((g == OptimizerGoal.LeastCongestion || g == OptimizerGoal.MostCongestion) ? 25 : 150); i++)
					MapRenderer.CurrentMap.OptimizeZoning();
			}
		}
	}

	void OnGUI() {
		GUI.Box (new Rect(5, 5, 290, DrawMode ? 140 : 100), "Drawing");
		GUI.Label(new Rect(10, 25, 250, 32), "Residents: " + MapRenderer.CurrentMap.ResidentCount + " Jobs: " + MapRenderer.CurrentMap.JobCount);
		if(GUI.Button(new Rect(10, 45, 60, 32), "Clear")) {
			MapRenderer.CurrentMap.Clear();
		}

		if(GUI.Button(new Rect(75, 45, 80, 32), "Randomize")) {
			MapRenderer.CurrentMap.Randomize();
		}

		MapRenderer.DrawBuildings = GUI.Toggle(new Rect(160, 50, 110, 32), MapRenderer.DrawBuildings, "Show Buildings");

		DrawMode = GUI.Toggle (new Rect(10, 77, 250, 20), DrawMode, "Drawing");

		if(DrawMode) {
			GUI.Label (new Rect(10, 97, 250, 20), "Draw:");
			if(GUI.Toggle(new Rect(50, 97, 60, 20), ClearTool, "Nothing")) {
				ClearTool = true;
				ResidentialTool = false;
				CommercialTool = false;
			}

			if(GUI.Toggle(new Rect(110, 97, 90, 20), ResidentialTool, "Residential")) {
				ClearTool = false;
				ResidentialTool = true;
				CommercialTool = false;
			}

			if(GUI.Toggle(new Rect(200, 97, 90, 20), CommercialTool, "Commercial")) {
				ClearTool = false;
				ResidentialTool = false;
				CommercialTool = true;
			}

			if(!ClearTool) {
				GUI.Label(new Rect(10, 117, 50, 32), "Density:");
				Density = (int)GUI.HorizontalSlider(new Rect(60, 123, 190, 32), (float)Density, 1.0f, 15.0f);
				GUI.Label(new Rect(265, 117, 20, 32), Density.ToString());
			}
		} else if (MapRenderer.CurrentMap.JobCount != MapRenderer.CurrentMap.ResidentCount) {
			GUI.Box(new Rect(5, 110, 290, 100), "Jobs");
			GUI.Label(new Rect(10, 125, 280, 50), "The number of jobs and residents are not equal. Please fix this in " +
			          " drawing or click the balance button below to do this automatically.");
			if(GUI.Button(new Rect(10, 175, 100, 32), "Balance"))
				MapRenderer.CurrentMap.Balance();
		} else if(MapRenderer.CurrentMap.JobCount == 0 && MapRenderer.CurrentMap.ResidentCount == 0) {
			GUI.Box(new Rect(5, 110, 290, 55), "Jobs");
			GUI.Label(new Rect(10, 125, 280, 50), "Place some residents and jobs in drawing to get started.");

		} else if(MapRenderer.CurrentMap.Jobs == null ) {
			GUI.Box (new Rect(5, 110, 290, 120), "Jobs");
			if(GUI.Button(new Rect(10, 130, 150, 32), "Assign Random Jobs"))
				MapRenderer.CurrentMap.AssignRandomJobs();

			GUI.Label(new Rect(10, 168, 280, 50), "Nobody have been paired up with a job. Click the above button to assign each " +
			          "resident a random job.");
		} else {
			GUI.Box (new Rect(5, 110, 290, 85), "Jobs");
			if(GUI.Button(new Rect(10, 130, 150, 32), "Assign Random Jobs"))
				MapRenderer.CurrentMap.AssignRandomJobs();

			OptimizeJobs = GUI.Toggle(new Rect(10, 168, 280, 32), OptimizeJobs, "Optimize Jobs (Without Touching Zoning)");

			GUI.Box (new Rect(5, 205, 290, 40), "Zoning");
			OptimizeZoning = GUI.Toggle(new Rect(10, 220, 280, 32), OptimizeZoning, "Optimize Zoning");

			GUI.Box (new Rect(5, Screen.height - 75, 340, 70), "Goal");
			if(GUI.Toggle(new Rect(10, Screen.height - 55, 70, 22), MapRenderer.CurrentMap.Goal == OptimizerGoal.NoGoal, "No Goal"))
				MapRenderer.CurrentMap.Goal = OptimizerGoal.NoGoal;
			if(GUI.Toggle(new Rect(80, Screen.height - 55, 130, 22), MapRenderer.CurrentMap.Goal == OptimizerGoal.ShortestCommute, "Shortest Commute"))
				MapRenderer.CurrentMap.Goal = OptimizerGoal.ShortestCommute;
			if(GUI.Toggle(new Rect(215, Screen.height - 55, 130, 22), MapRenderer.CurrentMap.Goal == OptimizerGoal.LongestCommute, "Longest Commute"))
				MapRenderer.CurrentMap.Goal = OptimizerGoal.LongestCommute;
			if(GUI.Toggle(new Rect(10, Screen.height - 35, 130, 32), MapRenderer.CurrentMap.Goal == OptimizerGoal.LeastCongestion, "Least Congestion")) {
				OptimizeJobs = false;
				MapRenderer.CurrentMap.Goal = OptimizerGoal.LeastCongestion;
			}
			if(GUI.Toggle(new Rect(140, Screen.height - 35, 130, 32), MapRenderer.CurrentMap.Goal == OptimizerGoal.MostCongestion, "Most Congestion")) {
				OptimizeJobs = false;
				MapRenderer.CurrentMap.Goal = OptimizerGoal.MostCongestion;
			}

			// commute
			GUI.Box (new Rect(Screen.width - 295, 5, 290, 200), "Commutes");

			int totalCommute; float averageCommute;
			MapRenderer.CurrentMap.GetCommuteStatistics(out totalCommute, out averageCommute);

			GUI.Label(new Rect(Screen.width - 290, 25, 280, 32), "Total Commute: " + totalCommute);
			GUI.Label(new Rect(Screen.width - 290, 45, 280, 32), "Average Commute: " + averageCommute);

			MapRenderer.DrawCommutes = GUI.Toggle(new Rect(Screen.width - 125, 25, 120, 32), MapRenderer.DrawCommutes, "Show Commutes");

			int largestGroup = 0;
			int largestGroupValue = 0;
			for(int i = 1; i < 64; i++)
			{
				int c = MapRenderer.CurrentMap.CommuteStats[i];
				if(c > 0)
					largestGroup = i;
				if(c > largestGroupValue)
					largestGroupValue = c;
			}

			float commuteGraphHeight = 100.0f;
			float commuteGraphHeightScale = commuteGraphHeight/(float)largestGroupValue;

			float commuteGraphWidth = 260.0f;
			float commuteGraphWidthScale = commuteGraphWidth/(float)largestGroup;

			float commuteGraphX = Screen.width - 285.0f;
			float commuteGraphY = Screen.height - 180.0f;
			GUI.Label(new Rect(Screen.width - 290, 65, 35, 32), largestGroupValue.ToString());
			GUI.Label(new Rect(Screen.width - 290, 182, 10, 30), "0");
			GUI.Label(new Rect(Screen.width - 30, 182, 25, 32), largestGroup.ToString());

			// congestion
			GUI.Box(new Rect(Screen.width - 295, 210, 290, 200), "Congestion");
			int mostCongested, mostCongestedNumber;
			MapRenderer.CurrentMap.GetConguestionStatistics(out mostCongested, out mostCongestedNumber);
			GUI.Label(new Rect(Screen.width - 290, 230, 280, 32), "Worst Congestion: " + mostCongested);
			
			MapRenderer.DrawCongestion = GUI.Toggle(new Rect(Screen.width - 125, 230, 120, 32), MapRenderer.DrawCongestion, "Show congestion");
			for(int i = 0; i <= mostCongested; i++) {
				congestionValues[i] = 0;
			}

			for(int z = 0; z < 32; z++) {
				for(int x = 0; x < 31; x++) {
					int c = MapRenderer.CurrentMap.HorizontalStreetCongestion[x, z];
					congestionValues[c]++;
					c = MapRenderer.CurrentMap.VerticalStreetCongestion[z, x];
					congestionValues[c]++;
				}
			}

			mostCongestedNumber = 0;
			for(int i = 0; i <= mostCongested; i++) {
				int c = congestionValues[i];
				if(c > mostCongestedNumber)
					mostCongestedNumber = c;
			}

			float congestionGraphHeight = 100.0f;
			float congestionGraphHeightScale = congestionGraphHeight/(float)mostCongestedNumber;
			
			float congestionGraphWidth = 260.0f;
			float congestionGraphWidthScale = congestionGraphWidth/(float)mostCongested;
			
			float congestionGraphX = Screen.width - 285.0f;
			float congestionGraphY = Screen.height - 365.0f;
			GUI.Label(new Rect(Screen.width - 290, 250, 35, 32), mostCongestedNumber.ToString());
			GUI.Label(new Rect(Screen.width - 290, 367, 10, 30), "0");
			GUI.Label(new Rect(Screen.width - 30, 367, 25, 32), mostCongested.ToString());

			VertexColour.SetPass(0);
			GL.LoadPixelMatrix();
			GL.Begin(GL.LINES);
			GL.Color(Color.white);

			// draw lines for the commute graph
			for(int i = 0; i < largestGroup; i++) {
				int c = MapRenderer.CurrentMap.CommuteStats[i];
				GL.Vertex3(commuteGraphX, commuteGraphY + (float)c * commuteGraphHeightScale, 0);
				           
				c = MapRenderer.CurrentMap.CommuteStats[i + 1];
				commuteGraphX += commuteGraphWidthScale;
				GL.Vertex3(commuteGraphX, commuteGraphY + (float)c * commuteGraphHeightScale, 0);
			}

			// draw lines for the congestion graph
			for(int i = 0; i < mostCongested; i++) {
				int c = congestionValues[i];
				GL.Vertex3(congestionGraphX, congestionGraphY + (float)c * congestionGraphHeightScale, 0);
				
				c = congestionValues[i + 1];
				congestionGraphX += congestionGraphWidthScale;
				GL.Vertex3(congestionGraphX, congestionGraphY + (float)c * congestionGraphHeightScale, 0);
			}
			GL.End();
		}
	}
}
