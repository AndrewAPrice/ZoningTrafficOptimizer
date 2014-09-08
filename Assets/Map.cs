using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block {
	public bool Residential;
	public int Size; // 0 = empty
	public int X, Z;
	public int Rotation;
}

public class Job {
	public Block Home;
	public Block Work;
	public int Commute;
}

public enum OptimizerGoal {
	NoGoal,
	ShortestCommute,
	LongestCommute,
	LeastCongestion,
	MostCongestion
}

public class Map {
	public Block[,] Blocks = new Block[32,32];
	public Job[] Jobs = null;

	public int ResidentCount = 0;
	public int JobCount = 0;

	int TotalCommute;
	float AverageCommute;
	public int[] CommuteStats = new int[64];
	public int[,] HorizontalStreetCongestion = new int[31, 32];
	public int[,] VerticalStreetCongestion = new int[32, 31];

	public OptimizerGoal Goal = OptimizerGoal.ShortestCommute;
	bool UpdateCommuteStatistics = true;
	bool UpdateCongestionStatistics = true;
	int MostCongested = 0;
	int MostCongestedNumber = 0;

	public Map() {
		for(int z = 0; z < 32; z++) {
			for(int x = 0; x < 32; x++) {
				Block b = new Block();
				b.Residential = false;
				b.Size = 0;
				b.X = x;
				b.Z = z;
				b.Rotation = 0;
				Blocks[z,x] = b;
			}
		}

		Randomize ();
		UpdateCounts();
	}

	public void Randomize() {
		for(int z = 0; z < 32; z++) {
			for(int x = 0; x < 32; x++) {
				Block b = Blocks[z,x];
				b.Residential = Random.Range(0, 2) == 0;
				b.Size = Random.Range (0, 16);
				b.Rotation = 0;
			}
		}
		UpdateCounts ();
		Balance();
	}

	public void Clear() {
		for(int z = 0; z < 32; z++) {
			for(int x = 0; x < 32; x++) {
				Block b = Blocks[z,x];
				b.Residential = false;
				b.Size = 0;
				b.Rotation = 0;
			}
		}
		JobCount = 0;
		ResidentCount = 0;
		Jobs = null;
	}

	public void SetBlock(int x, int z, bool residential, int density) {
		if(x < 0 || z < 0 || x >= 32 || z >= 32)
			return;

		Block b = Blocks[z,x];
		// subtract old values
		if(b.Residential)
			ResidentCount -= b.Size;
		else
			JobCount -= b.Size;

		b.Residential = residential;
		b.Size = density;
		b.Rotation = Random.Range(0, 4);
		if(residential)
			ResidentCount += density;
		else
			JobCount += density;

		Jobs = null; // invalidate jobs because the map has changed
	}

	void UpdateCounts() {
		ResidentCount = 0;
		JobCount = 0;

		for(int z = 0; z < 32; z++) {
			for(int x = 0; x < 32; x++) {
				Block b = Blocks[z,x];
				if(b.Residential)
					ResidentCount += b.Size;
				else
					JobCount += b.Size;
			}
		}
	}

	public void AssignRandomJobs() {
		UpdateCounts();
		if(ResidentCount != JobCount) // must be equal to assign everyone a job
			return;

		// make sure UpdateCount is called before this

		// create a pool of all avaliable jobs
		List<Block> jobs = new List<Block>();
		for(int z = 0; z < 32; z++) {
			for(int x = 0; x < 32; x++) {
				Block b = Blocks[z, x];
				if(!b.Residential) {
					for(int j = 0; j < b.Size; j++) {
						jobs.Add(b);
					}
				}
			}
		}

		// create a job for each resident
		Jobs = new Job[ResidentCount];
		int i = 0;
		for(int z = 0; z < 32; z++) {
			for(int x = 0; x < 32; x++) {
				Block b = Blocks[z, x];
				if(b.Residential) {
					for(int j = 0; j < b.Size; j++) {
						int k = Random.Range(0, jobs.Count);
						Block work = jobs[k];
						jobs.RemoveAt(k);

						Job job = new Job();
						job.Home = b;
						job.Work = work;
						job.Commute = Mathf.Abs(x - work.X) + Mathf.Abs(z - work.Z);
						Jobs[i] = job;
						i++;
					}
				}
			}
		}

		UpdateCommuteStatistics = true;
	}

