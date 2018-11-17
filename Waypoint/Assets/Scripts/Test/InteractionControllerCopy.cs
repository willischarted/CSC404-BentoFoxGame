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
	[SerializeField]
	private lightSourceController lScript;

	public GameObject targetMonster;

	public GameObject targetTraveller;

	public float stunCost;

	public GameObject interactionPopUp;
	private WorldSpaceObjectController popUpController;
	public Text popUpText;

	public GameObject interactionPopUp2;
	private WorldSpaceObjectController popUpController2;
	public Text popUpText2;

	public GameObject interactionPopUp3;
	private WorldSpaceObjectController popUpController3;
	public Text popUpText3;

	public float textVerticalOffset;

	public GameObject monsterPopup;
	public Canvas worldCanvas;

	public List<GameObject> monstersInRange;

	public GameObject lightPath;




	private bool healUnlocked;
	private bool canHeal;
	private bool stunUnlocked;


	private bool inTutorial;




	// Use this for initialization
	void Start () {
		//impulseCooldown = 5.0f;
		//setImpulse = false;
		canHeal = false;
		setHealing = false;
		pController = GetComponentInParent<playerControllerCopy>();
		if (pController == null) {
			Debug.Log("Could not find pController");
		}

		popUpController = interactionPopUp.GetComponent<WorldSpaceObjectController>();
		if (popUpController == null)
			Debug.Log("Could not find worldspacecontroller");

		popUpController2 = interactionPopUp2.GetComponent<WorldSpaceObjectController>();
		if (popUpController2 == null)
			Debug.Log("Could not find worldspacecontroller");
		
		popUpController3 = interactionPopUp3.GetComponent<WorldSpaceObjectController>();
		if (popUpController3 == null)
			Debug.Log("Could not find worldspacecontroller");


		
		unlockAbilties();

		createMonsterPopups();

		
	}
	
	// Update is called once per frame
	void Update () {

		if (inTutorial)
			return;
	

		if (Input.GetButton("Circle") || Input.GetKey(KeyCode.Space)) {
			heldDuration += Time.deltaTime;
			if (heldDuration > 0.5f) { //&& !setHealing) {
				//start healing
				//setHealing = true;
				if (targetTraveller != null && targetTraveller.tag == "Traveller" && canHeal) {
					travellerHealth tScript = targetTraveller.GetComponent<travellerHealth>();
					if (pController.getResource() > 0) {
						//tScript.increaseCape();
						tScript.GetHeal(1);
						pController.addResource(-1f);
					}
					return;
				}

			
					
			}

		}

		if (Input.GetButtonUp("Circle")|| Input.GetKeyUp(KeyCode.Space)) {
			heldDuration = 0f;
		}

		if (Input.GetMouseButtonDown(1) ||  Input.GetButtonDown("Square") ) {
			
			if (pController.getResource() >= 20 && monstersInRange.Count >=1) {
				setStun();
				return;
			}				
	
		}


		if (Input.GetButtonDown("X") || Input.GetMouseButtonDown(0)) {
				//call stun enemy function
				if (currentTarget != null && currentTarget.tag == "LampLight" && pController.getResource() > pController.getCurrentResourceNeeded()) {
					pController.setTargetLight(currentTarget);
					lightSourceController lController = currentTarget.GetComponent<lightSourceController>();
					if (lController == null) {
						Debug.Log("Could not find lightsource controller");
					
					}
					lController.turnOnPaths();
					Debug.Log("truning on light");
					return;
				}
		}
	

		if (currentTarget && (lScript != null)) {
			//interactionText.text = "Light";
			popUpText.fontSize = 100;
			
			if(lScript.getCurrentLightType() !=0) {
				popUpText.fontSize = 110;
				popUpText.text =   "Harvest";
				// " + (int) lScript.harvestAmount()
			}
			else if (pController.getResource() < pController.getCurrentResourceNeeded()) {
				popUpText.fontSize = 70;
				popUpText.text =   "Not Enough!";
			}
			else {
				popUpText.fontSize = 120;
				popUpText.text =   "Ignite" ;
				//(int) pController.getCurrentResourceNeeded() + ")"
				//Debug.Log("Setting to ignite");
			}
			//popUpText.text = "hello";
			//Debug.Log("Could not set text");
			return;
		}
		
		
		else {
			interactionText.text = "";
			return;
		}
		
	}

	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "LampLight") {
			lScript = other.GetComponent<lightSourceController>();
			if (lScript == null) 
				Debug.Log("Could not get lscript");
		}
	}
	void OnTriggerStay(Collider other) {


		if (other.tag == "Traveller" && healUnlocked) {
			targetTraveller = other.gameObject;
			travellerHealth tHealth = targetTraveller.GetComponent<travellerHealth>();
			if (tHealth == null)
				Debug.Log("Could not get traveller health script");


			if (tHealth.currentHealth != tHealth.startingHealth) {
				canHeal = true;
				//interactionText.text = "Hold X to transfer light to Traveller";
				interactionPopUp3.SetActive(true);
				Vector3 popUpLocation = other.gameObject.transform.position;
				popUpLocation.y = popUpLocation.y +textVerticalOffset;
				popUpController3.updateWorldObjectTransform(popUpLocation);

				if (pController.getResource() > 0) {
					popUpText3.fontSize = 80;
					popUpText3.text = "Hold to Heal";
				}
				else {
					popUpText3.fontSize = 80;
					popUpText3.text = "Not Enough!";
				} 
				//controlLureImage();
				//return;
			}
			else { 
				interactionPopUp3.SetActive(false);
				canHeal = false;

			}
			return;
		}

		if (other.tag == "LampLight") {
			currentTarget = other.gameObject;
			//interactionText.text = "Press X to interact with Light Source";
			interactionPopUp.SetActive(true);
			Vector3 popUpLocation = other.gameObject.transform.position;
			popUpLocation.y = popUpLocation.y + textVerticalOffset;
			popUpController.updateWorldObjectTransform(popUpLocation);
			//controlLureImage();
			return;
		}

		if (other.tag == "Monster" && stunUnlocked) {
			
			/* 
			//currentTarget = other.gameObject;
			targetMonster = other.gameObject;
			//interactionText.text = "Press X to stun Monster";
			interactionPopUp2.SetActive(true);
			Vector3 popUpLocation = other.gameObject.transform.position;
			popUpLocation.y = popUpLocation.y +textVerticalOffset;
			popUpController2.updateWorldObjectTransform(popUpLocation);
			controlLureImage();
			return;
			*/

			if (!monstersInRange.Contains(other.gameObject)){
				monstersInRange.Add(other.gameObject);
			}

			EnemyMovement monScript = other.gameObject.GetComponent<EnemyMovement>();
			if (monScript == null) {
				Debug.Log("Could not find monscript");
			}

			else {
				monScript.popUp.SetActive(true);
				Vector3 popUpLocation = other.gameObject.transform.position;
				popUpLocation.y = popUpLocation.y +textVerticalOffset;
				monScript.popUp.GetComponent<WorldSpaceObjectController>().updateWorldObjectTransform(popUpLocation);
				Text monText = monScript.popUp.GetComponentInChildren<Text>();
				if (pController.getResource() >= 20){ // change to public var later
					
					monText.fontSize = 100;
					monText.text =   "Stun";
				}

				else {
					monText.fontSize = 80;
					monText.text =   "Not Enough!";
				}

				return;

			}
			
	
	
	
		}


	

	}

	void OnTriggerExit(Collider other) {

		if (other.tag == "Traveller" && other.gameObject == targetTraveller) {
			//currentTarget = null;
			interactionPopUp3.SetActive(false);
			targetTraveller = null;
			//interactionText.text  = "";
			return;
		}
		if (other.tag == "LampLight" && other.gameObject == currentTarget) {
			interactionPopUp.SetActive(false);
			currentTarget = null;
			lScript = null;
			//interactionText.text  = "";
			return;
		}
		if (other.tag == "Monster" && monstersInRange.Count >=1) { //maybe use isunlocked?? //&& other.gameObject == targetMonster
			//interactionPopUp2.SetActive(false);
			//currentTarget = null;
			//targetMonster = null;
			//interactionText.text  = "";
			EnemyMovement monScript = other.gameObject.GetComponent<EnemyMovement>();
			monScript.popUp.SetActive(false);
			monstersInRange.Remove(other.gameObject);
			return;
		}

		
	}

	void setStun() {
		//Debug.Log("Setting stun");
		
		foreach (GameObject m in monstersInRange) {
			//for each monster in array

			/* 
			Animator anim = m.GetComponent<Animator>();
			if (anim == null) {
				Debug.Log("Could not find anim");
			}
			anim.SetTrigger("isStunned");
			*/
			EnemyMovement monScript = m.GetComponent<EnemyMovement>();
			if (monScript == null)
				Debug.Log("Could not find monScript!");
			monScript.setStunned();
			
		
			// get animator contoller and set stun
		}
		pController.addResource(-stunCost);
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
        //Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.CompareTo("Level1") == 0) {
            healUnlocked = false;
			stunUnlocked = false;

        }
        else if (SceneManager.GetActiveScene().name.CompareTo("Level2") == 0) {
             healUnlocked = true;
			 stunUnlocked = false;
        }
		else if (SceneManager.GetActiveScene().name.CompareTo("Level2.5") == 0) {
             healUnlocked = true;
			 stunUnlocked = false;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level3") == 0) {
            healUnlocked = true;
			stunUnlocked = false;
			
        }
		else if (SceneManager.GetActiveScene().name.CompareTo("Level3.5") == 0) {
            healUnlocked = true;
			stunUnlocked = true;
			
        }
        else if (SceneManager.GetActiveScene().name.CompareTo("Level4") == 0) {
			//Debug.Log("Unlock for level4");
            healUnlocked = true;
			stunUnlocked = true;
        }
    }

	public void setInTutorial(bool _inTutorial) {
		inTutorial = _inTutorial;
	}

	public void createMonsterPopups() {
		foreach(GameObject m in GameObject.FindGameObjectsWithTag("Monster")) {
			EnemyMovement monScript = m.GetComponent<EnemyMovement>();
			if (monScript == null) {
				Debug.Log("Could not find monScript");
			}
			else {
				GameObject mPopup = Instantiate(monsterPopup, transform.position, Quaternion.identity);
				mPopup.transform.parent = worldCanvas.transform;
				mPopup.SetActive(false);
				
				monScript.popUp = mPopup;
			}


		}
	}

	

	


}
