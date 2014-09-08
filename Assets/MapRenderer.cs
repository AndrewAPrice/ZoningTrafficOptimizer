using UnityEngine;
using System.Collections;

public class MapRenderer : MonoBehaviour {
	public static Map CurrentMap;
	public Material DrawingMaterial;
	public Material VertexColourMaterial;

	// texture coords
	readonly float GrassL = 0.5f / 256.0f; readonly float GrassT = 0.5f / 512.0f;
	readonly float GrassR = 127.5f / 256.0f; readonly float GrassB = 127.5f / 512.0f;
	
	readonly float ResidentialTopL = 128.5f / 256.0f; readonly float ResidentialTopT = 0.5f / 512.0f;
	readonly float ResidentialTopR = 255.5f / 256.0f; readonly float ResidentialTopB = 127.5f / 512.0f;
	
	readonly float CommercialTopL = 0.5f / 256.0f;  readonly float CommercialTopT = 128.5f / 512.0f;
	readonly float CommercialTopR = 127.5f / 256.0f;  readonly float CommercialTopB = 255.5f / 512.0f;
	
	readonly float TownhouseSideL = 128.5f / 256.0f; readonly float TownhouseSideT = 128.5f / 512.0f;
	readonly float TownhouseSideR = 255.5f / 256.0f; readonly float TownhouseSideB = 191.5f / 512.0f;
	
	readonly float TownhouseUpperSideL = 128.5f / 256.0f; readonly float TownhouseUpperSideT = 192.5f / 512.0f;
	readonly float TownhouseUpperSideR = 255.5f / 256.0f; readonly float TownhouseUpperSideB = 223.5f / 512.0f;
	
	readonly float TownhouseFrontL = 0.5f / 256.0f; readonly float TownhouseFrontT = 256.5f / 512.0f;
	readonly float TownhouseFrontR = 127.5f / 256.0f; readonly float TownhouseFrontB = 319.5f / 512.0f;
	
	readonly float OfficeGroundL = 128.5f / 256.0f; readonly float OfficeGroundT = 256.5f / 512.0f;
	readonly float OfficeGroundR = 255.5f / 256.0f; readonly float OfficeGroundB = 319.5f / 512.0f;
	
	readonly float TownhouseUpperFrontL = 0.5f / 256.0f; readonly float TownhouseUpperFrontT = 320.5f / 512.0f;
	readonly float TownhouseUpperFrontR = 127.5f / 256.0f; readonly float TownhouseUpperFrontB = 351.5f / 512.0f;
	
	readonly float MixedUseUpperL = 128.5f / 256.0f; readonly float MixedUseUpperT = 320.5f / 512.0f;
	readonly float MixedUseUpperR = 255.5f / 256.0f; readonly float MixedUseUpperB = 351.5f / 512.0f;
	
	readonly float StreetL = 0.5f / 256.0f; readonly float StreetT = 352.5f / 512.0f;
	readonly float StreetR = 31.5f / 256.0f; readonly float StreetB = 479.5f / 512.0f;
	
	readonly float MixedUseGroundL = 32.5f / 256.0f; readonly float MixedUseGroundT = 352.5f / 512.0f;
	readonly float MixedUseGroundR = 159.5f / 256.0f; readonly float MixedUseGroundB = 383.5f / 512.0f;
	
	readonly float ShopSideL = 160.5f / 256.0f; readonly float ShopSideT = 352.5f / 512.0f;
	readonly float ShopSideR = 191.5f / 256.0f; readonly float ShopSideB = 383.5f / 512.0f;
	
	readonly float HouseSideL = 192.5f / 256.0f; readonly float HouseSideT = 352.5f / 512.0f;
	readonly float HouseSideR = 223.5f / 256.0f; readonly float HouseSideB = 383.5f / 512.0f;
	
	readonly float OfficeUpperL = 32.5f / 256.0f; readonly float OfficeUpperT = 384.5f / 512.0f;
	readonly float OfficeUpperR = 159.5f / 256.0f; readonly float OfficeUpperB = 415.5f / 512.0f;
	
	readonly float ShopFrontL = 160.5f / 256.0f; readonly float ShopFrontT = 384.5f / 512.0f;
	readonly float ShopFrontR = 223.5f / 256.0f; readonly float ShopFrontB = 415.5f / 512.0f;
	
	readonly float ApartmentUpperL = 32.5f / 256.0f; readonly float ApartmentUpperT = 416.5f / 512.0f;
	readonly float ApartmentUpperR = 159.5f / 256.0f; readonly float ApartmentUpperB = 447.5f / 512.0f;
	
	readonly float LowriseRoofL = 160.5f / 256.0f; readonly float LowriseRoofT = 416.5f / 512.0f;
	readonly float LowriseRoofR = 224.5f / 256.0f; readonly float LowriseRoofB = 447.5f / 512.0f;
	
	readonly float ApartmentGroundL = 32.5f / 256.0f; readonly float ApartmentGroundT = 448.5f / 512.0f;
	readonly float ApartmentGroundR = 159.5f / 256.0f; readonly float ApartmentGroundB = 479.5f / 512.0f;
	
	readonly float IntersectionL = 0.5f / 256.0f; readonly float IntersectionT = 480.5f / 512.0f;
	readonly float IntersectionR = 31.5f / 256.0f; readonly float IntersectionB = 511.5f / 512.0f;
	
	readonly float ShopBackL = 32.5f / 256.0f; readonly float ShopBackT = 480.5f / 512.0f;
	readonly float ShopBackR = 95.5f / 256.0f; readonly float ShopBackB = 511.5f / 512.0f;
	
	readonly float HouseFrontL = 96.5f / 256.0f; readonly float HouseFrontT = 480.5f / 512.0f;
	readonly float HouseFrontR = 159.5f / 256.0f; readonly float HouseFrontB = 511.5f / 512.0f;
	
	readonly float HouseBackL = 160.5f / 256.0f; readonly float HouseBackT = 480.5f / 512.0f;
	readonly float HouseBackR = 223.5f / 256.0f; readonly float HouseBackB = 511.5f / 512.0f;

	readonly float Scale = 1.0f / (128.0f + 64.0f); // Scale texels into 1 block = 1 unit
	readonly float StreetWidth = 64.0f;

	public static bool DrawBuildings = true;
	public static bool DrawCommutes = false;
	public static bool DrawCongestion = false;

	// Use this for initialization
	void Start () {
		CurrentMap = new Map();
	}

