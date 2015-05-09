using UnityEngine;
using System.Collections;
using Leap;

public class ClickCube : MonoBehaviour {


	private int nClicks = 0;
	public string scene;

	private GameObject bar;

	private float secsHoldingButton=0;

	protected Controller leap_controller_;
//	private m 

	void Awake() {
		leap_controller_ = new Controller();
	}

	// Use this for initialization
	void Start () {
		bar = transform.GetChild (1).gameObject;
		bar.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (leap_controller_.Frame ().Hands.Count == 0) {
			nClicks = 0;
			secsHoldingButton = 0;
			bar.SetActive (false);
		}

		if(nClicks == 0){
			bar.SetActive (false);
			bar.transform.localScale = new Vector3(0,0.1f,0.1f);
		}else {
			secsHoldingButton += Time.deltaTime;
//			bar.localScale = new Vector3(1,0.1f,0.1f);
			bar.SetActive (true);
			bar.transform.localScale = new Vector3(secsHoldingButton,0.1f,0.1f);
		}

		if (secsHoldingButton > 1) {
			Application.LoadLevel(scene);
		}
	}

	void OnTriggerEnter (Collider col) {
		if(nClicks == 0){
			transform.GetChild (0).position += new Vector3(0,0,0.1f);
			transform.GetChild (0).GetComponent<Renderer>().material.color = new Color(0.5f,0.5f,0.5f);
		}

		nClicks++;
	}

	void OnTriggerExit (Collider col) {
		nClicks--;
		if (nClicks == 0) {
			transform.GetChild (0).position -= new Vector3 (0, 0, 0.1f);
			transform.GetChild (0).GetComponent<Renderer>().material.color = new Color (1, 1, 1);
			secsHoldingButton = 0;
			bar.SetActive (false);
		}
	}


//	void buttonDown(){
//	
//	}




}