	public void OptimizeJobs() {
		if(JobCount < 2)
			return;

		// pick two jobs
		int j1 = Random.Range(0, JobCount);
		int j2 = Random.Range(0, JobCount - 1);
		if(j1 == j2)
			j2++;

		Job job1 = Jobs[j1];
		Job job2 = Jobs[j2];

		// calculate our new commute were we to swap jobs
		int newCommute1 = Mathf.Abs(job1.Home.X - job2.Work.X) + Mathf.Abs(job1.Home.Z - job2.Work.Z);
		int newCommute2 = Mathf.Abs(job2.Home.X - job1.Work.X) + Mathf.Abs(job2.Home.Z - job1.Work.Z);

		bool save;
		switch(Goal) {
		case OptimizerGoal.NoGoal:
		default:
			save = true;
			break;
		case OptimizerGoal.ShortestCommute:
			save = newCommute1 + newCommute2 < job1.Commute + job2.Commute;
			break;
		case OptimizerGoal.LongestCommute:
			save = newCommute1 + newCommute2 > job1.Commute + job2.Commute;
			break;
		}

		// is this better or the same for both parties?
		if(save) {
			Block t = job1.Work;
			job1.Work = job2.Work;
			job1.Commute = newCommute1;

			job2.Work = t;
			job2.Commute = newCommute2;
		}

		UpdateCommuteStatistics = true;
	}

	public void OptimizeZoning() {
		int b1 = Random.Range (0, 32 * 32);
		int b2 = Random.Range (0, 32 * 32 - 1);
		if(b1 == b2)
			b2++;

		int x1 = b1 % 32;
		int z1 = b1 / 32;

		int x2 = b2 % 32;
		int z2 = b2 / 32;

		Block block1 = Blocks[z1, x1];
		Block block2 = Blocks[z2, x2];

		// swap these blocks
		block1.X = x2;
		block1.Z = z2;

		block2.X = x1;
		block2.Z = z1;

		// see if it's better
		int oldTotalCommute = TotalCommute;

		UpdateCommuteStatistics = true;
		DoUpdateCommuteStatistics();

		bool save;
		switch(Goal) {
		case OptimizerGoal.NoGoal:
		default:
			save = true;
			break;
		case OptimizerGoal.ShortestCommute:
			save = TotalCommute < oldTotalCommute;
			break;
		case OptimizerGoal.LongestCommute:
			save = TotalCommute > oldTotalCommute;
			break;
		case OptimizerGoal.LeastCongestion: {
				int oldMostCongested = MostCongested;	
				int oldMostCongestedNumber = MostCongestedNumber;
				DoUpdateCongestionStatistics();
				if(oldMostCongested == MostCongested)
					save = MostCongestedNumber < oldMostCongestedNumber;
				else
					save = MostCongested < oldMostCongested;

				if(!save) {
					MostCongested = oldMostCongested;
					MostCongestedNumber = oldMostCongestedNumber;
				}
			break;
			}
		case OptimizerGoal.MostCongestion: {
			int oldMostCongested = MostCongested;	
			int oldMostCongestedNumber = MostCongestedNumber;
			DoUpdateCongestionStatistics();
			if(oldMostCongested == MostCongested)
				save = MostCongestedNumber > oldMostCongestedNumber;
			else
				save = MostCongested > oldMostCongested;
			
			if(!save) {
				MostCongested = oldMostCongested;
				MostCongestedNumber = oldMostCongestedNumber;
			}
			break;
		}
		}
		
		if(save) {
			// make it permanent
			Blocks[z1, x1] = block2;
			Blocks[z2, x2] = block1;
		} else {
			// swap it back
			block1.X = x1;
			block1.Z = z1;
			
			block2.X = x2;
			block2.Z = z2;
			
			UpdateCommuteStatistics = true;
			TotalCommute = oldTotalCommute;
		}
	}

