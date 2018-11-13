using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StationaryController : MonoBehaviour {
    public GameObject childMonster1;    
    private int flag;
    public AudioSource awakenSound;
    public AudioSource attackSound;
    private GameObject traveler;
    private float soundTimer;

    public GameObject monsterGeo;

    private Animator anim;

    // Use this for initialization
    void Start () {   


        flag = 0;
        awakenSound = transform.Find("Audio Source").transform.GetComponent<AudioSource>();
        awakenSound.enabled = true;
        traveler = GameObject.FindGameObjectWithTag("Traveller");
        soundTimer = 0f;
        anim = monsterGeo.GetComponent<Animator>();
        if (anim == null){
            Debug.Log("Can not find the anim");
        }
        
    }

    // Update is called once per frame
    void Update () {
        if (flag == 1){
            childMonster1.SetActive(false);
            gameObject.SetActive(false);
        }
        soundTimer += Time.deltaTime;        
        if (Vector3.Distance(traveler.transform.position, transform.position) < 5)
        {
            Debug.Log("wake up");
            if (soundTimer < 3)
            {
                if (!awakenSound.isPlaying)
                {
                    awakenSound.Play();
                }
            } else
            {
                awakenSound.Pause();
            }
        }  
        if(soundTimer > 5)
        {
            soundTimer = 0f;
        }
    }    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Traveller"))
        {
            
            travellerHealth th1 = other.gameObject.GetComponentInChildren<travellerHealth>();
            if (th1 == null){                
                Debug.Log("Could not find travellerHealth script");            
            }
            else{
                th1.TakeStationaryDamage();            
            }
            anim.SetTrigger("isDead");
            //Invoke("setDeath", 10);        
        }    
    }

    void setDeath() {
         flag = 1;  
    }
}