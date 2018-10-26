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

        //When low health, the screen shows a warning image
	}

    public void TakeBasicDamage (int amount){
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        //TODO: clothe material change
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public void TakeStationaryDamage (){
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