	void DrawHouse(float _x, float _z, int rotation) {
		if(rotation == 0 || rotation == 2) {
			float frontL, frontT, frontR, frontB;
			float backL, backT, backR, backB;
			if(rotation == 0) {
				frontL = HouseFrontL; frontT = HouseFrontT; frontR = HouseFrontR; frontB = HouseFrontB;
				backL = HouseBackL; backT = HouseBackT; backR = HouseBackR; backB = HouseBackB;
			} else {
				backL = HouseFrontL; backT = HouseFrontT; backR = HouseFrontR; backB = HouseFrontB;
				frontL = HouseBackL; frontT = HouseBackT; frontR = HouseBackR; frontB = HouseBackB;
			}

			// front
			GL.TexCoord2(frontL, frontT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z + 16.0f);
			GL.TexCoord2(frontL, frontB);
			GL.Vertex3(_x - 32.0f, 0.0f, _z + 16.0f);
			GL.TexCoord2(frontR, frontB);
			GL.Vertex3(_x + 32.0f, 0.0f, _z + 16.0f);
			GL.TexCoord2(frontR, frontT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z + 16.0f);

			// back
			GL.TexCoord2(backR, backT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z - 16.0f);
			GL.TexCoord2(backR, backB);
			GL.Vertex3(_x + 32.0f, 0.0f, _z - 16.0f);
			GL.TexCoord2(backL, backB);
			GL.Vertex3(_x - 32.0f, 0.0f, _z - 16.0f);
			GL.TexCoord2(backL, backT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z - 16.0f);
		
			// left
			GL.TexCoord2(HouseSideL, HouseSideT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z - 16.0f);
			GL.TexCoord2(HouseSideL, HouseSideB);
			GL.Vertex3(_x - 32.0f, 0.0f, _z - 16.0f);
			GL.TexCoord2(HouseSideR, HouseSideB);
			GL.Vertex3(_x - 32.0f, 0.0f, _z + 16.0f);
			GL.TexCoord2(HouseSideR, HouseSideT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z + 16.0f);
		
			// right
			GL.TexCoord2(HouseSideR, HouseSideT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z + 16.0f);
			GL.TexCoord2(HouseSideR, HouseSideB);
			GL.Vertex3(_x + 32.0f, 0.0f, _z + 16.0f);
			GL.TexCoord2(HouseSideL, HouseSideB);
			GL.Vertex3(_x + 32.0f, 0.0f, _z - 16.0f);
			GL.TexCoord2(HouseSideL, HouseSideT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z - 16.0f);
		
			// roof
			GL.TexCoord2(LowriseRoofL, LowriseRoofT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z - 16.0f);
			GL.TexCoord2(LowriseRoofL, LowriseRoofB);
			GL.Vertex3(_x - 32.0f, 32.0f, _z + 16.0f);
			GL.TexCoord2(LowriseRoofR, LowriseRoofB);
			GL.Vertex3(_x + 32.0f, 32.0f, _z + 16.0f);
			GL.TexCoord2(LowriseRoofR, LowriseRoofT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z - 16.0f);
		} else {
			float frontL, frontT, frontR, frontB;
			float backL, backT, backR, backB;
			if(rotation == 1) {
				frontL = HouseFrontL; frontT = HouseFrontT; frontR = HouseFrontR; frontB = HouseFrontB;
				backL = HouseBackL; backT = HouseBackT; backR = HouseBackR; backB = HouseBackB;
			} else {
				backL = HouseFrontL; backT = HouseFrontT; backR = HouseFrontR; backB = HouseFrontB;
				frontL = HouseBackL; frontT = HouseBackT; frontR = HouseBackR; frontB = HouseBackB;
			}
			
			// front
			GL.TexCoord2(frontR, frontT);
			GL.Vertex3(_x + 16.0f, 32.0f, _z + 32.0f);
			GL.TexCoord2(frontR, frontB);
			GL.Vertex3(_x + 16.0f, 0.0f, _z + 32.0f);
			GL.TexCoord2(frontL, frontB);
			GL.Vertex3(_x + 16.0f, 0.0f, _z - 32.0f);
			GL.TexCoord2(frontL, frontT);
			GL.Vertex3(_x + 16.0f, 32.0f, _z - 32.0f);
			
			// back
			GL.TexCoord2(backL, backT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z - 32.0f);
			GL.TexCoord2(backL, backB);
			GL.Vertex3(_x - 16.0f, 0.0f, _z - 32.0f);
			GL.TexCoord2(backR, backB);
			GL.Vertex3(_x - 16.0f, 0.0f, _z + 32.0f);
			GL.TexCoord2(backR, backT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z + 32.0f);
			
			// left
			GL.TexCoord2(HouseSideR, HouseSideT);
			GL.Vertex3(_x + 16.0f, 32.0f, _z - 32.0f);
			GL.TexCoord2(HouseSideR, HouseSideB);
			GL.Vertex3(_x + 16.0f, 0.0f, _z - 32.0f);
			GL.TexCoord2(HouseSideL, HouseSideB);
			GL.Vertex3(_x - 16.0f, 0.0f, _z - 32.0f);
			GL.TexCoord2(HouseSideL, HouseSideT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z - 32.0f);
			
			// right
			GL.TexCoord2(HouseSideL, HouseSideT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z + 32.0f);
			GL.TexCoord2(HouseSideL, HouseSideB);
			GL.Vertex3(_x - 16.0f, 0.0f, _z + 32.0f);
			GL.TexCoord2(HouseSideR, HouseSideB);
			GL.Vertex3(_x + 16.0f, 0.0f, _z + 32.0f);
			GL.TexCoord2(HouseSideR, HouseSideT);
			GL.Vertex3(_x + 16.0f, 32.0f, _z + 32.0f);
			
			// roof
			GL.TexCoord2(LowriseRoofR, LowriseRoofT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z + 32.0f);
			GL.TexCoord2(LowriseRoofR, LowriseRoofB);
			GL.Vertex3(_x + 16.0f, 32.0f, _z + 32.0f);
			GL.TexCoord2(LowriseRoofL, LowriseRoofB);
			GL.Vertex3(_x + 16.0f, 32.0f, _z - 32.0f);
			GL.TexCoord2(LowriseRoofL, LowriseRoofT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z - 32.0f);
		}
	}

