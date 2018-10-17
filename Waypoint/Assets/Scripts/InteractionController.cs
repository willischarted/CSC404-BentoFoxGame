using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour {

	
	public Text interactionText;
	float heldDuration = 0f;

	bool setImpulse;

	bool setHealing;

	private playerController pController;

	private List<GameObject> monsters;

	public GameObject currentTarget;

	public GameObject targetMonster;

	public GameObject targetTraveller;

	public float stunCost;





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
        
		//Debug.Log(currentTarget);

		if (Input.GetMouseButton(0) || Input.GetButton("X")) {
			heldDuration += Time.deltaTime;
			if (heldDuration > 0.5f) { //&& !setHealing) {
				//start healing
				//setHealing = true;
				if (targetTraveller != null && targetTraveller.tag=="Traveller") {
					travellerScript tScript = targetTraveller.GetComponent<travellerScript>();
					if (pController.getResource() > 0) {
					tScript.increaseCape();
					pController.addResource(-0.1f);
					}
					return;
				}

			
					
			}
			
		}

		if (Input.GetMouseButtonUp(0) ||  Input.GetButtonUp("X")) {
			//Debug.Log(heldDuration);
			

			//0.2f is general approximation of a tap
			if (heldDuration <= 0.5f) {
				//start impulse
				
				//call stun enemy function
				if (currentTarget != null && currentTarget.tag == "Switch") {
						pController.setTargetLight(currentTarget);
					
					
				}

				if (targetMonster != null && targetMonster.tag=="Monster") {
					setStun();
					return;
				}
				
			}
			heldDuration = 0f;
			//if (setHealing)
			//	setHealing = false;
		}

		if (currentTarget) {
			interactionText.text = "Press X to interact with Light Source";
			return;
		}

		if (targetMonster && currentTarget) {
			interactionText.text = "Press X to stun Monster \n Hold X to transfer light to Traveller";
			return;
		}


		if (targetMonster) {
			interactionText.text = "Press X to stun Monster";
			return;
		}
		if (targetTraveller) {
			interactionText.text = "Hold X to transfer light to Traveller";
			return;
		}
		
		else {
			interactionText.text = "";
			return;
		}
		
	}

	/*
	//check performace later
	void OnTriggerStay(Collider other)
    {
		
		if (other.tag == "Traveller") {
			currentTarget = other.gameObject;
			interactionText.text = "Hold X to transfer light to Traveller";
			return;
		}

		if (other.tag == "Monster") {
			currentTarget = other.gameObject;
			interactionText.text = "Press X to stun Monster";
			return;
		}


		if (other.tag == "Switch") {
			currentTarget = other.gameObject;
			interactionText.text = "Press X to interact with Light Source";
			return;
		}


	}
	*/
	void OnTriggerEnter(Collider other) {
	//	Debug.Log(other.name);
	//	if (other.tag == "Monster") {
			//add to array
			//monsters.Add(other.gameObject);
	//	}

		if (other.tag == "Traveller") {
			targetTraveller = other.gameObject;
			//interactionText.text = "Hold X to transfer light to Traveller";
			return;
		}

		if (other.tag == "Monster") {
			//currentTarget = other.gameObject;
			targetMonster = other.gameObject;
			//interactionText.text = "Press X to stun Monster";
			return;
		}


		if (other.tag == "Switch") {
			currentTarget = other.gameObject;
			//interactionText.text = "Press X to interact with Light Source";
			return;
		}

	}

	void OnTriggerExit(Collider other) {

		if (other.tag == "Traveller" && other.gameObject == targetTraveller) {
			//currentTarget = null;
			targetTraveller = null;
			//interactionText.text  = "";
			return;
		}
		if (other.tag == "Switch" && other.gameObject == currentTarget) {
			currentTarget = null;
			//interactionText.text  = "";
			return;
		}
		if (other.tag == "Monster" && other.gameObject == targetMonster) {
			//currentTarget = null;
			targetMonster = null;
			//interactionText.text  = "";
			return;
		}

		
	}

	void setStun() {
		//for each monster in array
		Animator anim = targetMonster.GetComponent<Animator>();
			if (anim == null) {
				Debug.Log("Could not find anim");
			}
			anim.SetTrigger("isStunned");
			pController.addResource(-stunCost);
		
		// get animator contoller and set stun
	}


}
