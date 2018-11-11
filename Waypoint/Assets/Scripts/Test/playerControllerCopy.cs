using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class playerControllerCopy: MonoBehaviour {
    //enum lightType {Default, Traveller, Monster};

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
 	AudioSource audioSource;
    
    // is the firefly interacting, and restricts movement
    private bool lightReady;
    private bool restrictMovement;
    
    // Used to determine what light/ability firefly has equipped
    private int equippedLight;
    public float lightResource;

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
    private bool inTutorial;

    public float rotateSpeed = 10f;
    

    void Awake(){
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
       
	}
	
	// Update is called once per frame
    void Update() {
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
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Triangle")) && toggleUnlocked){
            //equippedLight = 2;
            //setFireFlyMaterial();
           // abilityBackground.color = Color.yellow;



            if (equippedLight == 3) {
                equippedLight = 1;
            }
            else{
                if (monLightOnly)
                    equippedLight+=2;
                
                else
                    equippedLight++;

            }
            
            
           // setFireFlyMaterial();
           // updateAbilityUI(); 

            abilityUIScript.updateAbilityIcon(equippedLight);
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

            if (moveDirection == Vector3.zero)
                return;
            //transform.rotation = Quaternion.LookRotation(moveDirection); //old
            var rotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
            rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);


        }
    }


    public void setTargetLight(GameObject lightSource) {
        //Debug.Log("called set light");
        if (lightSource.CompareTag("LampLight"))
        {
           // Debug.Log(lightSource.transform.position);
            count = count + 1;
            SetCountText();
           // cCollider = lightSource.GetComponentInParent<CapsuleCollider>();
            lampLight = lightSource.gameObject.GetComponentInChildren<Light>();
            Material bulb = lightSource.GetComponentInChildren<Renderer>().material;
            //Behaviour halo =(Behaviour)other.GetComponent ("Halo");
            
            //.Log(bulb.name);
            if (bulb == null)
                Debug.Log("HHH");
            if (lampLight.intensity > 0)
            {
                lampLight.intensity = 0;
             //   cCollider.enabled = false;
                audioSource.clip = offSoundEffect;
                audioSource.Play();

                if (equippedLight == 1 || equippedLight == 2){
                    tMovement.findLatest(lightSource);
                }
                    
                bulb.DisableKeyword("_EMISSION");
                //addResource(tempLightCost);

                //halo.enabled = false;
                lightSourceController lController = lightSource.GetComponentInParent<lightSourceController>();
                if (lController != null)
                    lController.setCurrentLightType(0);
                
            }
            else{
                //setLightColor(lampLight, equippedLight);
                if (getResource() >= getCurrentResourceNeeded()){
 
                setChildLight(lightSource.GetComponentsInChildren<Light>());
                lampLight.intensity = lightValueOn;
               // Debug.Log("the intensity is  " + lampLight.intensity);
               // cCollider.enabled = true;
                audioSource.clip = onSoundEffect;
                audioSource.Play();
                if (equippedLight == 1 || equippedLight == 2)
                {
                    tMovement.findLatest(lightSource);
                }

                //
                if (equippedLight == 1 || equippedLight == 3) {
                    //Debug.Log("Sending signal to all monsters");
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

                bulb.EnableKeyword("_EMISSION");
                setMaterialColor(bulb, equippedLight);

                // halo.enabled = true;

                //addResource(-tempLightCost);
                subtractResource();
                lightSourceController lController = lightSource.GetComponentInParent<lightSourceController>();
                if (lController != null)
                    lController.setCurrentLightType(equippedLight);

                }
                //halo.enabled = true;
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
        Color c;
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
        resourceCount.text = resourceValue.ToString();
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
        resourceCount.text = resourceValue.ToString();
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

        else if (SceneManager.GetActiveScene().name.CompareTo("Level2.5") == 0) {
            toggleUnlocked = true;
            monLightOnly = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level3") == 0) {
            toggleUnlocked = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level3.5") == 0) {
            toggleUnlocked = true;
        }

        else if (SceneManager.GetActiveScene().name.CompareTo("Level4") == 0) {
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

}
