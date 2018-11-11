using System.Collections; using System.Collections.Generic; using UnityEngine;  public class StationaryController : MonoBehaviour {     public GameObject childMonster1;      private int flag;
    public AudioSource awakenSound;
    public AudioSource attackSound;     private GameObject traveler;     // Use this for initialization  void Start () {         flag = 0;         awakenSound = transform.Find("Audio Source").transform.GetComponent<AudioSource>();
        awakenSound.enabled = true;
        traveler = GameObject.FindGameObjectWithTag("Traveller");
    }

    // Update is called once per frame
    void Update () {         if (flag == 1){             childMonster1.SetActive(false);             gameObject.SetActive(false);         }         if (Vector3.Distance(traveler.transform.position, transform.position) < 5)
        {
            Debug.Log("wake up");
            if (!awakenSound.isPlaying)
            {
                awakenSound.Play();
            }
        }     }      private void OnTriggerEnter(Collider other)     {
         if (other.gameObject.CompareTag("Traveller")){                          flag = 1;             travellerHealth th1 = other.gameObject.GetComponentInChildren<travellerHealth>();             if (th1 == null){                 Debug.Log("Could not find travellerHealth script");             }else{                 th1.TakeStationaryDamage();             }         }     } }  