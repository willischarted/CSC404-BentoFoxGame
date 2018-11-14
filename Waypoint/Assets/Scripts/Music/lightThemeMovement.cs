using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightThemeMovement : MonoBehaviour {

    private GameObject traveller;

	// Use this for initialization
	void Start () {
        
        findTraveller();
            
		
	}
	
	// Update is called once per frame
	void Update () {
        if (traveller != null)
            transform.position = traveller.transform.position;
        else 
            findTraveller();
	}

    void findTraveller() {
        traveller = GameObject.FindGameObjectWithTag("Traveller");

        if (traveller == null)
        {
            Debug.Log("Could not the traveller");
          
        }

    }
}
