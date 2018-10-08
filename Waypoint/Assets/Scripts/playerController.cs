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

    public AudioClip onSoundEffect;
    public AudioClip offSoundEffect;
 	AudioSource audioSource;

    private bool lightReady;

    private bool restrictMovement;
    
    private int equippedLight;

    private int lightResource;

    public Slider resourceBar;
    public Text resourceCount;

    void Awake(){
        equippedLight = 0;
        restrictMovement = false;
        tScript = traveller.GetComponent<travellerScript>();
        if (tScript == null) {
            Debug.Log("Cound not tScript");
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
        if (Input.GetKeyDown(KeyCode.V) || Input.GetButtonDown("X")){
            equippedLight = 0;
            setFireFlyMaterial();
        }
        if (Input.GetKeyDown(KeyCode.B) || Input.GetButtonDown("Square")){
            equippedLight = 1;
            setFireFlyMaterial();
        }
        if (Input.GetKeyDown(KeyCode.N) || Input.GetButtonDown("Triangle")){
            equippedLight = 2;
            setFireFlyMaterial();
        }
        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Circle")){
            equippedLight = 3;
            setFireFlyMaterial();
        }

        // Restart button
        if (Input.GetButtonDown("L1")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        // Heal/Stun only while button pressed
        if (Input.GetButton("R1")) {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            Debug.Log("Healing or stunning");
        }

        // Only interact while r2 is pulled, set tag otherwise release.
        if (Input.GetButtonDown("R2")) {
            lightReady = true;
        }
        if (Input.GetButtonUp("R2")) {
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
        if (other.gameObject.CompareTag("Switch") && lightReady)
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
                
                bulb.DisableKeyword("_EMISSION");
                

                //halo.enabled = false;

                addResource(20);
                
            }
            else{
                //setLightColor(lampLight, equippedLight);
                if (lightResource >=20){
                setChildLight(other.GetComponentsInChildren<Light>());
                
                lampLight.intensity = 3;
                cCollider.enabled = true;
                audioSource.clip = onSoundEffect;
                audioSource.Play();
                tScript.setTarget(other.transform.parent.transform);
                bulb.EnableKeyword("_EMISSION");
                setMaterialColor(bulb, equippedLight);
               
               // halo.enabled = true;

                addResource(-20);
                }
            }
            
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    //emisve
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

    void setTrailRenderer() {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        if (tr == null) {
            Debug.Log("Could not find trailrender");
        }


        Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;
        if (equippedLight == 0) {
            //Color.TryParseHexString("#F00", out light.color);
            gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.yellow;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.blue;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
            tr.colorGradient= gradient;
        }
        else if (equippedLight == 1) {
            //light.color = Color.red;
               //Color.TryParseHexString("#F00", out light.color);
            gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.red;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.blue;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
            tr.colorGradient= gradient;
        }
        else if (equippedLight == 2) {
           // light.color = Color.blue;
              //Color.TryParseHexString("#F00", out light.color);
            gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.magenta;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.white;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
            tr.colorGradient= gradient;
        }
        else if (equippedLight == 3) {
            //light.color = Color.green;
               //Color.TryParseHexString("#F00", out light.color);
            gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = Color.green;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.blue;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
            tr.colorGradient= gradient;
        }
        

    }

    void setChildLight(Light[] list) {
        foreach (Light l in list) {
            setLightColor(l,equippedLight);
        }
    }

    public void setRestrictMovement(bool _restrictMovement) {
        restrictMovement = _restrictMovement;
    }

    public void addResource(int value) {
        //Debug.Log(lightResource);
        //Debug.Log(lightResource += value);
        lightResource += value;
        Debug.Log("Adding " + value);
        resourceBar.value = lightResource;
        resourceCount.text = lightResource.ToString();
    }
}