	public void Balance() {
		if(ResidentCount == JobCount)
			return;

		Jobs = null;
		for(int i = 0; i < 10000 && JobCount != ResidentCount; i++) { 
			// pick a block at random
			int x = Random.Range (0, 32);
			int z = Random.Range (0, 32);

			Block b = Blocks[z, x];
			if(b.Residential) {
				// is the difference so great we should swap this type?
				if(ResidentCount > JobCount + 30) {
					b.Residential = false;
					ResidentCount -= b.Size;
					JobCount += b.Size;
				} else {
					if(ResidentCount > JobCount) {
						// too many residents
						if(b.Size > 0) {
							b.Size--;
							ResidentCount--;
						}
					} else {
						// too many jobs
						if(b.Size < 15) {
							b.Size++;
							ResidentCount++;
						}
					}
				}
			} else {
				// is the difference so great we should swap this type?
				if(JobCount > ResidentCount + 30) {
					b.Residential = true;
					JobCount -= b.Size;
					ResidentCount += b.Size;
				} else {
					if(JobCount > ResidentCount) {
						// too many jobs
						if(b.Size > 0) {
							b.Size--;
							JobCount--;
						}
					} else {
						// too many residents
						if(b.Size < 15) {
							b.Size++;
							JobCount++;
						}
					}
				}
			}
		}
	}

	public void GetConguestionStatistics(out int mostcongested, out int congestednumber) {
		if(UpdateCongestionStatistics)
			DoUpdateCongestionStatistics();
		mostcongested = MostCongested;
		congestednumber = MostCongestedNumber;
	}

