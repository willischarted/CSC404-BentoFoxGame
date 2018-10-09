using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {

	float heldDuration = 0f;

	bool setImpulse;

	bool setHealing;

	private playerController pController;

	//private monsterscript[];

	// Use this for initialization
	void Start () {
		//impulseCooldown = 5.0f;
		//setImpulse = false;
		setHealing = false;
		pController = GetComponentInParent<playerController>();
		if (pController == null) {
			Debug.Log("Could not find pController");
		}
	}
	
	// Update is called once per frame
	void Update () {

		// Heal when held in vicinity of monster
		// impulse stun when tapped
        


		if (Input.GetButton("R1") || Input.GetMouseButton(1)) {
			heldDuration += Time.deltaTime;
			if (heldDuration > 0.2f && !setHealing) {
				//start healing
				setHealing = true;
			}
		}

		if (Input.GetButtonUp("R1") || Input.GetMouseButtonUp(1)) {
			Debug.Log(heldDuration);
			heldDuration = 0f;

			//0.2f is general approximation of a tap
			if (heldDuration <= 0.2f) {
				//start impulse
				
				//call stun enemy function
				
			}
			if (setHealing)
				setHealing = false;
		}


		
	}

	void OnTriggerStay(Collider other)
    {

		

		if (other.tag == "Traveller" && setHealing) {
			//call healing function in traveller's script
			travellerScript tScript = other.GetComponent<travellerScript>();
			if (pController.getResource() > 0) {
				tScript.increaseCape();
				pController.addResource(-0.1f);
			}

			
		}


	}
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Monster") {
			//add to array
		}
	}

	void OnTriggerExit(Collider other) {

		//remove from array
		
	}

	void setStun() {
		//for each monster in array

		// get animator contoller and set stun
	}


}
