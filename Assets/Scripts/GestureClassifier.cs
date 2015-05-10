using UnityEngine;
using System.Collections;
using Leap;

public class GestureClassifier {
	Controller controller;

	public GestureClassifier(){
		this.controller = new Controller ();
	}

	public GestureClassifier(Controller controller){
		this.controller = controller;
	}

	public int getClassification(Hand hand){
		Frame frame = controller.Frame ();

		int result = snapshot (hand, frame);
		return result; //this is the result of the hand, not the game
	}

	/**
	 * Idea from this method is to get all the data we can get from the sensor
	 * then, we will write it to a file that will be learned on in Matlab
	 * then, passed back to another method here that will have a method to classify
	 * 
	 */
	int snapshot(Hand hand1, Frame frame){

			//Hand hand1 = hands [h];
			FingerList fingers = hand1.Fingers;
			HandRecord record = new HandRecord ();
			
			for (int j = 0; j < fingers.Count; j++) {
				Finger f1 = fingers [j];
				Finger.FingerType fingtype = f1.Type ();
				
				if (f1.IsExtended) {
					record.extend (fingtype);
				}
				
				Vector fpos = f1.TipPosition;
				record.tips(fpos, fingtype);
			}
			
			if(hand1.IsLeft){
				record.left = true;
			}
			
			float pitch = hand1.Direction.Pitch;
			float yaw = hand1.Direction.Yaw;
			float roll = hand1.PalmNormal.Roll;
			record.setDirections(pitch, yaw, roll);
			
			Vector handCenter = hand1.PalmPosition;
			record.setHandCenter(handCenter);
			
			Classifier cf = new Classifier(handCenter);
			
			
			record.updateAlt ();
			//record.writeData ();
			
			//1 is Feature Detection
			//2 is Nearest Neighbor
			//3 is K-Nearest Neighbor
			int result = cf.classify(record, 1);

			Debug.Log ("Result: " + result.ToString());
			
			//float confid = hand1.Confidence;
			//confSum += confid;
			//writeResultData(result, confid);
			
			//GameMechanic game = new GameMechanic (result);
			//game.printPlayers ();
			//Debug.Log (game.decision ());

		return result;
		
		//test randoms
		//in theory - should be good to use with classifier now
		//GameMechanic game = new GameMechanic ();
		//game.printPlayers ();
		//Debug.Log (game.decision ());
		
	}//end get hand data


}
