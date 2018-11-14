using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class travellerHealth : MonoBehaviour {
    public float startingHealth = 100;
    public float currentHealth;
    public Slider healthSlider;
    public GameObject goMenu;
    //public Image lowHealthImage;
    public Image damageImage;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public float flashSpeed = 50f;
    private MeshRenderer[] meshRenderers;
    private MeshRenderer meshRendererTraveller;

    private GameOver gameEnder;
    Material cloak;
    Material hood;
    Material l_ear;
    Material r_ear;
    float lightValue;
    float orig_lightValue;
    Animator anim;
    travellerMovement travellerMovement;
    public bool isDead;
    bool isLowHealth;
    bool damaged;
    bool healed;
    private AudioSource takeStationary;
    //Material cloaktwo;

	void Awake ()
    {
        gameEnder = goMenu.GetComponent<GameOver>();
        anim = GetComponent<Animator>();
        travellerMovement = GetComponent<travellerMovement>();
        currentHealth = startingHealth;
        takeStationary = transform.Find("Audio Source").transform.GetComponent<AudioSource>();
        takeStationary.enabled = true;
        cloak = transform.Find("traveler").transform.Find("model:geo")
            .transform.Find("model:dress_GEO").GetComponentInChildren<SkinnedMeshRenderer>().material;
        hood = transform.Find("traveler").transform.Find("model:geo")
            .transform.Find("model:hoodie_GEO").GetComponentInChildren<SkinnedMeshRenderer>().material;
        l_ear = transform.Find("traveler").transform.Find("model:geo")
            .transform.Find("model:ear_L_GEO").GetComponentInChildren<SkinnedMeshRenderer>().material;
        r_ear = transform.Find("traveler").transform.Find("model:geo")
            .transform.Find("model:ear_R_GEO").GetComponentInChildren<SkinnedMeshRenderer>().material;
        //cloak = GetComponent<MeshRenderer>().material;
        lightValue =  0.005f;
        orig_lightValue = 0.005f;
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        Debug.Log("cloak: " + cloak);
    }
	
	void Update () {
        //When take damege, flash color
        /*  Testing of cloth change*/
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeBasicDamage(10);
            Debug.Log(lightValue);
            Debug.Log("cloak: " + cloak);
            lightValue = lightValue * (currentHealth / startingHealth);
            lightValue = Mathf.Clamp(lightValue, -0.002f, 0.005f);
            cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            hood.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            l_ear.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            r_ear.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            cloak.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            hood.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            l_ear.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            r_ear.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        }

        if(damaged){
            damageImage.color = flashColour;
        }else{
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        healed = false;
        //When low health, the screen shows a warning image
    }
    //TODO: getHeal
    public void GetHeal(int amount){
        healed = true;
        Mathf.Clamp(currentHealth += amount, 0, startingHealth);
       // currentHealth += amount;
        healthSlider.value = currentHealth;

        //TODO: clothe material change, not function now
        lightValue = orig_lightValue * (currentHealth / startingHealth);
        lightValue = Mathf.Clamp(lightValue, -0.002f, 0.005f);
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        hood.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        l_ear.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        r_ear.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        cloak.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        hood.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        l_ear.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        r_ear.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
    }

    //TODO: warningLowHealth
    public void TakeBasicDamage (int amount){
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        //TODO: clothe material change, not function now
        lightValue = orig_lightValue * (currentHealth / startingHealth);
        lightValue = Mathf.Clamp(lightValue, -0.002f, 0.005f);
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        hood.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        l_ear.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        r_ear.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        cloak.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        hood.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        l_ear.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        r_ear.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void TakeStationaryDamage (){
        damaged = true;
        currentHealth = currentHealth / 2;
        healthSlider.value = currentHealth;
        takeStationary.Play();
        //TODO: clothe material change
        lightValue = orig_lightValue * (currentHealth / startingHealth);
        lightValue = Mathf.Clamp(lightValue, -0.002f, 0.005f);
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        hood.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        l_ear.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        r_ear.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        cloak.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        hood.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        l_ear.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        r_ear.SetColor("_Color", new Color(255f, 255f, 255f, 1.0f) * lightValue);

        if (currentHealth <= 3 && !isDead)
        {
            Death();
        }

    }

    void Death (){
        isDead = true;
        anim.SetTrigger("isDead");
        travellerMovement.enabled = false;
        Invoke("endGame", 1.5f);
    }

    void endGame() {
        gameEnder.gameOverr();
    }

    public void restart(){
        //TODO: Restart
    }
}
