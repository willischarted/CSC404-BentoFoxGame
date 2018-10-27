using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class travellerHealth : MonoBehaviour {
    public float startingHealth = 100;
    public float currentHealth;
    public Slider healthSlider;
    //public Image lowHealthImage;
    public Image damageImage;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public float flashSpeed = 50f; 

    Animator anim;
    travellerMovement travellerMovement;
    bool isDead;
    bool isLowHealth;
    bool damaged;

	void Awake () {
        anim = GetComponent<Animator>();
        travellerMovement = GetComponent<travellerMovement>();
        currentHealth = startingHealth;
	}
	
	void Update () {
        //When take damege, flash color
        if(damaged){
            damageImage.color = flashColour;
        }else{
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
        //When low health, the screen shows a warning image
	}

    public void TakeBasicDamage (int amount){
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        //TODO: clothe material change
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void TakeStationaryDamage (){
        damaged = true;
        currentHealth = currentHealth / 2;
        healthSlider.value = currentHealth;
        //TODO: clothe material change
        if (currentHealth <= 3 && !isDead)
        {
            Death();
        }

    }

    void Death (){
        isDead = true;
        anim.SetTrigger("isDead");
        travellerMovement.enabled = false;
        restart();
    }

    public void restart(){
        //TODO: Restart
    }
}
