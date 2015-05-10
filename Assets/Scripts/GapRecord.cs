using UnityEngine;
using System.Collections;
using System;

public class GapRecord{

	public float a;
	public float b;
	public float c;
	public float d;

	public GapRecord(float a, float b, float c, float d){
		this.a = a;
		this.b = b;
		this.c = c;
		this.d = d;
	}

	public double norm2(GapRecord other){
		double diffA = Math.Pow (this.a - other.a,2);
		double diffB = Math.Pow (this.b - other.b,2);
		double diffC = Math.Pow (this.c - other.c,2);
		double diffD = Math.Pow (this.d - other.d,2);

		double sumall = diffA + diffB + diffC + diffD;
		double result = Math.Sqrt (sumall);

		return result;
	}

	public double norm1(GapRecord other){
		double diffA = Math.Abs (this.a - other.a);
		double diffB = Math.Abs(this.b - other.b);
		double diffC = Math.Abs (this.c - other.c);
		double diffD = Math.Abs (this.d - other.d);
		
		double sumall = diffA + diffB + diffC + diffD;
		
		return sumall;
	}
	
	public string ToString(){
		return a + ", " + b + ", " + c + ", " + d;
	}
}
