using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class InteractionControllerCopy : MonoBehaviour {

	
	public Text interactionText;
	float heldDuration = 0f;

	bool setImpulse;

	bool setHealing;

	private playerControllerCopy pController;

	private List<GameObject> monsters;

	public GameObject currentTarget;

	public GameObject targetMonster;

	public GameObject targetTraveller;

	public float stunCost;

	public GameObject interactionPopUp;
	private WorldSpaceObjectController popUpController;
	public Text popUpText;

	public int textVerticalOffset;




	private bool healUnlocked;
	private bool stunUnlocked;


	 



	// Use this for initialization
	void Start () {
		//impulseCooldown = 5.0f;
		//setImpulse = false;
		setHealing = false;
		pController = GetComponentInParent<playerControllerCopy>();
		if (pController == null) {
			Debug.Log("Could not find pController");
		}

		popUpController = interactionPopUp.GetComponent<WorldSpaceObjectController>();
		if (popUpController == null)
			Debug.Log("Could not find worldspacecontroller");

		
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
				if (currentTarget != null && currentTarget.tag == "LampLight") {
						pController.setTargetLight(currentTarget);
						Debug.Log("truning on light");
					
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
			//interactionText.text = "Light";
			popUpText.fontSize = 150;
			popUpText.text =   "Light";
			return;
		}

		if (targetMonster && currentTarget) {
			interactionText.text = "Press X to stun Monster \n Hold X to transfer light to Traveller";
			return;
		}


		if (targetMonster) {
			//interactionText.text = "Stun";
			popUpText.fontSize = 150;
			popUpText.text =   "Stun";
			return;
		}
		if (targetTraveller) {
			//interactionText.text = "(Hold) Heal";
			popUpText.fontSize = 90;
			popUpText.text = "Hold to Heal";
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

		if (other.tag == "Traveller" && healUnlocked) {
			targetTraveller = other.gameObject;
			//interactionText.text = "Hold X to transfer light to Traveller";
			interactionPopUp.SetActive(true);
			Vector3 popUpLocation = other.gameObject.transform.position;
			popUpLocation.y = popUpLocation.y +textVerticalOffset;
			popUpController.updateWorldObjectTransform(popUpLocation);
			controlLureImage();
			return;
		}

		if (other.tag == "Monster" && stunUnlocked) {
			//currentTarget = other.gameObject;
			targetMonster = other.gameObject;
			//interactionText.text = "Press X to stun Monster";
			interactionPopUp.SetActive(true);
			Vector3 popUpLocation = other.gameObject.transform.position;
			popUpLocation.y = popUpLocation.y +textVerticalOffset;
			popUpController.updateWorldObjectTransform(popUpLocation);
			controlLureImage();
			return;
		}


		if (other.tag == "LampLight") {
			currentTarget = other.gameObject;
			//interactionText.text = "Press X to interact with Light Source";
			interactionPopUp.SetActive(true);
			Vector3 popUpLocation = other.gameObject.transform.position;
			popUpLocation.y = popUpLocation.y + textVerticalOffset;
			popUpController.updateWorldObjectTransform(popUpLocation);
			controlLureImage();
			return;
		}

	}

	void OnTriggerExit(Collider other) {

		if (other.tag == "Traveller" && other.gameObject == targetTraveller) {
			//currentTarget = null;
			interactionPopUp.SetActive(false);
			targetTraveller = null;
			//interactionText.text  = "";
			return;
		}
		if (other.tag == "LampLight" && other.gameObject == currentTarget) {
			interactionPopUp.SetActive(false);
			currentTarget = null;
			//interactionText.text  = "";
			return;
		}
		if (other.tag == "Monster" && other.gameObject == targetMonster) {
			interactionPopUp.SetActive(false);
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

	void controlLureImage() {
		//check if light is in traveller radius
		/* 
		GameObject trav = GameObject.FindGameObjectWithTag("Traveller");
		if (trav == null)
			Debug.Log("Could not find the traveller");
		travellerScript tScript = trav.GetComponent<travellerScript>();
		if (tScript == null)
			Debug.Log("could not find tScript");
		*/
		popUpController.setTravelIcon(currentTarget);
		//get the current lamp that the traveller is at

		// get its adjacent lamps from light controller

		// check against our current target

			//if it is 
			//lureImage.sprite = travellerLureIcon;
		


		//go through all monster tagged objects
		//for each grab script and get the current lamp
		/* 
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Monster")) {
			EnemyMovement eMovement = g.GetComponent<EnemyMovement>();
			if (eMovement == null)
				Debug.Log("Could not find monster movement script");
			
			GameObject lamp = eMovement.findCurrentLamp();
			if (lamp != null) {
				lightSourceController lController = lamp.GetComponent<lightSourceController>();
				if (lController == null)
					Debug.Log("Could not find light source controller");
				//if (lController.getAdjacentSources().)
				if (checkAdjacent(lController.getAdjacentSources())) {
					//check if monster is in monster radius
					//lureImage.enabled = true;
					//lureImage.sprite = monsterLureIcon;

				}
			}
			
		}
		*/
	}



	  void unlockAbilties() {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        //if ()
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.CompareTo("Level1") == 0) {
            healUnlocked = false;
			stunUnlocked = false;

        }
        else if (SceneManager.GetActiveScene().name.CompareTo("Level2") == 0) {
             healUnlocked = true;
			 stunUnlocked = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level3") == 0) {
            healUnlocked = true;
			 stunUnlocked = true;
			
        }
        else if (SceneManager.GetActiveScene().name.CompareTo("Level4") == 0) {
            healUnlocked = true;
			 stunUnlocked = true;
        }
    }


	


}