	void DrawShop(float _x, float _z, int rotation) {
		if(rotation == 0 || rotation == 2) {
			float frontL, frontT, frontR, frontB;
			float backL, backT, backR, backB;
			if(rotation == 0) {
				frontL = ShopFrontL; frontT = ShopFrontT; frontR = ShopFrontR; frontB = ShopFrontB;
				backL = ShopBackL; backT = ShopBackT; backR = ShopBackR; backB = ShopBackB;
			} else {
				backL = ShopFrontL; backT = ShopFrontT; backR = ShopFrontR; backB = ShopFrontB;
				frontL = ShopBackL; frontT = ShopBackT; frontR = ShopBackR; frontB = ShopBackB;
			}
			
			// front
			GL.TexCoord2(frontL, frontT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z + 16.0f);
			GL.TexCoord2(frontL, frontB);
			GL.Vertex3(_x - 32.0f, 0.0f, _z + 16.0f);
			GL.TexCoord2(frontR, frontB);
			GL.Vertex3(_x + 32.0f, 0.0f, _z + 16.0f);
			GL.TexCoord2(frontR, frontT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z + 16.0f);
			
			// back
			GL.TexCoord2(backR, backT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z - 16.0f);
			GL.TexCoord2(backR, backB);
			GL.Vertex3(_x + 32.0f, 0.0f, _z - 16.0f);
			GL.TexCoord2(backL, backB);
			GL.Vertex3(_x - 32.0f, 0.0f, _z - 16.0f);
			GL.TexCoord2(backL, backT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z - 16.0f);
			
			// left
			GL.TexCoord2(ShopSideL, ShopSideT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z - 16.0f);
			GL.TexCoord2(ShopSideL, ShopSideB);
			GL.Vertex3(_x - 32.0f, 0.0f, _z - 16.0f);
			GL.TexCoord2(ShopSideR, ShopSideB);
			GL.Vertex3(_x - 32.0f, 0.0f, _z + 16.0f);
			GL.TexCoord2(ShopSideR, ShopSideT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z + 16.0f);
			
			// right
			GL.TexCoord2(ShopSideR, ShopSideT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z + 16.0f);
			GL.TexCoord2(ShopSideR, ShopSideB);
			GL.Vertex3(_x + 32.0f, 0.0f, _z + 16.0f);
			GL.TexCoord2(ShopSideL, ShopSideB);
			GL.Vertex3(_x + 32.0f, 0.0f, _z - 16.0f);
			GL.TexCoord2(ShopSideL, ShopSideT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z - 16.0f);
			
			// roof
			GL.TexCoord2(LowriseRoofL, LowriseRoofT);
			GL.Vertex3(_x - 32.0f, 32.0f, _z - 16.0f);
			GL.TexCoord2(LowriseRoofL, LowriseRoofB);
			GL.Vertex3(_x - 32.0f, 32.0f, _z + 16.0f);
			GL.TexCoord2(LowriseRoofR, LowriseRoofB);
			GL.Vertex3(_x + 32.0f, 32.0f, _z + 16.0f);
			GL.TexCoord2(LowriseRoofR, LowriseRoofT);
			GL.Vertex3(_x + 32.0f, 32.0f, _z - 16.0f);
		} else {
			float frontL, frontT, frontR, frontB;
			float backL, backT, backR, backB;
			if(rotation == 1) {
				frontL = ShopFrontL; frontT = ShopFrontT; frontR = ShopFrontR; frontB = ShopFrontB;
				backL = ShopBackL; backT = ShopBackT; backR = ShopBackR; backB = ShopBackB;
			} else {
				backL = ShopFrontL; backT = ShopFrontT; backR = ShopFrontR; backB = ShopFrontB;
				frontL = ShopBackL; frontT = ShopBackT; frontR = ShopBackR; frontB = ShopBackB;
			}
			
			// front
			GL.TexCoord2(frontR, frontT);
			GL.Vertex3(_x + 16.0f, 32.0f, _z + 32.0f);
			GL.TexCoord2(frontR, frontB);
			GL.Vertex3(_x + 16.0f, 0.0f, _z + 32.0f);
			GL.TexCoord2(frontL, frontB);
			GL.Vertex3(_x + 16.0f, 0.0f, _z - 32.0f);
			GL.TexCoord2(frontL, frontT);
			GL.Vertex3(_x + 16.0f, 32.0f, _z - 32.0f);
			
			// back
			GL.TexCoord2(backL, backT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z - 32.0f);
			GL.TexCoord2(backL, backB);
			GL.Vertex3(_x - 16.0f, 0.0f, _z - 32.0f);
			GL.TexCoord2(backR, backB);
			GL.Vertex3(_x - 16.0f, 0.0f, _z + 32.0f);
			GL.TexCoord2(backR, backT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z + 32.0f);
			
			// left
			GL.TexCoord2(ShopSideR, ShopSideT);
			GL.Vertex3(_x + 16.0f, 32.0f, _z - 32.0f);
			GL.TexCoord2(ShopSideR, ShopSideB);
			GL.Vertex3(_x + 16.0f, 0.0f, _z - 32.0f);
			GL.TexCoord2(ShopSideL, ShopSideB);
			GL.Vertex3(_x - 16.0f, 0.0f, _z - 32.0f);
			GL.TexCoord2(ShopSideL, ShopSideT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z - 32.0f);
			
			// right
			GL.TexCoord2(ShopSideL, ShopSideT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z + 32.0f);
			GL.TexCoord2(ShopSideL, ShopSideB);
			GL.Vertex3(_x - 16.0f, 0.0f, _z + 32.0f);
			GL.TexCoord2(ShopSideR, ShopSideB);
			GL.Vertex3(_x + 16.0f, 0.0f, _z + 32.0f);
			GL.TexCoord2(ShopSideR, ShopSideT);
			GL.Vertex3(_x + 16.0f, 32.0f, _z + 32.0f);
			
			// roof
			GL.TexCoord2(LowriseRoofR, LowriseRoofT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z + 32.0f);
			GL.TexCoord2(LowriseRoofR, LowriseRoofB);
			GL.Vertex3(_x + 16.0f, 32.0f, _z + 32.0f);
			GL.TexCoord2(LowriseRoofL, LowriseRoofB);
			GL.Vertex3(_x + 16.0f, 32.0f, _z - 32.0f);
			GL.TexCoord2(LowriseRoofL, LowriseRoofT);
			GL.Vertex3(_x - 16.0f, 32.0f, _z - 32.0f);
		}
	}
	
