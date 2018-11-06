using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HintsController : MonoBehaviour {

	public GameObject promptIcon;
	public Text promptText;

	public GameObject hintsBox;


	public bool setHintsBox = false;


	// Use this for initialization
	void Start () {
		Invoke("turnOnPrompt", 0.5f);
		Invoke("turnOffPrompt", 3.0f);

		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("TouchPad")) {
			setHintsBox = !setHintsBox;
			hintsBox.SetActive(setHintsBox);

			if (setHintsBox){
				promptIcon.SetActive(true);
				promptText.text = "Hide Hints";
			}

			else {
				promptIcon.SetActive(false);
			}
			//Invoke("turnOffPrompt", 2.5f);
			

		}
		
	}

	void turnOffPrompt() {
		promptIcon.SetActive(false);
		
	}

	void turnOnPrompt() {
		promptIcon.SetActive(true);
		
	}
}
