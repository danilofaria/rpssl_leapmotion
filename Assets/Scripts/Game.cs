using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Leap;

public class Game : MonoBehaviour {

	protected Controller leap_controller_;

//	[SerializeField]
	public Text gameTextDisplay;
	
	void Awake() {
		leap_controller_ = new Controller();
	}

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		if (leap_controller_.Frame ().Hands.Count == 0) {
			gameTextDisplay.text = "No hand";
		} else
		gameTextDisplay.text = "Hand";
	}
}
