using UnityEngine;
using System.Collections;
using Leap;
using System.IO;
using System.Linq;


public class Classifier {

	float[] rock;
	float[] paper;
	float[] scissors;
	float[] lizard;
	float[] spock;
	Vector handCenter;

	float[,] handData;
	int[] yi;

	public Classifier(Vector handCenter){
		rock = new float[4];
		paper = new float[4];
		scissors = new float[4];
		lizard = new float[4];
		spock = new float[4];
		readData ();

		this.handCenter = handCenter;

		handData = new float[60,4];
		yi = new int[60];
		readLearning ();
		//Debug.Log ("Y: " + yi [45]);
		//Debug.Log ("Test D: " + handData[40, 2]);
	}

	public int classify(HandRecord handRecord, int classMode){
		int[] fex = handRecord.getExtended ();

		int tx = fex [0];
		int ix = fex [1];
		int mx = fex [2];
		int rx = fex [3];
		int px = fex [4];


		//Debug.Log ("Fingers: " + tx + " " + ix + " " + mx + " " + rx + " " + px);
		int allex = tx + ix + mx + rx + px;

		float gap0 = handRecord.getThumbPos ().DistanceTo (handRecord.getIndexPos ());
		float gap1 = handRecord.getIndexPos ().DistanceTo (handRecord.getMiddlePos ());
		float gap2 = handRecord.getMiddlePos ().DistanceTo (handRecord.getRingPos ());
		float gap3 = handRecord.getRingPos ().DistanceTo (handRecord.getPinkyPos ());

		float sumgap = gap0 + gap1 + gap2 + gap3;

		//float scisNorm = gap1 / sumgap;
		//float spockNorm = gap2 / sumgap;
		//float spockTest = (gap1 + gap3) / sumgap;

		//Debug.Log (gap2.ToString ());

		Vector lizthumb = handRecord.getThumbPos ();
		Vector lizindex = handRecord.getIndexPos ();

		/** 
		 * Hierarchy of Tree
		 * Rock - Scissors --> easy to determine
		 * Spock
		 * Lizard is a special type of Paper, so needs to be checked first
		 * */

		/*
		if (tx == 1) {
			Debug.Log ("Thumb is Extended");
		}*/


		float ngap0 = gap0 / sumgap;
		float ngap1 = gap1 / sumgap;
		float ngap2 = gap2 / sumgap;
		float ngap3 = gap3 / sumgap;


		bool isRock = this.classifyRock (allex);
		bool isScissors = this.classifyScissors (tx, ix, mx, rx, px, ngap1);
		bool isSpock = this.classifySpock (ix, mx, rx, px, ngap2);
		bool isLizard = this.classifyLizard (allex, lizthumb, lizindex);
		bool isPaper = this.classifyPaper (ix, mx, rx, px);

		switch (classMode) {
		case 1:
			return featureMap (isRock, isPaper, isScissors, isLizard, isSpock);
		case 2:
			return nearestNeighbor (ngap0, ngap1, ngap2, ngap3);
		case 3:
			return kNearestNeighbor (ngap0, ngap1, ngap2, ngap3);
		default:
			return -1;
		}
	}

	int featureMap(bool isRock, bool isPaper, bool isScissors, bool isLizard, bool isSpock){
		if (isRock) {
			return 1;
		} else if (isScissors) {
			return 3;
		} else if (isSpock) {
			return 5;
		} else if (isLizard) {
			return 4;
		} else if (isPaper) {
			return 2;
		} else {
			return -1;
		}
	}

	bool classifyRock(int allex){
		/**
		 * The only way to classify rock is if all the fingers are extended
		 * If any finger is extended, then it's not a rock
		 * */
		if (allex == 0) {
			return true;
		} else {
			return false;
		}
	}

	bool classifyScissors(int tx, int ix, int mx, int rx, int px, float gap){
		/**
		 * In order to be called a scissors
		 * Index and Middle need to be extended
		 * Thumb, Ring, and Pinky can't be extended
		 * Debating on whether the gap should come into play
		 * */
		int twoup = ix + mx;
		int threedown = tx + rx + px;
		if ((twoup == 2) && (threedown == 0)) {
			//&& (gap > (scissors[1] * 0.7f))
			return true;
		} else {
			return false;
		}
	}

	bool classifySpock(int ix, int mx, int rx, int px, float gap){
		/**
		 * To classify as spock:
		 * Four fingers need to be extended, we don't care if the thumb is out
		 * Needs to be a noticeable gap between the middle and ring fingers
		 * */
		int allup = ix + mx + rx + px;
		//float diff = gap1 + gap3;
		//might be better for accuracy - see how it goes

		if ((allup == 4) && (gap > (spock[2] * 0.65f))) {
			return true;
		} else {
			return false;
		}
	}
	
	bool classifyLizard(int allex, Vector thumb, Vector index){
		/**
		 * For lizard, all fingers need to be extended
		 * BUT if the thumb is in the jaw position, then it is not extended
		 * the thumb is actually important for this, so we need it
		 * the thumb then has to have a lower y value than the index finger
		 * so we know the finger is below it with relation to the floor
		 * */
		float ty = thumb.y;
		float iy = index.y + 10f; //error allowed for the thumb to be over

		float dist = thumb.DistanceTo (index);
		//Debug.Log ("Dist from Thumb to Index" + dist);

		if ((allex == 4) && (ty < iy)) {
			return true;
		} else {
			return false;
		}
	}

