/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using Leap;

// Overall Controller object that will instantiate hands and tools when they appear.
public class ComputerHandController : HandController {


	private List<Frame> frames;
	private int framesCount;
//	

	public float initialZ = 250;
	public float endZ = 0;

	override protected void Start() {
		base.Start();
		if (enableRecordPlayback && recordingAsset != null) {
			framesCount = recorder_.GetFramesCount();
			frames = recorder_.GetFrames ();
		}
	}

	override protected void Update() {
		base.Update();
		if (leap_controller_ == null)
			return;
		Frame frame = leap_controller_.Frame();
		HandList hands = frame.Hands;

		if (hands.Count >= 1){
			Hand firstHand = hands[0];
			float z = firstHand.WristPosition.z;
			Debug.Log ( z);
			Debug.Log ( (z-initialZ)/(endZ-initialZ));
		}
		//
	}
	
	public override Frame GetFrame() {
		if (enableRecordPlayback && recorder_.state == RecorderState.Playing) {
			Frame frame = leap_controller_.Frame();
			HandList hands = frame.Hands;
			float t = 0;
			if (hands.Count >= 1){
				Hand firstHand = hands[0];
				float z = firstHand.WristPosition.z;
				t = (z-initialZ)/(endZ-initialZ);
				t = Mathf.Min(1, Mathf.Max(t,0));
			}
			int frameIndex = Mathf.Min(((int)Mathf.Floor(framesCount*t)), framesCount-1);


			return frames[frameIndex];
		}

		return leap_controller_.Frame();
	}
}
