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
    public Text popUpTextCount;

    public GameObject interactionPopUp2;
	private WorldSpaceObjectController popUpController2;
	public Text popUpText2;

	public GameObject interactionPopUp3;
	private WorldSpaceObjectController popUpController3;
	public Text popUpText3;



    public float textVerticalOffset;

	public GameObject monsterPopup;
    public GameObject monsterTimer;

    public GameObject lightCountdown;

    public Canvas worldCanvas;

	public List<GameObject> monstersInRange;

	public GameObject lightPath;

	//the world space ui slider (not the canvas one)
	public GameObject travHealingBar;
	private WorldSpaceObjectController travHealingBarController;

	private AudioSource healingSFX;

	public Animator anim;



	private bool healUnlocked;
	private bool canHeal;
	private bool stunUnlocked;


	private bool inTutorial;


	private bool isHealing;

	
    private float timeToHeal;
    private float currentHealTime;

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



        travHealingBarController = travHealingBar.GetComponent<WorldSpaceObjectController>();
		if (travHealingBarController == null)
			Debug.Log("Could not find travhealingbarcontroller"); 


		
		unlockAbilties();

		createMonsterPopups();

        createLightCountDowns();

        //travSlider = Instantiate(travHealthPrefab, transform.position, Quaternion.identity);
        //travSlider.SetActive(false);
        healingSFX = GetComponent<AudioSource>();
		if (healingSFX == null)
			Debug.Log("Could not find the healing sound effect");
		isHealing = false;

		timeToHeal = .2f;
        currentHealTime = 0f;


		
	}
	
	// Update is called once per frame
	void Update () {

		if (inTutorial)
			return;
	
		if (Input.GetButtonDown("Circle") || Input.GetKeyDown(KeyCode.Space)) {
			isHealing = true;
			if (targetTraveller != null && targetTraveller.tag == "Traveller" ) {
				travellerHealth tScript = targetTraveller.GetComponent<travellerHealth>();
				if (tScript.currentHealth != tScript.startingHealth)  {
					tScript.startHealEffect();
					healingSFX.Play();
				}
			}


		}
		if (Input.GetButton("Circle") || Input.GetKey(KeyCode.Space)) {
			heldDuration += Time.deltaTime;
			if (heldDuration > 0.5f) { //&& !setHealing) {
				//start healing
				//setHealing = true;
			
				if (targetTraveller != null && targetTraveller.tag == "Traveller" ) {
					travellerHealth tScript = targetTraveller.GetComponent<travellerHealth>();
					currentHealTime += Time.deltaTime;
					if (canHeal){
						
						if (pController.getResource() > 0 &&  currentHealTime >= timeToHeal) {
							anim.SetBool("isHealing", true);
							//tScript.increaseCape();
							tScript.GetHeal(1);

							pController.addResource(-1f);

							currentHealTime = 0f;
						}

						return;
					}

					//stop healing if we cannot heal
					else {
						if (tScript.isHealingEffectOn()) {
							tScript.stopHealingEffect();
							healingSFX.Stop();
						}
						//Debug.Log("stopping effect");
						//Debug.Log(tScript.currentHealth);
						//Debug.Log(tScript.startingHealth);

					}
				
				}
				

			
					
			}

		}

		if (Input.GetButtonUp("Circle")|| Input.GetKeyUp(KeyCode.Space)) {
			heldDuration = 0f;
			isHealing = false;
			if (targetTraveller != null){
				travellerHealth tScript = targetTraveller.GetComponent<travellerHealth>();
				if (tScript.isHealingEffectOn()) { 
					tScript.stopHealingEffect();
					healingSFX.Stop();
					
				}
			}
			if (travHealingBar.activeInHierarchy)
				travHealingBar.SetActive(false);
			// moved outside of off claue above (if you move away from trav it'll loop infintely)
			anim.SetBool("isHealing", false);
		}

		if (Input.GetMouseButtonUp(1) ||  Input.GetButtonUp("Square") ) {
			
			if (pController.getResource() >= 20 && monstersInRange.Count >=1) {
				setStun();
				return;
			}				
	
		}


		if (Input.GetButtonDown("X") || Input.GetMouseButtonDown(0)) {
				//call stun enemy function
				if (currentTarget != null && currentTarget.tag == "LampLight") {
					if (lScript.getCurrentLightType() == 0 && pController.getResource() >= pController.getCurrentResourceNeeded()){
					
						pController.setTargetLight(currentTarget);
						//set color
						lScript.setMiniMapPathColor(pController.equippedLight);
						lScript.turnOnPaths();
						
						
						//Debug.Log("truning on light");
						return;
					}
					else if (lScript.getCurrentLightType() > 0 ) {
						//Debug.Log("turning off light");
						pController.setTargetLight(currentTarget);
						lScript.setMiniMapPathColor(0);
						lScript.turnOffPaths();
					}
				}
		}
	

		if (currentTarget && (lScript != null)) {
			//interactionText.text = "Light";
			//popUpText.fontSize = 100;
			
			if(lScript.getCurrentLightType() !=0) {
				//popUpText.fontSize = 110;
				popUpText.text =   "Recover";
                popUpTextCount.text = "(+" + (int) lScript.harvestAmount() + ")";
				// " + (int) lScript.harvestAmount()
			}
			else if (pController.getResource() < pController.getCurrentResourceNeeded()) {
				//popUpText.fontSize = 70;
				popUpText.text =   "Not Enough!";
                if (pController.equippedLight == 1)
                    popUpTextCount.text = "(-10)";
                else if (pController.equippedLight == 2)
                    popUpTextCount.text = "(-15)";
                else if (pController.equippedLight == 3)
                    popUpTextCount.text = "(-15)";
            }
			else {
				//popUpText.fontSize = 120;
				popUpText.text =   "Ignite" ;
             
                if (pController.equippedLight == 1)
                    popUpTextCount.text = "(-10)";
                else if (pController.equippedLight == 2)
                    popUpTextCount.text = "(-15)";
                else if (pController.equippedLight == 3)
                    popUpTextCount.text = "(-15)";
                  
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

	/*
	void OnTriggerEnter(Collider other) {
        Debug.Log("")
        
		if (other.tag == "LampLight") {
            currentTarget = other.gameObject;
            //interactionText.text = "Press X to interact with Light Source";
            interactionPopUp.SetActive(true);
            Vector3 popUpLocation = other.gameObject.transform.position;
            popUpLocation.y = popUpLocation.y + textVerticalOffset;
            popUpController.updateWorldObjectTransform(popUpLocation);
            //controlLureImage();

            lScript = other.GetComponent<lightSourceController>();
			if (lScript == null) 
				Debug.Log("Could not get lscript");

			lScript.turnOnWorldPaths();
			lScript.turnOnPaths();
		}
      
	}
    */
	void OnTriggerStay(Collider other) {


		if (other.tag == "Traveller" && healUnlocked) {
			targetTraveller = other.gameObject;
			travellerHealth tHealth = targetTraveller.GetComponent<travellerHealth>();
			if (tHealth == null)
				Debug.Log("Could not get traveller health script");


			if (tHealth.currentHealth != tHealth.startingHealth) {
				canHeal = true;
			
				//canHeal = true;
				//interactionText.text = "Hold X to transfer light to Traveller";
				
			//	travHealingBar.SetActive(true);
				Vector3 popUpLocation = other.gameObject.transform.position;
				popUpLocation.y = popUpLocation.y +textVerticalOffset;
				popUpController3.updateWorldObjectTransform(popUpLocation);
				travHealingBarController.updateWorldObjectTransform(popUpLocation);
				
				if (isHealing){
					//popUpText3.fontSize = 80;
					//popUpText3.text = "Healing";
					interactionPopUp3.SetActive(false);
					travHealingBar.SetActive(true);
					//travhealing
				}
				else {
					interactionPopUp3.SetActive(true);
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
				
			}
			else { 
				interactionPopUp3.SetActive(false);
				travHealingBar.SetActive(false);
				canHeal = false;

			}
			return;
		}
        
		if (other.tag == "LampLight") {
            //Debug.Log("at lamp light");
            if (currentTarget != other.gameObject) //new lamp entered radius but old hasnt left
            {

                if (lScript != null)
                {
                    if (lScript.getCurrentLightType() == 0)
                    {
                        //lScript.setMiniMapPathColor(0);

                        lScript.turnOffPaths();
                        //Debug.Log("turning off)");
                    }
                    lScript.turnOffWorldPaths();
                    lScript.switcherScript.setDefault();
                }

            }
			currentTarget = other.gameObject;
			
            
			interactionPopUp.SetActive(true);
            /*
			Vector3 popUpLocation = other.gameObject.transform.position;
			popUpLocation.y = popUpLocation.y + textVerticalOffset;
			popUpController.updateWorldObjectTransform(popUpLocation);
           */


           

            lScript = other.GetComponent<lightSourceController>();
            if (lScript == null)
                Debug.Log("Could not get lscript");

           

            lScript.turnOnWorldPaths();
            lScript.turnOnPaths();
            lScript.switcherScript.sethighlight();
            return;
		}

        if (other.tag == "Monster")
        {
            EnemyMovement monScript = other.gameObject.GetComponent<EnemyMovement>();
            if (monScript == null)
            {
                Debug.Log("Could not find monscript");
            }

            //monScript.popUp2.SetActive(true);

            Vector3 popUpLocation = other.gameObject.transform.position;
            popUpLocation.y = popUpLocation.y + textVerticalOffset;
            monScript.popUp2.GetComponent<WorldSpaceObjectController>().updateWorldObjectTransform(popUpLocation);
        }
        
        /*
		if (other.tag == "Monster" && stunUnlocked) {
			
			
			//currentTarget = other.gameObject;
			//targetMonster = other.gameObject;
			//interactionText.text = "Press X to stun Monster";
			//interactionPopUp2.SetActive(true);
			//Vector3 popUpLocation = other.gameObject.transform.position;
			//popUpLocation.y = popUpLocation.y +textVerticalOffset;
			////popUpController2.updateWorldObjectTransform(popUpLocation);
			//controlLureImage();
			//return;
			

			

			EnemyMovement monScript = other.gameObject.GetComponent<EnemyMovement>();
			if (monScript == null) {
				Debug.Log("Could not find monscript");
			}
			//else
			if (!monScript.getIsStunned()) {
				
				if (!monstersInRange.Contains(other.gameObject)){
					monstersInRange.Add(other.gameObject);
				}
				monScript.popUp.SetActive(true);
               // monScript.popUp2.SetActive(false);
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
			else {
				monScript.popUp.SetActive(false);
                monScript.popUp2.SetActive(true);

                Vector3 popUpLocation = other.gameObject.transform.position;
                popUpLocation.y = popUpLocation.y + textVerticalOffset;
                monScript.popUp2.GetComponent<WorldSpaceObjectController>().updateWorldObjectTransform(popUpLocation);

                monstersInRange.Remove(other.gameObject);
			}
			
	
	
	
		}

        */
	

	}



	void OnTriggerExit(Collider other) {

		if (other.tag == "Traveller" && other.gameObject == targetTraveller) {
            //currentTarget = null;
            travellerHealth tScript = other.gameObject.GetComponent<travellerHealth>();
            tScript.stopHealingEffect();
            interactionPopUp3.SetActive(false);
			travHealingBar.SetActive(false);
			targetTraveller = null;
            healingSFX.Stop();
           
            //interactionText.text  = "";
            return;
		}
		if (other.tag == "LampLight" && other.gameObject == currentTarget) {


            if (lScript != null){
				if ( lScript.getCurrentLightType() == 0) {
					//lScript.setMiniMapPathColor(0);
				
					lScript.turnOffPaths();
					//Debug.Log("turning off)");
				}
				lScript.turnOffWorldPaths();
                lScript.switcherScript.setDefault();
            }
			
			interactionPopUp.SetActive(false);

            currentTarget = null;
			lScript = null;
			
			return;
		}

   
		
	}

	void setStun() {
		//Debug.Log("Setting stun");
		
		foreach (GameObject m in monstersInRange) {
			//for each monster in array

			stunEffectController sScript = m.GetComponentInChildren<stunEffectController>();
			if (sScript == null) {
				Debug.Log("Could not find the stun effect");

			}
			sScript.playStunEffect();

			EnemyMovement monScript = m.GetComponent<EnemyMovement>();
			if (monScript == null)
				Debug.Log("Could not find monScript!");
			monScript.setStunned();
            monScript.popUp2.SetActive(true);


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
		else if (SceneManager.GetActiveScene().name.CompareTo("Level2.5") == 0 || SceneManager.GetActiveScene().name.CompareTo("Level2.5.5EDIT") == 0) {
             healUnlocked = true;
			 stunUnlocked = false;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level3") == 0 ||  SceneManager.GetActiveScene().name.CompareTo("Level3EDIT") == 0) {
            healUnlocked = true;
			stunUnlocked = false;
			
        }
		else if (SceneManager.GetActiveScene().name.CompareTo("Level3.5") == 0 || SceneManager.GetActiveScene().name.CompareTo("Level3.5EDIT") == 0) {
            healUnlocked = true;
			stunUnlocked = true;
			
        }
        else if (SceneManager.GetActiveScene().name.CompareTo("Level4") == 0 ||  SceneManager.GetActiveScene().name.CompareTo("Level4EDIT") == 0) {
			//Debug.Log("Unlock for level4");
            healUnlocked = true;
			stunUnlocked = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level5") == 0)
        {
            //Debug.Log("Unlock for level4");
            healUnlocked = true;
            stunUnlocked = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level5.5") == 0)
        {
            //Debug.Log("Unlock for level4");
            healUnlocked = true;
            //stunUnlocked = true;
        }
        else if (SceneManager.GetActiveScene().name.CompareTo("Level7") == 0) {
			//Debug.Log("Unlock for level4");
           
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
                //mPopup.transform.parent = worldCanvas.transform;
                mPopup.transform.SetParent(worldCanvas.transform);
                mPopup.SetActive(false);
				
				monScript.popUp = mPopup;

                //the countdown timer
                GameObject mPopup2 = Instantiate(monsterTimer, transform.position, Quaternion.identity);
               // mPopup2.transform.parent = worldCanvas.transform;
                mPopup2.transform.SetParent(worldCanvas.transform);
                mPopup2.SetActive(false);

                monScript.popUp2 = mPopup2;
            }


		}
	}

    public void createLightCountDowns()
    {
        foreach (GameObject m in GameObject.FindGameObjectsWithTag("LampLight"))
        {
            lightSourceController lController = m.GetComponent<lightSourceController>();
            if (lController == null)
            {
                Debug.Log("Could not find lController");
            }
            else
            {
                GameObject countDownPopUp = Instantiate(lightCountdown, transform.position, Quaternion.identity);
               // countDownPopUp.transform.parent = worldCanvas.transform;
                countDownPopUp.transform.SetParent(worldCanvas.transform);
                countDownPopUp.SetActive(false);

                Vector3 popUpLocation = m.transform.position;
                popUpLocation.y = popUpLocation.y + textVerticalOffset;
              

                lController.countDown = countDownPopUp;
                lController.countDown.GetComponent<WorldSpaceObjectController>().updateWorldObjectTransform(popUpLocation);
            }
        }
    }

    public bool getStunnedUnlocked()
    {
        return stunUnlocked;
    }

	public List<GameObject> getMonsters()
    {
        return monstersInRange;
    }

	


}