	bool classifyPaper(int ix, int mx, int rx, int px){
		/**
		 * Paper can be easy, but also some tricks
		 * Depending on the person, sometimes the thumb doesn't register
		 * so if four fingers are extended, and spock or lizard hasn't been called,
		 * then it may as well be paper
		 * */
		int fourup = ix + mx + rx + px;
		if (fourup == 4) {
			return true;
		} else {
			return false;
		}
	}

	int nearestNeighbor(float gap0, float gap1, float gap2, float gap3){
		GapRecord primary = new GapRecord (gap0, gap1, gap2, gap3);
		GapRecord secondary;
		int rows = handData.GetLength (0);

		int result = -1;
		double minvalue = 10000;
		double dist;

		for (int i = 0; i < rows; i++) {
			secondary = new GapRecord(handData[i,0], handData[i,1], handData[i,2], handData[i,3]);
			dist = primary.norm2(secondary);
			if(dist < minvalue){
				minvalue = dist;
				result = yi[i];
			}
		}

		//Debug.Log ("GAP: " + primary.ToString());
		return result;
	}


	int kNearestNeighbor(float gap0, float gap1, float gap2, float gap3){
		GapRecord primary = new GapRecord (gap0, gap1, gap2, gap3);
		GapRecord secondary;
		int rows = handData.GetLength (0);

		int[] results = new int[5] {-1, -1, -1, -1, -1};
		double[] min = new double[5] {10000, 10000, 10000, 10000, 10000};
		double dist;

		//Debug.Log (rows);
		for (int i = 0; i < rows; i++) {
			secondary = new GapRecord(handData[i,0], handData[i,1], handData[i,2], handData[i,3]);
			dist = primary.norm2(secondary);
			if(dist < min[4]){
				min[4] = dist;
				results[4] = yi[i];
			}
			if(dist < min[3]){
				min[4] = min[3];
				results[4] = results[3];
				min[3] = dist;
				results[3] = yi[i];
			}
			if(dist < min[2]){
				min[3] = min[2];
				results[3] = results[2];
				min[2] = dist;
				results[2] = yi[i];
			}
			if(dist < min[1]){
				min[2] = min[1];
				results[2] = results[1];
				min[1] = dist;
				results[1] = yi[i];
			}
			if(dist < min[0]){
				min[1] = min[0];
				results[1] = results[0];
				min[0] = dist;
				results[0] = yi[i];
			}
		}
		
		//Debug.Log ("GAP: " + primary.ToString());
		Debug.Log ("MODE: " + results[0] + " " + results[1] + " " + results[2] + " " + results[3] + " " + results[4]);

		int result = modeFunction (results);

		return result;
	}
	
	int modeFunction(int[] classes){
		/*
		 * 	//http://stackoverflow.com/questions/8260555/how-to-find-the-mode-in-array-c
		int mode = classes.GroupBy(v => v)
				.OrderByDescending(g => g.Count())
				.First()
				.Key;
		*/

		//http://stackoverflow.com/questions/13755007/c-sharp-find-highest-array-value-and-index
		int y = -1;
		int[] possible = new int[5]{0, 0, 0, 0, 0};
		Debug.Log ("CL: " + classes.Length);
		for (int i = 0; i < classes.Length; i++) {
			y = classes[i] - 1; //-1 to account for 
			possible[y]++;
		}

		int maxValue = possible.Max ();
		if (maxValue == 1) {
			return classes[0]; //if the mode is 1, then return the closest
		}

		int mode = possible.ToList ().IndexOf (maxValue) + 1;

		return mode;
	}


	void readData(){
		string filePath = "Data/relative2.csv";
		string[] values = File.ReadAllText(filePath).Split(',');
		//right now using relative distance between tips

		for (int i = 0; i < 4; i++) {
			int j = 5 * i;
			rock[i] = float.Parse(values[j]);
			paper[i] = float.Parse (values[j+1]);
			scissors[i] = float.Parse (values[j+2]);
			lizard[i] = float.Parse (values[j+3]);
			spock[i] = float.Parse (values[j+4]);
		}
		//Debug.Log (scissors [1].ToString ());
	}

	void readLearning(){
		string filePath = "Data/learning2.csv";
		string[] values = File.ReadAllText(filePath).Split(',');
		//right now using relative distance between tips

		int k = 0;
		int nums = 60;
		for (int i = 0; i < 4; i++) {
			//int j = 5 * i;
			for (int j = 0; j < nums; j++){
				k = j + (nums*i);
				handData[j,i] = float.Parse(values[k]);
				//yi[i] = int.Parse(values[j+4]);
			}
		}

		int xx = 0;
		for (int x = 240; x < 300; x++) {
			xx = x - 240;
			yi[xx] = int.Parse (values[x]);
		}
		//Debug.Log (scissors [1].ToString ());
	}

}