	void OnRenderObject() {
		DrawingMaterial.SetPass(0);

		GL.PushMatrix();
		Matrix4x4 scale = Matrix4x4.Scale(new Vector3(Scale, Scale, Scale));
		GL.MultMatrix(scale);
		GL.Begin(GL.QUADS);

		// quad order is top left, bottom left, bottom right, top right

		/*GL.TexCoord2(0, 0);
		GL.Vertex3(0, 0, 0);
		GL.TexCoord2(0, 1);
		GL.Vertex3(0, 0, 128.0f);
		GL.TexCoord2(1, 1);
		GL.Vertex3(64.0f, 0, 128.0f);
		GL.TexCoord2(1, 0);
		GL.Vertex3(64.0f, 0, 0);*/

		float _z = 0;

		for(int z = 0; z < 32; z++) {
			float _x = 0;
			for(int x = 0; x < 32; x++) {
				if(DrawBuildings) {
				// draw building
				Block b = CurrentMap.Blocks[z, x];
				if(b.Size <= 4) {
					// draw ground

					switch(b.Rotation) {
					case 0:
						GL.TexCoord2(GrassL, GrassT);
						GL.Vertex3(_x, 0, _z);
						GL.TexCoord2(GrassL, GrassB);
						GL.Vertex3(_x, 0, _z + 128.0f);
						GL.TexCoord2(GrassR, GrassB);
						GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
						GL.TexCoord2(GrassR, GrassT);
						GL.Vertex3(_x + 128.0f, 0, _z);
						break;
					case 1:
						GL.TexCoord2(GrassR, GrassT);
						GL.Vertex3(_x, 0, _z);
						GL.TexCoord2(GrassL, GrassT);
						GL.Vertex3(_x, 0, _z + 128.0f);
						GL.TexCoord2(GrassL, GrassB);
						GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
						GL.TexCoord2(GrassR, GrassB);
						GL.Vertex3(_x + 128.0f, 0, _z);
						break;
					case 2:
						GL.TexCoord2(GrassR, GrassB);
						GL.Vertex3(_x, 0, _z);
						GL.TexCoord2(GrassR, GrassT);
						GL.Vertex3(_x, 0, _z + 128.0f);
						GL.TexCoord2(GrassL, GrassT);
						GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
						GL.TexCoord2(GrassL, GrassB);
						GL.Vertex3(_x + 128.0f, 0, _z);
						break;
					case 3:
						GL.TexCoord2(GrassL, GrassB);
						GL.Vertex3(_x, 0, _z);
						GL.TexCoord2(GrassR, GrassB);
						GL.Vertex3(_x, 0, _z + 128.0f);
						GL.TexCoord2(GrassR, GrassT);
						GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
						GL.TexCoord2(GrassL, GrassT);
						GL.Vertex3(_x + 128.0f, 0, _z);
						break;
					}
				}

				if(b.Residential) {
					// draw residential
					if(b.Size <= 4) {
						// draw houses
						if(b.Size == 1) {
							DrawHouse(_x + 66.0f, _z + 64.0f, b.Rotation);
						} else if(b.Size == 2) {
							if(b.Rotation == 0) {
								DrawHouse(_x + 32.0f, _z + 32.0f, 2);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
							} else if(b.Rotation == 1) {
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 2);
								DrawHouse(_x + 32.0f, _z + 128.0f - 32.0f, 0);
							} else if(b.Rotation == 2) {
								DrawHouse(_x + 32.0f, _z + 32.0f, 3);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 1);
							} else {
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 1);
								DrawHouse(_x + 32.0f, _z + 128.0f - 32.0f, 3);
							}
						} else if(b.Size == 3) {
							if(b.Rotation == 0) {
								DrawHouse(_x + 32.0f, _z + 32.0f, 2);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 1);
							} else if(b.Rotation == 1) {
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 2);
								DrawHouse(_x + 32.0f, _z + 128.0f - 32.0f, 0);
								DrawHouse(_x + 32.0f, _z + 32.0f, 3);
							} else if(b.Rotation == 2) {
								DrawHouse(_x + 32.0f, _z + 32.0f, 3);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 1);
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 2);
							} else {
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 1);
								DrawHouse(_x + 32.0f, _z + 128.0f - 32.0f, 3);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
							}
						} else if(b.Size == 4) {
							if(b.Rotation == 0) {
								DrawHouse(_x + 32.0f, _z + 32.0f, 2);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 1);
								DrawHouse(_x + 32.0f, _z + 128.0f - 32.0f, 3);
							} else if(b.Rotation == 1) {
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 2);
								DrawHouse(_x + 32.0f, _z + 128.0f - 32.0f, 0);
								DrawHouse(_x + 32.0f, _z + 32.0f, 3);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 1);
							} else if(b.Rotation == 2) {
								DrawHouse(_x + 32.0f, _z + 32.0f, 3);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 1);
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 2);
								DrawHouse(_x + 32.0f, _z + 128.0f - 32.0f, 0);
							} else {
								DrawHouse(_x + 128.0f - 32.0f, _z + 32.0f, 1);
								DrawHouse(_x + 32.0f, _z + 128.0f - 32.0f, 3);
								DrawHouse(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
								DrawHouse(_x + 32.0f, _z + 32.0f, 2);
							}
						}
					} else if(b.Size <= 10) {
						// draw townhouse
						float frontL, frontT, frontR, frontB;
						float sideL, sideT, sideR, sideB;
						float upperfrontL, upperfrontT, upperfrontR, upperfrontB;
						float uppersideL, uppersideT, uppersideR, uppersideB;
						if(b.Rotation == 0 || b.Rotation == 1) {
							frontL = TownhouseFrontL; frontT = TownhouseFrontT; frontR = TownhouseFrontR; frontB = TownhouseFrontB;
							sideL = TownhouseSideL; sideT = TownhouseSideT; sideR = TownhouseSideR; sideB = TownhouseSideB;
							upperfrontL = TownhouseUpperFrontL; upperfrontT = TownhouseUpperFrontT; upperfrontR = TownhouseUpperFrontR; upperfrontB = TownhouseUpperFrontB;
							uppersideL = TownhouseUpperSideL; uppersideT = TownhouseUpperSideT; uppersideR = TownhouseUpperSideR; uppersideB = TownhouseUpperSideB;
						} else {
							sideL = TownhouseFrontL; sideT = TownhouseFrontT; sideR = TownhouseFrontR; sideB = TownhouseFrontB;
							frontL = TownhouseSideL; frontT = TownhouseSideT; frontR = TownhouseSideR; frontB = TownhouseSideB;
							uppersideL = TownhouseUpperFrontL; uppersideT = TownhouseUpperFrontT; uppersideR = TownhouseUpperFrontR; uppersideB = TownhouseUpperFrontB;
							upperfrontL = TownhouseUpperSideL; upperfrontT = TownhouseUpperSideT; upperfrontR = TownhouseUpperSideR; upperfrontB = TownhouseUpperSideB;
						}

						// front
						GL.TexCoord2(frontL, frontT);
						GL.Vertex3(_x, 64.0f, _z + 128.0f);
						GL.TexCoord2(frontL, frontB);
						GL.Vertex3(_x, 0.0f, _z + 128.0f);
						GL.TexCoord2(frontR, frontB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z + 128.0f);
						GL.TexCoord2(frontR, frontT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z + 128.0f);
						
						// back
						GL.TexCoord2(frontR, frontT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z);
						GL.TexCoord2(frontR, frontB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z);
						GL.TexCoord2(frontL, frontB);
						GL.Vertex3(_x, 0.0f, _z);
						GL.TexCoord2(frontL, frontT);
						GL.Vertex3(_x, 64.0f, _z);
						
						// left
						GL.TexCoord2(sideL, sideT);
						GL.Vertex3(_x, 64.0f, _z);
						GL.TexCoord2(sideL, sideB);
						GL.Vertex3(_x, 0.0f, _z);
						GL.TexCoord2(sideR, sideB);
						GL.Vertex3(_x, 0.0f, _z + 128.0f);
						GL.TexCoord2(sideR, sideT);
						GL.Vertex3(_x, 64.0f, _z + 128.0f);
						
						// right
						GL.TexCoord2(sideR, sideT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z + 128.0f);
						GL.TexCoord2(sideR, sideB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z + 128.0f);
						GL.TexCoord2(sideL, sideB);
						GL.Vertex3(_x + 128, 0.0f, _z);
						GL.TexCoord2(sideL, sideT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z);

						// draw upper floors
						float y = 64.0f;
						int floors = (b.Size - 5) / 2;
						for(int i = 0; i < floors; i++) {
							// front
							GL.TexCoord2(upperfrontL, upperfrontT);
							GL.Vertex3(_x, y + 32.0f, _z + 128.0f);
							GL.TexCoord2(upperfrontL, upperfrontB);
							GL.Vertex3(_x, y, _z + 128.0f);
							GL.TexCoord2(upperfrontR, upperfrontB);
							GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
							GL.TexCoord2(upperfrontR, upperfrontT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z + 128.0f);
							
							// back
							GL.TexCoord2(upperfrontR, upperfrontT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z);
							GL.TexCoord2(upperfrontR, upperfrontB);
							GL.Vertex3(_x + 128.0f, y, _z);
							GL.TexCoord2(upperfrontL, upperfrontB);
							GL.Vertex3(_x, y, _z);
							GL.TexCoord2(upperfrontL, upperfrontT);
							GL.Vertex3(_x, y + 32.0f, _z);
							
							// left
							GL.TexCoord2(uppersideL, uppersideT);
							GL.Vertex3(_x, y + 32.0f, _z);
							GL.TexCoord2(uppersideL, uppersideB);
							GL.Vertex3(_x, y, _z);
							GL.TexCoord2(uppersideR, uppersideB);
							GL.Vertex3(_x, y, _z + 128.0f);
							GL.TexCoord2(uppersideR, uppersideT);
							GL.Vertex3(_x, y + 32.0f, _z + 128.0f);
							
							// right
							GL.TexCoord2(uppersideR, uppersideT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z + 128.0f);
							GL.TexCoord2(uppersideR, uppersideB);
							GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
							GL.TexCoord2(uppersideL, uppersideB);
							GL.Vertex3(_x + 128, y, _z);
							GL.TexCoord2(uppersideL, uppersideT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z);

							y += 32.0f;
						}

						// draw roof
						GL.TexCoord2(ResidentialTopL, ResidentialTopT);
						GL.Vertex3(_x, y, _z);
						GL.TexCoord2(ResidentialTopL, ResidentialTopB);
						GL.Vertex3(_x, y, _z + 128.0f);
						GL.TexCoord2(ResidentialTopR, ResidentialTopB);
						GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
						GL.TexCoord2(ResidentialTopR, ResidentialTopT);
						GL.Vertex3(_x + 128.0f, y, _z);

					} else {
						// draw apartment
												
						// front
						GL.TexCoord2(ApartmentGroundL, ApartmentGroundT);
						GL.Vertex3(_x, 64.0f, _z + 128.0f);
						GL.TexCoord2(ApartmentGroundL, ApartmentGroundB);
						GL.Vertex3(_x, 0.0f, _z + 128.0f);
						GL.TexCoord2(ApartmentGroundR, ApartmentGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z + 128.0f);
						GL.TexCoord2(ApartmentGroundR, ApartmentGroundT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z + 128.0f);
						
						// back
						GL.TexCoord2(ApartmentGroundR, ApartmentGroundT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z);
						GL.TexCoord2(ApartmentGroundR, ApartmentGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z);
						GL.TexCoord2(ApartmentGroundL, ApartmentGroundB);
						GL.Vertex3(_x, 0.0f, _z);
						GL.TexCoord2(ApartmentGroundL, ApartmentGroundT);
						GL.Vertex3(_x, 64.0f, _z);
						
						// left
						GL.TexCoord2(ApartmentGroundL, ApartmentGroundT);
						GL.Vertex3(_x, 64.0f, _z);
						GL.TexCoord2(ApartmentGroundL, ApartmentGroundB);
						GL.Vertex3(_x, 0.0f, _z);
						GL.TexCoord2(ApartmentGroundR, ApartmentGroundB);
						GL.Vertex3(_x, 0.0f, _z + 128.0f);
						GL.TexCoord2(ApartmentGroundR, ApartmentGroundT);
						GL.Vertex3(_x, 64.0f, _z + 128.0f);
						
						// right
						GL.TexCoord2(ApartmentGroundR, ApartmentGroundT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z + 128.0f);
						GL.TexCoord2(ApartmentGroundR, ApartmentGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z + 128.0f);
						GL.TexCoord2(ApartmentGroundL, ApartmentGroundB);
						GL.Vertex3(_x + 128, 0.0f, _z);
						GL.TexCoord2(ApartmentGroundL, ApartmentGroundT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z);

						// draw upper floors
						float y = 64.0f;
						int floors = (b.Size - 5) / 2;
						for(int i = 0; i < floors; i++) {
							// front
							GL.TexCoord2(ApartmentUpperL, ApartmentUpperT);
							GL.Vertex3(_x, y + 32.0f, _z + 128.0f);
							GL.TexCoord2(ApartmentUpperL, ApartmentUpperB);
							GL.Vertex3(_x, y, _z + 128.0f);
							GL.TexCoord2(ApartmentUpperR, ApartmentUpperB);
							GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
							GL.TexCoord2(ApartmentUpperR, ApartmentUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z + 128.0f);
							
							// back
							GL.TexCoord2(ApartmentUpperR, ApartmentUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z);
							GL.TexCoord2(ApartmentUpperR, ApartmentUpperB);
							GL.Vertex3(_x + 128.0f, y, _z);
							GL.TexCoord2(ApartmentUpperL, ApartmentUpperB);
							GL.Vertex3(_x, y, _z);
							GL.TexCoord2(ApartmentUpperL, ApartmentUpperT);
							GL.Vertex3(_x, y + 32.0f, _z);
							
							// left
							GL.TexCoord2(ApartmentUpperL, ApartmentUpperT);
							GL.Vertex3(_x, y + 32.0f, _z);
							GL.TexCoord2(ApartmentUpperL, ApartmentUpperB);
							GL.Vertex3(_x, y, _z);
							GL.TexCoord2(ApartmentUpperR, ApartmentUpperB);
							GL.Vertex3(_x, y, _z + 128.0f);
							GL.TexCoord2(ApartmentUpperR, ApartmentUpperT);
							GL.Vertex3(_x, y + 32.0f, _z + 128.0f);
							
							// right
							GL.TexCoord2(ApartmentUpperR, ApartmentUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z + 128.0f);
							GL.TexCoord2(ApartmentUpperR, ApartmentUpperB);
							GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
							GL.TexCoord2(ApartmentUpperL, ApartmentUpperB);
							GL.Vertex3(_x + 128, y, _z);
							GL.TexCoord2(ApartmentUpperL, ApartmentUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z);
							
							y += 32.0f;
						}
						
						// draw roof
						GL.TexCoord2(ResidentialTopL, ResidentialTopT);
						GL.Vertex3(_x, y, _z);
						GL.TexCoord2(ResidentialTopL, ResidentialTopB);
						GL.Vertex3(_x, y, _z + 128.0f);
						GL.TexCoord2(ResidentialTopR, ResidentialTopB);
						GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
						GL.TexCoord2(ResidentialTopR, ResidentialTopT);
						GL.Vertex3(_x + 128.0f, y, _z);
					}
				} else {
					// draw commercial

					if(b.Size <= 4) {
						// draw shops
						if(b.Size == 1) {
							DrawShop(_x + 66.0f, _z + 64.0f, b.Rotation);
						} else if(b.Size == 2) {
							if(b.Rotation == 0) {
								DrawShop(_x + 32.0f, _z + 32.0f, 2);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
							} else if(b.Rotation == 1) {
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 2);
								DrawShop(_x + 32.0f, _z + 128.0f - 32.0f, 0);
							} else if(b.Rotation == 2) {
								DrawShop(_x + 32.0f, _z + 32.0f, 3);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 1);
							} else {
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 1);
								DrawShop(_x + 32.0f, _z + 128.0f - 32.0f, 3);
							}
						} else if(b.Size == 3) {
							if(b.Rotation == 0) {
								DrawShop(_x + 32.0f, _z + 32.0f, 2);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 1);
							} else if(b.Rotation == 1) {
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 2);
								DrawShop(_x + 32.0f, _z + 128.0f - 32.0f, 0);
								DrawShop(_x + 32.0f, _z + 32.0f, 3);
							} else if(b.Rotation == 2) {
								DrawShop(_x + 32.0f, _z + 32.0f, 3);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 1);
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 2);
							} else {
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 1);
								DrawShop(_x + 32.0f, _z + 128.0f - 32.0f, 3);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
							}
						} else if(b.Size == 4) {
							if(b.Rotation == 0) {
								DrawShop(_x + 32.0f, _z + 32.0f, 2);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 1);
								DrawShop(_x + 32.0f, _z + 128.0f - 32.0f, 3);
							} else if(b.Rotation == 1) {
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 2);
								DrawShop(_x + 32.0f, _z + 128.0f - 32.0f, 0);
								DrawShop(_x + 32.0f, _z + 32.0f, 3);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 1);
							} else if(b.Rotation == 2) {
								DrawShop(_x + 32.0f, _z + 32.0f, 3);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 1);
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 2);
								DrawShop(_x + 32.0f, _z + 128.0f - 32.0f, 0);
							} else {
								DrawShop(_x + 128.0f - 32.0f, _z + 32.0f, 1);
								DrawShop(_x + 32.0f, _z + 128.0f - 32.0f, 3);
								DrawShop(_x + 128.0f - 32.0f, _z + 128.0f - 32.0f, 0);
								DrawShop(_x + 32.0f, _z + 32.0f, 2);
							}
						}
					} else if(b.Size <= 10) {
						// draw mixed use
						
						// front
						GL.TexCoord2(MixedUseGroundL, MixedUseGroundT);
						GL.Vertex3(_x, 32.0f, _z + 128.0f);
						GL.TexCoord2(MixedUseGroundL, MixedUseGroundB);
						GL.Vertex3(_x, 0.0f, _z + 128.0f);
						GL.TexCoord2(MixedUseGroundR, MixedUseGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z + 128.0f);
						GL.TexCoord2(MixedUseGroundR, MixedUseGroundT);
						GL.Vertex3(_x + 128.0f, 32.0f, _z + 128.0f);
						
						// back
						GL.TexCoord2(MixedUseGroundR, MixedUseGroundT);
						GL.Vertex3(_x + 128.0f, 32.0f, _z);
						GL.TexCoord2(MixedUseGroundR, MixedUseGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z);
						GL.TexCoord2(MixedUseGroundL, MixedUseGroundB);
						GL.Vertex3(_x, 0.0f, _z);
						GL.TexCoord2(MixedUseGroundL, MixedUseGroundT);
						GL.Vertex3(_x, 32.0f, _z);
						
						// left
						GL.TexCoord2(MixedUseGroundL, MixedUseGroundT);
						GL.Vertex3(_x, 32.0f, _z);
						GL.TexCoord2(MixedUseGroundL, MixedUseGroundB);
						GL.Vertex3(_x, 0.0f, _z);
						GL.TexCoord2(MixedUseGroundR, MixedUseGroundB);
						GL.Vertex3(_x, 0.0f, _z + 128.0f);
						GL.TexCoord2(MixedUseGroundR, MixedUseGroundT);
						GL.Vertex3(_x, 32.0f, _z + 128.0f);
						
						// right
						GL.TexCoord2(MixedUseGroundR, MixedUseGroundT);
						GL.Vertex3(_x + 128.0f, 32.0f, _z + 128.0f);
						GL.TexCoord2(MixedUseGroundR, MixedUseGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z + 128.0f);
						GL.TexCoord2(MixedUseGroundL, MixedUseGroundB);
						GL.Vertex3(_x + 128, 0.0f, _z);
						GL.TexCoord2(MixedUseGroundL, MixedUseGroundT);
						GL.Vertex3(_x + 128.0f, 32.0f, _z);
						
						// draw upper floors
						float y = 32.0f;
						int floors = (b.Size - 5) / 2;
						for(int i = 0; i < floors; i++) {
							// front
							GL.TexCoord2(MixedUseUpperL, MixedUseUpperT);
							GL.Vertex3(_x, y + 32.0f, _z + 128.0f);
							GL.TexCoord2(MixedUseUpperL, MixedUseUpperB);
							GL.Vertex3(_x, y, _z + 128.0f);
							GL.TexCoord2(MixedUseUpperR, MixedUseUpperB);
							GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
							GL.TexCoord2(MixedUseUpperR, MixedUseUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z + 128.0f);
							
							// back
							GL.TexCoord2(MixedUseUpperR, MixedUseUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z);
							GL.TexCoord2(MixedUseUpperR, MixedUseUpperB);
							GL.Vertex3(_x + 128.0f, y, _z);
							GL.TexCoord2(MixedUseUpperL, MixedUseUpperB);
							GL.Vertex3(_x, y, _z);
							GL.TexCoord2(MixedUseUpperL, MixedUseUpperT);
							GL.Vertex3(_x, y + 32.0f, _z);
							
							// left
							GL.TexCoord2(MixedUseUpperL, MixedUseUpperT);
							GL.Vertex3(_x, y + 32.0f, _z);
							GL.TexCoord2(MixedUseUpperL, MixedUseUpperB);
							GL.Vertex3(_x, y, _z);
							GL.TexCoord2(MixedUseUpperR, MixedUseUpperB);
							GL.Vertex3(_x, y, _z + 128.0f);
							GL.TexCoord2(MixedUseUpperR, MixedUseUpperT);
							GL.Vertex3(_x, y + 32.0f, _z + 128.0f);
							
							// right
							GL.TexCoord2(MixedUseUpperR, MixedUseUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z + 128.0f);
							GL.TexCoord2(MixedUseUpperR, MixedUseUpperB);
							GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
							GL.TexCoord2(MixedUseUpperL, MixedUseUpperB);
							GL.Vertex3(_x + 128, y, _z);
							GL.TexCoord2(MixedUseUpperL, MixedUseUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z);
							
							y += 32.0f;
						}
						
						// draw roof
						GL.TexCoord2(CommercialTopL, CommercialTopT);
						GL.Vertex3(_x, y, _z);
						GL.TexCoord2(CommercialTopL, CommercialTopB);
						GL.Vertex3(_x, y, _z + 128.0f);
						GL.TexCoord2(CommercialTopR, CommercialTopB);
						GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
						GL.TexCoord2(CommercialTopR, CommercialTopT);
						GL.Vertex3(_x + 128.0f, y, _z);
					} else {
						// draw office
						
						// front
						GL.TexCoord2(OfficeGroundL, OfficeGroundT);
						GL.Vertex3(_x, 64.0f, _z + 128.0f);
						GL.TexCoord2(OfficeGroundL, OfficeGroundB);
						GL.Vertex3(_x, 0.0f, _z + 128.0f);
						GL.TexCoord2(OfficeGroundR, OfficeGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z + 128.0f);
						GL.TexCoord2(OfficeGroundR, OfficeGroundT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z + 128.0f);
						
						// back
						GL.TexCoord2(OfficeGroundR, OfficeGroundT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z);
						GL.TexCoord2(OfficeGroundR, OfficeGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z);
						GL.TexCoord2(OfficeGroundL, OfficeGroundB);
						GL.Vertex3(_x, 0.0f, _z);
						GL.TexCoord2(OfficeGroundL, OfficeGroundT);
						GL.Vertex3(_x, 64.0f, _z);
						
						// left
						GL.TexCoord2(OfficeGroundL, OfficeGroundT);
						GL.Vertex3(_x, 64.0f, _z);
						GL.TexCoord2(OfficeGroundL, OfficeGroundB);
						GL.Vertex3(_x, 0.0f, _z);
						GL.TexCoord2(OfficeGroundR, OfficeGroundB);
						GL.Vertex3(_x, 0.0f, _z + 128.0f);
						GL.TexCoord2(OfficeGroundR, OfficeGroundT);
						GL.Vertex3(_x, 64.0f, _z + 128.0f);
						
						// right
						GL.TexCoord2(OfficeGroundR, OfficeGroundT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z + 128.0f);
						GL.TexCoord2(OfficeGroundR, OfficeGroundB);
						GL.Vertex3(_x + 128.0f, 0.0f, _z + 128.0f);
						GL.TexCoord2(OfficeGroundL, OfficeGroundB);
						GL.Vertex3(_x + 128, 0.0f, _z);
						GL.TexCoord2(OfficeGroundL, OfficeGroundT);
						GL.Vertex3(_x + 128.0f, 64.0f, _z);
						
						// draw upper floors
						float y = 64.0f;
						int floors = (b.Size - 5) / 2;
						for(int i = 0; i < floors; i++) {
							// front
							GL.TexCoord2(OfficeUpperL, OfficeUpperT);
							GL.Vertex3(_x, y + 32.0f, _z + 128.0f);
							GL.TexCoord2(OfficeUpperL, OfficeUpperB);
							GL.Vertex3(_x, y, _z + 128.0f);
							GL.TexCoord2(OfficeUpperR, OfficeUpperB);
							GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
							GL.TexCoord2(OfficeUpperR, OfficeUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z + 128.0f);
							
							// back
							GL.TexCoord2(OfficeUpperR, OfficeUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z);
							GL.TexCoord2(OfficeUpperR, OfficeUpperB);
							GL.Vertex3(_x + 128.0f, y, _z);
							GL.TexCoord2(OfficeUpperL, OfficeUpperB);
							GL.Vertex3(_x, y, _z);
							GL.TexCoord2(OfficeUpperL, OfficeUpperT);
							GL.Vertex3(_x, y + 32.0f, _z);
							
							// left
							GL.TexCoord2(OfficeUpperL, OfficeUpperT);
							GL.Vertex3(_x, y + 32.0f, _z);
							GL.TexCoord2(OfficeUpperL, OfficeUpperB);
							GL.Vertex3(_x, y, _z);
							GL.TexCoord2(OfficeUpperR, OfficeUpperB);
							GL.Vertex3(_x, y, _z + 128.0f);
							GL.TexCoord2(OfficeUpperR, OfficeUpperT);
							GL.Vertex3(_x, y + 32.0f, _z + 128.0f);
							
							// right
							GL.TexCoord2(OfficeUpperR, OfficeUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z + 128.0f);
							GL.TexCoord2(OfficeUpperR, OfficeUpperB);
							GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
							GL.TexCoord2(OfficeUpperL, OfficeUpperB);
							GL.Vertex3(_x + 128, y, _z);
							GL.TexCoord2(OfficeUpperL, OfficeUpperT);
							GL.Vertex3(_x + 128.0f, y + 32.0f, _z);
							
							y += 32.0f;
						}
						
						// draw roof
						GL.TexCoord2(CommercialTopL, CommercialTopT);
						GL.Vertex3(_x, y, _z);
						GL.TexCoord2(CommercialTopL, CommercialTopB);
						GL.Vertex3(_x, y, _z + 128.0f);
						GL.TexCoord2(CommercialTopR, CommercialTopB);
						GL.Vertex3(_x + 128.0f, y, _z + 128.0f);
						GL.TexCoord2(CommercialTopR, CommercialTopT);
						GL.Vertex3(_x + 128.0f, y, _z);
					}
				}
				} else {
					Block b = CurrentMap.Blocks[z, x];
					if(b.Size == 0) {
						// draw ground
						
						switch(b.Rotation) {
						case 0:
							GL.TexCoord2(GrassL, GrassT);
							GL.Vertex3(_x, 0, _z);
							GL.TexCoord2(GrassL, GrassB);
							GL.Vertex3(_x, 0, _z + 128.0f);
							GL.TexCoord2(GrassR, GrassB);
							GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
							GL.TexCoord2(GrassR, GrassT);
							GL.Vertex3(_x + 128.0f, 0, _z);
							break;
						case 1:
							GL.TexCoord2(GrassR, GrassT);
							GL.Vertex3(_x, 0, _z);
							GL.TexCoord2(GrassL, GrassT);
							GL.Vertex3(_x, 0, _z + 128.0f);
							GL.TexCoord2(GrassL, GrassB);
							GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
							GL.TexCoord2(GrassR, GrassB);
							GL.Vertex3(_x + 128.0f, 0, _z);
							break;
						case 2:
							GL.TexCoord2(GrassR, GrassB);
							GL.Vertex3(_x, 0, _z);
							GL.TexCoord2(GrassR, GrassT);
							GL.Vertex3(_x, 0, _z + 128.0f);
							GL.TexCoord2(GrassL, GrassT);
							GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
							GL.TexCoord2(GrassL, GrassB);
							GL.Vertex3(_x + 128.0f, 0, _z);
							break;
						case 3:
							GL.TexCoord2(GrassL, GrassB);
							GL.Vertex3(_x, 0, _z);
							GL.TexCoord2(GrassR, GrassB);
							GL.Vertex3(_x, 0, _z + 128.0f);
							GL.TexCoord2(GrassR, GrassT);
							GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
							GL.TexCoord2(GrassL, GrassT);
							GL.Vertex3(_x + 128.0f, 0, _z);
							break;
						}
					} else if(b.Residential) {
						GL.TexCoord2(ResidentialTopL, ResidentialTopT);
						GL.Vertex3(_x, 0, _z);
						GL.TexCoord2(ResidentialTopL, ResidentialTopB);
						GL.Vertex3(_x, 0, _z + 128.0f);
						GL.TexCoord2(ResidentialTopR, ResidentialTopB);
						GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
						GL.TexCoord2(ResidentialTopR, ResidentialTopT);
						GL.Vertex3(_x + 128.0f, 0, _z);
					} else {
						GL.TexCoord2(CommercialTopL, CommercialTopT);
						GL.Vertex3(_x, 0, _z);
						GL.TexCoord2(CommercialTopL, CommercialTopB);
						GL.Vertex3(_x, 0, _z + 128.0f);
						GL.TexCoord2(CommercialTopR, CommercialTopB);
						GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
						GL.TexCoord2(CommercialTopR, CommercialTopT);
						GL.Vertex3(_x + 128.0f, 0, _z);
					}
				}

				if(x != 31) {
					// draw street to the right
					GL.TexCoord2(StreetL, StreetT);
					GL.Vertex3(_x + 128.0f, 0, _z);
					GL.TexCoord2(StreetL, StreetB);
					GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
					GL.TexCoord2(StreetR, StreetB);
					GL.Vertex3(_x + 128.0f + StreetWidth, 0, _z + 128.0f);
					GL.TexCoord2(StreetR, StreetT);
					GL.Vertex3(_x + 128.0f + StreetWidth, 0, _z);

					if(z != 31) {
						// draw intersection to the bottom right
						GL.TexCoord2(IntersectionL, IntersectionT);
						GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
						GL.TexCoord2(IntersectionL, IntersectionB);
						GL.Vertex3(_x + 128.0f, 0, _z + 128.0f + StreetWidth);
						GL.TexCoord2(IntersectionR, IntersectionB);
						GL.Vertex3(_x + 128.0f + StreetWidth, 0, _z + 128.0f + StreetWidth);
						GL.TexCoord2(IntersectionR, IntersectionT);
						GL.Vertex3(_x + 128.0f + StreetWidth, 0, _z + 128.0f);
					}
				}

				if(z != 31) {
					// draw street underneath
					GL.TexCoord2(StreetR, StreetT);
					GL.Vertex3(_x, 0, _z + 128.0f + StreetWidth);
					GL.TexCoord2(StreetR, StreetB);
					GL.Vertex3(_x + 128.0f, 0, _z + 128.0f + StreetWidth);
					GL.TexCoord2(StreetL, StreetB);
					GL.Vertex3(_x + 128.0f, 0, _z + 128.0f);
					GL.TexCoord2(StreetL, StreetT);
					GL.Vertex3(_x, 0, _z + 128.0f);
				}

				_x += 128.0f + StreetWidth;
			}

			_z += 128.0f + StreetWidth;
		}
		GL.End();

		if(DrawCommutes && CurrentMap.Jobs != null) {
			VertexColourMaterial.SetPass(0);
			GL.Begin(GL.LINES);
			_z = 64.0f;
			// draw lines from each non empty block
			for(int z = 0; z < 32; z++) {
				float _x = 64.0f;
				for(int x = 0; x < 32; x++) {
					Block b = CurrentMap.Blocks[z, x];
					if(b.Size > 0) {
						if(b.Residential)
							GL.Color(Color.red);
						else
							GL.Color(Color.blue);
						GL.Vertex3(_x, 0, _z);
						GL.Vertex3(_x, 256.0f, _z);
					}
					
					_x += 128.0f + StreetWidth;
				}
				
				_z += 128.0f + StreetWidth;
			}

			// draw lines between every job
			GL.Color(Color.black);
			foreach(Job j in CurrentMap.Jobs) {
				GL.Vertex3 ((float)j.Home.X * (128.0f + StreetWidth) + 64.0f, 256, (float)j.Home.Z * (128.0f + StreetWidth) + 64.0f);
				GL.Vertex3 ((float)j.Work.X * (128.0f + StreetWidth) + 64.0f, 256, (float)j.Work.Z * (128.0f + StreetWidth) + 64.0f);
			}

			GL.End ();
		}

		if(DrawCongestion && CurrentMap.Jobs != null) {
			VertexColourMaterial.SetPass(0);
			GL.Begin(GL.QUADS);
			_z = 0.0f;
			for(int z = 0; z < 32; z++) {
				float _x = 128.0f + StreetWidth / 2.0f;
				for(int x = 0; x < 31; x++) {
					int c = CurrentMap.HorizontalStreetCongestion[x, z];
					if(c > 0) {
						if(c > 255)
							GL.Color(Color.red);
						else {
							float cl = (float)c / 255.0f;
							GL.Color(new Color(cl, 1.0f - cl, 0));
						}

						GL.Vertex3(_z - 32.0f, 0, _x);
						GL.Vertex3(_z - 32.0f, (float)c, _x);
						GL.Vertex3(_z + 128.0f + 32.0f, (float)c, _x);
						GL.Vertex3(_z + 128.0f + 32.0f, 0, _x);
						
						GL.Vertex3(_z + 128.0f + 32.0f, 0, _x);
						GL.Vertex3(_z + 128.0f + 32.0f, (float)c, _x);
						GL.Vertex3(_z - 32.0f, (float)c, _x);
						GL.Vertex3(_z - 32.0f, 0, _x);
					}

					c = CurrentMap.VerticalStreetCongestion[z, x];
					if(c > 0) {
						if(c > 255)
							GL.Color(Color.red);
						else {
							float cl = (float)c / 255.0f;
							GL.Color(new Color(cl, 1.0f - cl, 0));
						}
						
						GL.Vertex3(_x, 0, _z - 32.0f);
						GL.Vertex3(_x, (float)c, _z - 32.0f);
						GL.Vertex3(_x, (float)c, _z + 128.0f + 32.0f);
						GL.Vertex3(_x, 0, _z + 128.0f + 32.0f);

						GL.Vertex3(_x, 0, _z + 128.0f + 32.0f);
						GL.Vertex3(_x, (float)c, _z + 128.0f + 32.0f);
						GL.Vertex3(_x, (float)c, _z - 32.0f);
						GL.Vertex3(_x, 0, _z - 32.0f);
					}

					_x += 128.0f + StreetWidth;
				}
				_z += 128.0f + StreetWidth;
			}
			GL.End();
		}

		GL.PopMatrix();
	}
}
