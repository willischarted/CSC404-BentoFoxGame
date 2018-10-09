using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour {
   
    public float speed;
    public Text countText;
    private Rigidbody rb;
    private int count;
    private Light lampLight;
    
    private CapsuleCollider cCollider;
    public GameObject traveller;
    private travellerScript tScript;

    // Audio effects
    public AudioClip onSoundEffect;
    public AudioClip offSoundEffect;
 	AudioSource audioSource;
    
    // is the firefly interacting, and restricts movement
    private bool lightReady;
    private bool restrictMovement;
    
    // Used to determine what light/ability firefly has equipped
    private int equippedLight;
    private float lightResource;

    // Link to firefly Resource UI
    public Slider resourceBar;
    public Text resourceCount;

    public float tempLightCost;

    void Awake(){
        equippedLight = 0;
        restrictMovement = false;
        tScript = traveller.GetComponent<travellerScript>();
        if (tScript == null) {
            Debug.Log("Could not find tscript");
        }
    }
	// Use this for initialization
    void Start () {
        lightReady = false;
        lightResource = 100;
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        audioSource = GetComponent<AudioSource>();
       
	}
	
	// Update is called once per frame
    void Update() {

      
         // Toggle between 4 types of light magic
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("X")){
            equippedLight = 0;
            setFireFlyMaterial();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown("Square")){
            equippedLight = 1;
            setFireFlyMaterial();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("Triangle")){
            equippedLight = 2;
            setFireFlyMaterial();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetButtonDown("Circle")){
            equippedLight = 3;
            setFireFlyMaterial();
        }
            // Restart button
        if (Input.GetButtonDown("L1") || Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }


        // Only interact while r2 is pulled, set tag otherwise release.
        if (Input.GetButtonDown("R2") || Input.GetMouseButtonDown(0)) {
            lightReady = true;
        }
        if (Input.GetButtonUp("R2") || Input.GetMouseButtonUp(0)) {
            lightReady = false;
        }

    }
    
    void FixedUpdate()
    {
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

            rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Switch")  && lightReady)
        {
            count = count + 1;
            SetCountText();
            cCollider = other.GetComponentInParent<CapsuleCollider>();
            lampLight = other.gameObject.GetComponentInChildren<Light>();
            Material bulb = other.GetComponentInChildren<Renderer>().material;
            //Behaviour halo =(Behaviour)other.GetComponent ("Halo");
            
            Debug.Log(bulb.name);
            if (bulb == null)
                Debug.Log("HHH");
            if (lampLight.intensity > 0)
            {
                lampLight.intensity = 0;
                cCollider.enabled = false;
                audioSource.clip = offSoundEffect;
                audioSource.Play();
                tScript.setTarget(other.transform.parent.transform, lampLight.intensity);

                bulb.DisableKeyword("_EMISSION");
                
                addResource(tempLightCost);

                //halo.enabled = false;
                
            }
            else{
                //setLightColor(lampLight, equippedLight);
                if (lightResource >=tempLightCost){
 
                setChildLight(other.GetComponentsInChildren<Light>());
                
                lampLight.intensity = 3;
                cCollider.enabled = true;
                audioSource.clip = onSoundEffect;
                audioSource.Play();
                tScript.setTarget(other.transform.parent.transform, lampLight.intensity);
                bulb.EnableKeyword("_EMISSION");
                setMaterialColor(bulb, equippedLight);

                // halo.enabled = true;

                addResource(-tempLightCost);
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
        if (color == 0) {
            //Color.TryParseHexString("#F00", out light.color);
            //material.color = Color.yellow;
            material.SetColor("_EmissionColor", Color.yellow);
            setTrailRenderer();
        }
        else if (color == 1) {
            //material.color = Color.red;
            material.SetColor("_EmissionColor", Color.red);
            setTrailRenderer();
        }
        else if (color == 2) {
            //material.color = Color.blue;
            material.SetColor("_EmissionColor", Color.magenta);
            setTrailRenderer();
        }
        else if (color == 3) {
            //material.color = Color.green;
            material.SetColor("_EmissionColor", Color.green);
            setTrailRenderer();
        }
    }
    void setLightColor(Light light, int color) {
        if (color == 0) {
            //Color.TryParseHexString("#F00", out light.color);
            light.color = Color.yellow;
        }
        else if (color == 1) {
            light.color = Color.red;
        }
        else if (color == 2) {
            light.color = Color.magenta;
        }
        else if (color == 3) {
            light.color = Color.green;
        }
    }

    void setFireFlyMaterial() {
       MeshRenderer[] meshRenderers  = GetComponentsInChildren<MeshRenderer>();
       foreach (MeshRenderer m in meshRenderers) {
           if (m.gameObject.name == "Sphere")
                setMaterialColor(m.material,equippedLight);
       }

    }
    void setGradient(ref GradientColorKey[] colorKey, ref GradientAlphaKey[] alphaKey, ref Gradient gr) {
        Color color1;
        Color color2;

        if (equippedLight == 0) {
        //Color.TryParseHexString("#F00", out light.color);
           color1 = Color.yellow;
           color2 = Color.blue;

        }
        else if (equippedLight == 1) {
            color1 = Color.red;
            color2 = Color.blue;
        }
        else if (equippedLight == 2) {
            color1 = Color.magenta;
            color2 = Color.white;
        }
        else if (equippedLight == 3) {
            color1 = Color.green;
            color2 = Color.blue;
           
        }
        else {
            color1 = Color.yellow;
            color2 = Color.blue;
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
        Debug.Log("Adding " + value);
        resourceBar.value = lightResource;
        int resourceValue = (int)lightResource;
        resourceCount.text = resourceValue.ToString();
    }

    public float getResource() {
        return lightResource;
    }

}
