using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class Game2Players : MonoBehaviour {

	public Text gameTextDisplay1;
	public Text gameTextDisplay2;

	//Z coordinate of wrist for when to do the classification
	public float playZ = 50;

	enum States { Preparing, Playing };
	private States state;

	protected Controller leap_controller_;

	private int move1 = -1;
	private int move2 = -1;

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

			if (leap_controller_.Frame ().Hands.Count == 0) {
				resultMessage = "";
				state = States.Playing;
			}
			break;
		case States.Playing:
			gameTextDisplay1.text = "Now make your gestures and place your hands in the center";

			Frame frame = leap_controller_.Frame();
			HandList hands = frame.Hands;
			if (hands.Count >= 2){
				Hand firstHand = hands[0];
				Hand secondHand = hands[1];

				Hand leftmost = frame.Hands.Leftmost;
				if(leftmost.Id != firstHand.Id){
					secondHand = firstHand;
					firstHand = leftmost;
				}

				float z1 = firstHand.WristPosition.z;
				float z2 = secondHand.WristPosition.z;

				int move1 = classifier.getClassification(firstHand);
				int move2 = classifier.getClassification(secondHand);


				if (move1 == -1 || move2 == -1  ){
					gameTextDisplay1.text = "Invalid gesture!";
				} 
				else if (z1 <= playZ){
					resultMessage = "Player 1 played " + moveNames[move1-1]+". Player 2 played " + moveNames[move2-1]+". ";
					state = States.Preparing;
					int result = mechanic.decision(move1,move2);
					switch(result){
						case(0):	
							resultMessage = resultMessage + "Draw!";
							break;
						case(1):
							resultMessage = resultMessage + "Player 1 wins!";
							break;
						case(2):
							resultMessage = resultMessage + "Player 2 wins!";
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
