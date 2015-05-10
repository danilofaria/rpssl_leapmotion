using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class Game : MonoBehaviour {

	public Text gameTextDisplay1;
	public Text gameTextDisplay2;

	//Z coordinate of wrist for when to do the classification
	public float playZ = 50;

	public GameObject computer;
	private int computerMove=0;

	enum States { Preparing, Playing };
	private States state;

	protected Controller leap_controller_;


	// Rock, Paper, Scissors, Lizard, Spock 
	private List<string> moveNames = new List<string> { "Rock", "Paper", "Scissors", "Lizard", "Spock" };

	private GestureClassifier classifier;
	private GameMechanic mechanic;

	private string resultMessage="";
	
	void Awake() {
		leap_controller_ = new Controller();
	}

	// Use this for initialization
	void Start () {
		gameTextDisplay1.text = "";
		gameTextDisplay2.text = "";
		state =  States.Preparing;
		classifier = new GestureClassifier ();
		mechanic = new GameMechanic ();
	
	}
	
	// Update is called once per frame
	void Update () {
		gameTextDisplay2.text = resultMessage;

		switch (state)
		{
		case States.Preparing:

			if (resultMessage == "")
				gameTextDisplay1.text = "Remove your hands off the screen to start";
			else 
				gameTextDisplay1.text = "Remove your hands off the screen to restart";
//			resultMessage

			if (leap_controller_.Frame ().Hands.Count == 0) {
				resultMessage = "";
				state = States.Playing;
				computer.transform.GetChild (computerMove).gameObject.SetActive (false);
				computer.transform.GetChild (computerMove).GetComponent<ComputerHandController>().DestroyAllHands();
				computerMove = Random.Range(0,5);
				computer.transform.GetChild (computerMove).gameObject.SetActive (true);
			}
			break;
		case States.Playing:
			gameTextDisplay1.text = "Now make your gesture and place your hand in the center";

			Frame frame = leap_controller_.Frame();
			HandList hands = frame.Hands;
			if (hands.Count >= 1){
				Hand firstHand = hands[0];
				float z = firstHand.WristPosition.z;
				int move = classifier.getClassification(firstHand);
				if (move == -1){
					gameTextDisplay1.text = "Invalid gesture!";
				} 
				else if (z <= playZ){
					resultMessage = "You played " + moveNames[move-1]+". Computer played " + moveNames[computerMove]+". ";
//					gameTextDisplay.text 
					state = States.Preparing;
					int result = mechanic.decision(move,computerMove+1);
					switch(result){
						case(0):	
							resultMessage = resultMessage + "Draw!";
							break;
						case(1):
							resultMessage = resultMessage + "Player wins!";
							break;
						case(2):
							resultMessage = resultMessage + "Computer wins!";
							break;
						default:
							break;
					}
				}
			}

			break;
		default:
			break;
		}

	}
}
