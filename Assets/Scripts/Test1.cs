using UnityEngine;
using System.Collections;
using Leap;

public class Test1 : MonoBehaviour {
	protected Controller controller;


	void Awake() {
		controller = new Controller();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = controller.Frame();
		// do something with the tracking data in the frame...
		HandList hands = frame.Hands;
		PointableList pointables = frame.Pointables;
		FingerList fingers = frame.Fingers;
		ToolList tools = frame.Tools;

//		foreach (Hand hand in hands) {
//			Debug.Log (hand.ToString ());
//		}
		if (hands.Count >= 1){
			Hand firstHand = hands[0];
			Debug.Log (firstHand.IsRight);
			foreach(Finger finger in firstHand.Fingers){
				Debug.Log (finger.TipPosition.z);
			}
		}
	}
}