	public void DoUpdateCongestionStatistics() {
		if(UpdateCommuteStatistics)
			DoUpdateCommuteStatistics();
		
		for(int z = 0; z < 31; z++) {
			for(int x = 0; x < 32; x++) {
				HorizontalStreetCongestion[z,x] = 0;
				VerticalStreetCongestion[x,z] = 0;
			}
		}

		foreach(Job j in Jobs) {
			int xDiff = Mathf.Abs(j.Home.X - j.Work.X);
			int zDiff = Mathf.Abs(j.Home.Z - j.Work.Z);

			// figure out the street congestion, travel longest axis first
			if(xDiff > zDiff) {
				if(zDiff == 0) {
					// straight horizontal line
					int z = j.Home.Z;
					if(z == 31) z = 30;
					
					int minX = Mathf.Min (j.Home.X, j.Work.X);
					int maxX = Mathf.Max (j.Home.X, j.Work.X);
					for(int x = minX; x <= maxX; x++)
						HorizontalStreetCongestion[z,x]++;
				} else {

					if(j.Home.X < j.Work.X) {
						if(j.Home.Z < j.Work.Z) {
							// go right then down
							for(int x = j.Home.X; x < j.Work.X; x++)
								HorizontalStreetCongestion[j.Home.Z,x]++;
							for(int z = j.Home.Z + 1; z <= j.Work.Z; z++)
								VerticalStreetCongestion[z, j.Work.X - 1]++;
						}
						else {
							// go right then up
							for(int x = j.Home.X; x < j.Work.X; x++)
								HorizontalStreetCongestion[j.Home.Z - 1,x]++;
							for(int z = j.Work.Z; z < j.Home.Z; z++)
								VerticalStreetCongestion[z, j.Work.X - 1]++;
						}
					}
					else {
						if(j.Home.Z < j.Work.Z) {
							// go left then down
							for(int x = j.Work.X + 1; x <= j.Home.X; x++)
								HorizontalStreetCongestion[j.Home.Z,x]++;
							for(int z = j.Home.Z + 1; z <= j.Work.Z; z++)
								VerticalStreetCongestion[z, j.Work.X]++;
						}
						else {
							// go left then up
							for(int x = j.Work.X + 1; x <= j.Home.X; x++)
								HorizontalStreetCongestion[j.Home.Z - 1,x]++;
							for(int z = j.Work.Z; z < j.Home.Z; z++)
								VerticalStreetCongestion[z, j.Work.X]++;
						}
					}
				}
			} else {
				if(xDiff == 0) {
					// straight vertical line
					int x = j.Home.X;
					if(x == 31) x = 30;
					
					int minZ = Mathf.Min (j.Home.Z, j.Work.Z);
					int maxZ = Mathf.Max (j.Home.Z, j.Work.Z);

					for(int z = minZ; z <= maxZ; z++)
						VerticalStreetCongestion[z,x]++;
				} else {
					if(j.Home.X < j.Work.X) {
						if(j.Home.Z < j.Work.Z) {
							// go down then right
							for(int z = j.Home.Z; z < j.Work.Z; z++)
								VerticalStreetCongestion[z, j.Home.X]++;
							for(int x = j.Home.X + 1; x <= j.Work.X; x++)
								HorizontalStreetCongestion[j.Work.Z - 1,x]++;
						}
						else {
							// go up then right
							for(int z = j.Work.Z + 1; z <= j.Home.Z; z++)
								VerticalStreetCongestion[z, j.Home.X]++;
							for(int x = j.Home.X + 1; x <= j.Work.X; x++)
								HorizontalStreetCongestion[j.Work.Z,x]++;
						}
					}
					else {
						if(j.Home.Z < j.Work.Z) {
							// go down then left
							for(int z = j.Home.Z; z < j.Work.Z; z++)
								VerticalStreetCongestion[z, j.Home.X - 1]++;
							for(int x = j.Work.X; x < j.Home.X; x++)
								HorizontalStreetCongestion[j.Work.Z - 1,x]++;
						}
						else {
							// go up then left
							for(int z = j.Work.Z + 1; z <= j.Home.Z; z++)
								VerticalStreetCongestion[z, j.Home.X - 1]++;
							for(int x = j.Work.X; x < j.Home.X; x++)
								HorizontalStreetCongestion[j.Work.Z,x]++;
						}
					}
				}
			}
		}

		MostCongested = 0;
		MostCongestedNumber = 0;
	
		for(int z = 0; z < 31; z++) {
			for(int x = 0; x < 32; x++) {
				int c = HorizontalStreetCongestion[z,x];
				if(c > MostCongested) {
					MostCongested = c;
					MostCongestedNumber = 1;
				} else if(c == MostCongested)
					MostCongestedNumber++;

				c = VerticalStreetCongestion[x,z];
				if(c > MostCongested) {
					MostCongested = c;
					MostCongestedNumber = 1;
				} else if(c == MostCongested)
					MostCongestedNumber++;
			}
		}

		UpdateCongestionStatistics = false;
	}
	
	void DoUpdateCommuteStatistics() {
		for(int i = 0; i < 64; i++)
			CommuteStats[i] = 0;
			
			
		TotalCommute = 0;
		foreach(Job j in Jobs) {
			int xDiff = Mathf.Abs(j.Home.X - j.Work.X);
			int zDiff = Mathf.Abs(j.Home.Z - j.Work.Z);
			j.Commute = xDiff + zDiff;
			TotalCommute += j.Commute;
			CommuteStats[j.Commute]++;
		}
			
		AverageCommute = (float)TotalCommute / (float)JobCount;
			
		UpdateCommuteStatistics = false;
		UpdateCongestionStatistics = true;
	}

	public void GetCommuteStatistics(out int totalCommute, out float averageCommute) {
		if(UpdateCommuteStatistics)
			DoUpdateCommuteStatistics();

		totalCommute = TotalCommute;
		averageCommute = AverageCommute;
	}
}
