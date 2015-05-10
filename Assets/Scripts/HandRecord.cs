using UnityEngine;
using System.Collections;
using Leap;
using System.IO;
using System;

public class HandRecord {

	//use this class to save the record of the hand
	//either to a file or some other source
	//tbd

	public bool left;

	public int[] extended;

	public Vector handCenter;

	public float pitch;
	public float yaw;
	public float roll;

	public float[] data;

	public Vector thumb;
	public Vector index;
	public Vector middle;
	public Vector ring;
	public Vector pinky;

	public HandRecord(){
		left = false;

		extended = new int[5];

		pitch = -1;
		yaw = -1;
		roll = -1;

		handCenter = new Vector ();

		data = new float[27]; //one for each piece of data that we need

		thumb = new Vector ();
		index = new Vector ();
		middle = new Vector ();
		ring = new Vector ();
		pinky = new Vector ();
	}

	public void setLeft(bool l){
		this.left = l;
	}

	public void setHandCenter(Vector handCenter){
		if (handCenter != null) {
			this.handCenter.x = handCenter.x;
			this.handCenter.y = handCenter.y;
			this.handCenter.z = handCenter.z;
		}
	}

	public void setDirections(float pitch, float yaw, float roll){
		this.pitch = pitch;
		this.yaw = yaw;
		this.roll = roll;
	}

	public void extend(Finger.FingerType i){
		switch (i) {
		case Finger.FingerType.TYPE_THUMB:
			extended[0] = 1;
			break;
		case Finger.FingerType.TYPE_INDEX:
			extended[1] = 1;
			break;
		case Finger.FingerType.TYPE_MIDDLE:
			extended[2] = 1;
			break;
		case Finger.FingerType.TYPE_RING:
			extended[3] = 1;
			break;
		case Finger.FingerType.TYPE_PINKY:
			extended[4] = 1;
			break;
		}
	}

	public void tips(Vector fpos, Finger.FingerType i){
		int j = getFingerNum (i);

		if (j != -1) {
			data [j] = fpos.x;
			data [j + 1] = fpos.y;
			data [j + 2] = fpos.z;
		}

		switch (j) {
		case 1:
			thumb = fpos;
			break;
		case 4:
			index = fpos;
			break;
		case 7:
			middle = fpos;
			break;
		case 10:
			ring = fpos;
			break;
		case 13:
			pinky = fpos;
			break;
		default:
			break;
		}
	}

	public int[] getExtended(){
		return this.extended;
	}

	public Vector getThumbPos(){
		return this.thumb;
	}

	public Vector getIndexPos(){
		return this.index;
	}

	public Vector getMiddlePos(){
		return this.middle;
	}

	public Vector getRingPos(){
		return this.ring;
	}

	public Vector getPinkyPos(){
		return this.pinky;
	}

	public int getFingerNum(Finger.FingerType i){
		switch (i) {
		case Finger.FingerType.TYPE_THUMB:
			return 1;
		case Finger.FingerType.TYPE_INDEX:
			return 4;
		case Finger.FingerType.TYPE_MIDDLE:
			return 7;
		case Finger.FingerType.TYPE_RING:
			return 10;
		case Finger.FingerType.TYPE_PINKY:
			return 13;
		default:
			return -1;
		}
	}

	/*
	public void updateRecord(){
		//method writes data to the array 

		//left or right hand
		if (this.left) {
			data [0] = 1;
		} else {
			data [0] = 0;
		}

		//fingers
		if (this.thumbEx) {
			data [1] = 1;
		} else {
			data [1] = 0;
		}

		if (this.indexEx) {
			data [2] = 1;
		} else {
			data [2] = 0;
		}

		if (this.middleEx) {
			data [3] = 1;
		} else {
			data [3] = 0;
		}

		if (this.ringEx) {
			data [4] = 1;
		} else {
			data [4] = 0;
		}

		if (this.pinkyEx) {
			data [5] = 1;
		} else {
			data [5] = 0;
		}


		//hand center data
		data [6] = this.handCenter.x;
		data [7] = this.handCenter.y;
		data [8] = this.handCenter.z;

		//p, y, r
		data [9] = this.pitch;
		data [10] = this.yaw;
		data [11] = this.roll;


	}//end write record
	*/

	public void updateAlt(){
		//left or right hand
		if (this.left) {
			data [0] = 1;
		} else {
			data [0] = 0;
		}

		//hand center data
		data [16] = this.handCenter.x;
		data [17] = this.handCenter.y;
		data [18] = this.handCenter.z;
		
		//p, y, r
		data [19] = this.pitch;
		data [20] = this.yaw;
		data [21] = this.roll;

		data [22] = this.extended [0];
		data [23] = this.extended [1];
		data [24] = this.extended [2];
		data [25] = this.extended [3];
		data [26] = this.extended [4];
	}

	public void writeData(){
		string fileName = "testData.csv";
		//probably should change the filename every time just to show what you are doing!
		//also just for safe keeping
		//maybe use unity input field? it would be cool to show

		using (StreamWriter file = File.AppendText(fileName))
		{
			foreach (float piece in this.data)
			{
				file.Write(piece + ",");
			}
			file.WriteLine ("");
		}
	}
}
