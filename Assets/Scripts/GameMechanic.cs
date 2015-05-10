using UnityEngine;
using System.Collections;

public class GameMechanic {
	
	int player1;
	int player2;

	//No Player - Test Mode
	public GameMechanic(){
		this.player1 = Random.Range (0, 5) + 1;
		this.player2 = Random.Range (0, 5) + 1;
	}

	//Single Player
	public GameMechanic(int player1){
		this.player1 = player1;
		this.player2 = Random.Range (0, 5) + 1;
	}

	//Two Player
	public GameMechanic(int player1, int player2){
		this.player1 = player1;
		this.player2 = player2;
	}

	public int decision(){
		/**
		 * Returning a -1 means that one of the gestures is invalid
		 * 	 - won't happen with nearest neighbor or KNN
		 * Returning a 0 means a draw
		 * Returning a 1 means Player 1 wins (and Player 2 loses)
		 * Returning a 2 means Player 2 wins (and Player 1 loses)
		 * */

		int p1 = this.player1;
		int p2 = this.player2;

		//rock crushes scissors and lizard
		//paper covers rock and disproves spock
		//scissors cut paper and kill lizard
		//lizard poison spock and eats paper
		//spock vaporizes rock and breaks scissors

		if (p1 == -1 || p2 == -1) {
			return -1;
		}
		//0 means draw
		//1 means 1 wins
		//2 means 2 wins
		if (p1 == 1) {
			switch (p2) {
			case 1:
				return 0; //draw
			case 2:
				return 2; //rock losed to paper
			case 3:
				return 1; //rock beats scissors
			case 4:
				return 1; //rock beats lizards
			case 5:
				return 2; //rock loses to spock
			default:
				return -1;
			}
		}//end rock
		else if (p1 == 2) {
			switch (p2) {
			case 1:
				return 1; //paper beats rock
			case 2:
				return 0; //draw
			case 3:
				return 2; //paper loses to scissors
			case 4:
				return 2; //paper loses to lizard
			case 5:
				return 1; //paper beats spock
			default:
				return -1;
			}
		}//end paper
		else if (p1 == 3) {
			switch (p2) {
			case 1:
				return 2; //scissors loses to rock
			case 2:
				return 1; //scissors beats paper
			case 3:
				return 0; //draw
			case 4:
				return 1; //scissors beats lizard
			case 5:
				return 2; //scissors loses to spock
			default:
				return -1;
			}
		}//end scissors
		else if (p1 == 4) {
			switch (p2) {
			case 1:
				return 2; //lizard loses to rock
			case 2:
				return 1; //lizard beats paper
			case 3:
				return 2; //lizard loses to scissors
			case 4:
				return 0; //draw
			case 5:
				return 1; //lizard beast spock
			default:
				return -1;
			}
		}//end lizard
		else if (p1 == 5) {
			switch (p2) {
			case 1:
				return 1; //spock beats rock
			case 2:
				return 2; //spock loses to paper
			case 3:
				return 1; //spock beats scissors
			case 4:
				return 2; //spock loses to lizard
			case 5:
				return 0; //draw
			default:
				return -1;
			}
		}//end spock
		else {
			return -1;
		}
	}

	public int alwaysWin(int p1){

		int randy = Random.Range (0, 2);
		switch (p1) {
		case 1: //rock
			if(randy ==0){
				return 2; //paper
			}
			else{
				return 5; //spock
			}
		case 2: //paper
			if(randy ==0){
				return 3; //scissors
			}
			else{
				return 4; //lizard
			}
		case 3: //scissors
			if(randy ==0){
				return 1; //rock
			}
			else{
				return 5; //spock
			}
		case 4: //lizard
			if(randy ==0){
				return 1; //rock
			}
			else{
				return 3; //scissors
			}
		case 5: //spock
			if(randy ==0){
				return 2; //paper
			}
			else{
				return 4; //lizard
			}
		default:
			return -1;
		}
	}

	public void printPlayers(){
		Debug.Log ("Game Results: " + player1 + ", " + player2);
	}
}
