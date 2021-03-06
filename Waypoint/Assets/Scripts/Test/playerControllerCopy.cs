﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class playerControllerCopy: MonoBehaviour {
    //enum lightType {Default, Traveller, Monster};

    public GameObject TutorialText;

    public Color color1;
    public Color color2;
    public Color color3;

    public float speed;
    public Text countText;
    private Rigidbody rb;
    private int count;
    private Light lampLight;
    
    private CapsuleCollider cCollider;
    public GameObject traveller;
    private travellerMovement tMovement;

    // Audio effects
    public AudioClip onSoundEffect;
    public AudioClip offSoundEffect;

    public AudioClip changeLightSoundEffect;
 	AudioSource audioSource;
    
    // is the firefly interacting, and restricts movement
    private bool lightReady;
    private bool restrictMovement;
    
    // Used to determine what light/ability firefly has equipped
    public int equippedLight;
    public float lightResource;
    public float startingResource;

    // Link to firefly Resource UI
    public Slider resourceBar;
    public Text resourceCount;

    private float tempLightCost;

    public GameObject pauseUI;
    private PauseMenu pauseScript;

    public GameObject gameOver;
    private GameOver gameOverScript;


    public GameObject lightAbility;
    private abilityIconController abilityUIScript;

   // public Image abilityBackground;
   // public Image abilityIcon;
   // public Sprite icon1;
   // public Sprite icon2;
   // public Sprite icon3;

    public int light1Value;
    public int light2Value;
    public int light3Value;

    public float lightValueOn;

    private bool toggleUnlocked;

    //used to restrict light for lv 2.5
    private bool monLightOnly;
    [SerializeField]  private bool inTutorial;

    public float rotateSpeed = 10f;

    private InteractionControllerCopy iScript;


    void Awake(){

        iScript = GetComponentInChildren<InteractionControllerCopy>();

        equippedLight = 1;
        restrictMovement = false;
        tMovement = traveller.GetComponent<travellerMovement>();
        if (tMovement == null) {
            Debug.Log("Could not find tscript");
        }

        abilityUIScript = lightAbility.GetComponent<abilityIconController>();
        if (abilityUIScript == null)
            Debug.Log("Could not find abilityUIscript");

        
    }
	// Use this for initialization
    void Start () {
        lightReady = false;
        tempLightCost = light1Value;
        //if (lightResource == 0)
        //    lightResource = 100;
        rb = GetComponent<Rigidbody>();
        pauseScript = pauseUI.GetComponent<PauseMenu>();
        gameOverScript = gameOver.GetComponent<GameOver>();
        count = 0;
        SetCountText();
        audioSource = GetComponent<AudioSource>();

        unlockAbilties();
        startingResource = lightResource;
       
	}
	
	// Update is called once per frame
    void Update() {

        /*
        if (TutorialText != null)
        {
            TutorialText.transform.position = this.gameObject.transform.position;
        }
        */
        if (inTutorial)
            return;

      
         // Toggle between 4 types of light magic
       // if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("X")){
            //equippedLight = 0;
            //setFireFlyMaterial();

        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown("Square")){
            //equippedLight = 1;
            //setFireFlyMaterial();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
       // }
       if ((Input.GetKeyDown(KeyCode.T)))
        {
            resetPlayerPrefs();

        }
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Triangle")) && toggleUnlocked && iScript.currentTarget !=null){
            //equippedLight = 2;
            //setFireFlyMaterial();
           // abilityBackground.color = Color.yellow;


            audioSource.clip = changeLightSoundEffect;
            audioSource.Play();

            if (equippedLight == 1)
            {
                equippedLight = 3;
            }
            else if(equippedLight == 2)
            {
                equippedLight = 1;
            }
            else if (equippedLight == 3) //monster light
            {
                if (monLightOnly)
                    equippedLight = 1;
                else
                    equippedLight = 2;
            }
            /*
            if (equippedLight == 3) {
                equippedLight = 1;
            }
            else{
                if (monLightOnly)
                    equippedLight+=2;
                
                else
                    equippedLight++;

            }

            */
            // setFireFlyMaterial();
            // updateAbilityUI(); 

            if (equippedLight == 3) //old monster light 
                abilityUIScript.updateAbilityIcon(equippedLight - 1);
            else if (equippedLight == 2) //old trav

                abilityUIScript.updateAbilityIcon(equippedLight + 1);
            else
                abilityUIScript.updateAbilityIcon(equippedLight);
                
           // abilityUIScript.updateAbilityIcon(equippedLight);
            // Invoke ("setBackgroundWhite", .5f);
        }

        //   if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Triangle")) {
        //   
        // }

        /* 
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Circle")){
            //equippedLight = 3;
            //setFireFlyMaterial();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        */
        // Restart button
        //  if (Input.GetButtonDown("L1") || Input.GetKeyDown(KeyCode.R)) {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        // }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonUp("Start"))
        {
            pauseScript.pause();
        }

        if((!tMovement.beatLevel) && (lightResource < 10) && !(anyLightsOn()))
        {
            gameOverScript.gameOverr();
        }

        // Only interact while r2 is pulled, set tag otherwise release.
        /* 
        if (Input.GetButtonDown("R2") || Input.GetMouseButtonDown(0)) {
            lightReady = true;
        }
        if (Input.GetButtonUp("R2") || Input.GetMouseButtonUp(0)) {
            lightReady = false;
        }
        */
    }

    void FixedUpdate()
    {
        if (inTutorial)
            return;
        if (!restrictMovement){
           
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            var camera = Camera.main;
            Vector3 relativeForward = camera.transform.forward;
            Vector3 relativeRight = camera.transform.right;
        
            relativeForward.y = 0f;
            relativeRight.y = 0f;
            relativeForward.Normalize();
            relativeRight.Normalize();

            

            Vector3 moveDirection = relativeForward * moveVertical + relativeRight * moveHorizontal;

            if (moveDirection == Vector3.zero) {
                rb.velocity = Vector3.zero;
                return;

            }
            //transform.rotation = Quaternion.LookRotation(moveDirection); //old
            var rotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
            
            
            rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
            //rb.AddForce (moveDirection * speed);
           // rb.velocity = new Vector3(relativeForward * moveVertical, 0.0f, relativeRight * moveHorizontal);


        }
    }


    public void setTargetLight(GameObject lightSource) {
        //Debug.Log("called set light");
        if (lightSource.CompareTag("LampLight"))
        {

            count = count + 1;
            SetCountText();

            lampLight = lightSource.gameObject.GetComponentInChildren<Light>();
           // Material bulb = lightSource.GetComponentInChildren<Renderer>().material;

           // if (bulb == null)
            //    Debug.Log("HHH");
            if (lampLight.intensity > 0)
            {
                lampLight.intensity = 0;

                audioSource.clip = offSoundEffect;
                audioSource.Play();

                if (equippedLight == 1 || equippedLight == 2){
                    tMovement.findLatest(lightSource);
                }
                    
                //bulb.DisableKeyword("_EMISSION");

                lightSourceController lController = lightSource.GetComponentInParent<lightSourceController>();
                if (lController != null)
                    lController.setCurrentLightType(0);
                
            }
            else{
               
               
 
                    setChildLight(lightSource.GetComponentsInChildren<Light>());
                    lampLight.intensity = lightValueOn;
              
                    audioSource.clip = onSoundEffect;
                    audioSource.Play();
                    if (equippedLight == 1 || equippedLight == 2)
                    {
                        tMovement.findLatest(lightSource);
                    }

                subtractResource();
                lightSourceController lController = lightSource.GetComponentInParent<lightSourceController>();
                if (lController != null)
                    lController.setCurrentLightType(equippedLight);

                if (equippedLight == 1 || equippedLight == 3) {

                    GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                    foreach (GameObject g in monsters) {
                        EnemyMovement eScript = g.GetComponent<EnemyMovement>();
                        if (eScript ==null)
                            Debug.Log("Could not find script for monster");
                        else {
                            eScript.monsterLampLit(lightSource);
                        }
                    }
                }

                

                

            }
        }
    }
    

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    //Placeholder effects
    //===========================================================================================================
      void setMaterialColor(Material material,int color) {
        if (color == 1) {
            //Color.TryParseHexString("#F00", out light.color);
            //material.color = Color.yellow;
           // material.SetColor("_EmissionColor", Color.yellow);
           // setTrailRenderer();

             //material.color = Color.blue;
            material.SetColor("_EmissionColor", Color.yellow);
            //setTrailRenderer();
        }
        else if (color == 2) {
          //material.color = Color.green;
            material.SetColor("_EmissionColor", Color.green);
            //setTrailRenderer();
        }
        else if (color == 3) {
           

            //material.color = Color.red;
            material.SetColor("_EmissionColor", Color.red);
            //setTrailRenderer();
        }
        /* 
        else if (color == 3) {
            //material.color = Color.green;
            material.SetColor("_EmissionColor", Color.green);
            setTrailRenderer();
        }
        */
    }
    void setLightColor(Light light, int color) {
        //Color c;
        if (color == 1) {
            //Color.TryParseHexString("#F00", out light.color);
            //light.color = Color.blue;//Color.yellow;

          //  ColorUtility.TryParseHtmlString(color1, out c);
            light.color = color1;

        }
        else if (color == 2) {
            //light.color = Color.red;
            //light.color = Color.green;
            //ColorUtility.TryParseHtmlString(color2, out c);
            light.color = color2;
        }
        else if (color == 3) {
            //light.color = Color.red;
           // ColorUtility.TryParseHtmlString(color3, out c);
            light.color = color3;
        }
      //  else if (color == 3) {
      //      light.color = Color.green;
       // }
    }

    void setFireFlyMaterial() {
       
    SkinnedMeshRenderer[] meshRenderers  = GetComponentsInChildren< SkinnedMeshRenderer>();
        foreach ( SkinnedMeshRenderer m in meshRenderers) {
            if (m.gameObject.name == "model:body_03_GEO") //was "Sphere
                setMaterialColor(m.material,equippedLight);
       }

    }
    void setGradient(ref GradientColorKey[] colorKey, ref GradientAlphaKey[] alphaKey, ref Gradient gr) {
        Color color1;
        Color color2;

        if (equippedLight == 1) {
        //Color.TryParseHexString("#F00", out light.color);
           //color1 = Color.yellow;
           //color2 = Color.blue;
            color1 = Color.yellow;
            color2 = Color.blue;

        }
        else if (equippedLight == 2) {
            color1 = Color.green;
            color2 = Color.blue;
        }
        else if (equippedLight == 3) {
          //  color1 = Color.magenta;
           // color2 = Color.white;
            color1 = Color.red;
            color2 = Color.blue;
        }
      //  else if (equippedLight == 3) {
       //     color1 = Color.green;
        //    color2 = Color.blue;
           
       // }
        else {
       //     color1 = Color.yellow;
        //    color2 = Color.blue;
          color1 = Color.magenta;
            color2 = Color.white;
        }
        colorKey[0].color = color1;
        colorKey[0].time = 0.0f;
        colorKey[1].color = color2;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

       
    }
    void setTrailRenderer() {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        if (tr == null) {
            Debug.Log("Could not find trailrender");
        }

        Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;
        gradient = new Gradient();
        colorKey = new GradientColorKey[2];
        alphaKey = new GradientAlphaKey[2];

        

        setGradient(ref colorKey, ref alphaKey, ref gradient);
        gradient.SetKeys(colorKey, alphaKey);
        tr.colorGradient= gradient;

    }



    void setChildLight(Light[] list) {
        foreach (Light l in list) {
            setLightColor(l,equippedLight);
        }
    }

    // END OF PLACEHOLDER EFFECTS
    //===========================================================================================================
    
    
    
    
    public void setRestrictMovement(bool _restrictMovement) {
        restrictMovement = _restrictMovement;
    }

    public void addResource(float value) {
        //Debug.Log(lightResource);
        //Debug.Log(lightResource += value);
        lightResource += value;
        //Debug.Log("Adding " + value);
        resourceBar.value = lightResource;
        int resourceValue = (int)lightResource;
        resourceCount.text = resourceValue.ToString() + "/" + startingResource;
    }

    public void subtractResource() {
        int value = 0;
        if (equippedLight ==1) {
            value = light1Value;
        }
        else if (equippedLight ==2) {
            value = light2Value;
        }
        else if (equippedLight == 3) {
            value = light3Value;
        }
        //Debug.Log(lightResource);
        //Debug.Log(lightResource += value);
        lightResource -= value;
        //Debug.Log("Adding " + value);
        resourceBar.value = lightResource;
        int resourceValue = (int)lightResource;
        resourceCount.text = resourceValue.ToString() + "/" + startingResource;
    }



    public float getResource() {
        return lightResource;
    }

    public void updateAbilityUI() {
        if (equippedLight == 1) {
            //abilityIcon.sprite = icon1;
            tempLightCost = light1Value;
        }
        if (equippedLight == 2) {
           // abilityIcon.sprite = icon2;
             tempLightCost = light2Value;
        }
        if (equippedLight == 3) {
            //abilityIcon.sprite = icon3;
            tempLightCost = light3Value;
        }

        
    }

    public void setBackgroundWhite() {
        //abilityBackground.color = Color.white;
    }





    void unlockAbilties() {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        //if ()
        //Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.CompareTo("Level1") == 0) {
            toggleUnlocked = false;

        }
        else if (SceneManager.GetActiveScene().name.CompareTo("Level2") == 0) {
            toggleUnlocked = false;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level2.5") == 0 || SceneManager.GetActiveScene().name.CompareTo("Level2.5EDIT") == 0) {
            toggleUnlocked = true;
            monLightOnly = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level3") == 0 || SceneManager.GetActiveScene().name.CompareTo("Level3EDIT") == 0) {
            toggleUnlocked = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level3.5") == 0 || SceneManager.GetActiveScene().name.CompareTo("Level3.5EDIT") == 0) {
            toggleUnlocked = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level4") == 0 ||   SceneManager.GetActiveScene().name.CompareTo("Level4EDIT") == 0) {
            toggleUnlocked = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level5") == 0)
        {
            toggleUnlocked = true;
            monLightOnly = true;
        }


        else if (SceneManager.GetActiveScene().name.CompareTo("Level5.5") == 0)
        {
            toggleUnlocked = true;
          
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level7") == 0)
        {
            toggleUnlocked = true;
        }
    }

    //Are there any lights on?
    private bool anyLightsOn()
    {
        GameObject [] lights = GameObject.FindGameObjectsWithTag("LampLight");
        foreach (GameObject light in lights)
        {
            lightSourceController lsc = light.GetComponent<lightSourceController>();
            if (lsc.getCurrentLightType() != 0)
            {
                return true;
            }
        }
        return false;
    }

    public void setInTutorial(bool _inTutorial){
        inTutorial = _inTutorial;
    }   

    public int getCurrentResourceNeeded() {
        if (equippedLight == 1) {
            return light1Value;
        }
        if (equippedLight == 2) {
             return light2Value;
        }
        if (equippedLight == 3) {
            return light3Value;
        }
        return 0;
    }

    void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Wall") {
            Debug.Log("Hitting the wall");
           // Physics.IgnoreCollision();
           
			rb.velocity = Vector3.zero;
            
		}
	}

    public void resetPlayerPrefs()
    {

        PlayerPrefs.SetInt("Level1", 0);
        PlayerPrefs.SetInt("Level2", 0);
        PlayerPrefs.SetInt("Level2.5", 0);
        PlayerPrefs.SetInt("Level2.5EDIT", 0);
        PlayerPrefs.SetInt("Level3", 0);
        PlayerPrefs.SetInt("Level3EDIT", 0);
        PlayerPrefs.SetInt("Level3.5", 0);
        PlayerPrefs.SetInt("Level3.5EDIT", 0);
        PlayerPrefs.SetInt("Level4", 0);
        PlayerPrefs.SetInt("Level4EDIT", 0);
        PlayerPrefs.SetInt("Level5", 0);
        PlayerPrefs.SetInt("Level5.5", 0);
        PlayerPrefs.SetInt("Level7", 0);
    }
}
