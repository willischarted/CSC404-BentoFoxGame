﻿using System.Collections;
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

    Material cloak;
    float lightValue;
    Animator anim;
    travellerMovement travellerMovement;
    bool isDead;
    bool isLowHealth;
    bool damaged;

	void Awake () {
        anim = GetComponent<Animator>();
        travellerMovement = GetComponent<travellerMovement>();
        currentHealth = startingHealth;

        cloak = GetComponentInChildren<Renderer>().material;
        lightValue = Mathf.Clamp(lightValue, -0.002f, 0.005f);
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
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
    //TODO: getHeal
    //TODO: warningLowHealth
    public void TakeBasicDamage (int amount){
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        //TODO: clothe material change, not function now
        lightValue = lightValue * (currentHealth / startingHealth);
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
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
        lightValue = lightValue * (currentHealth / startingHealth);
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);

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