using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class travellerMoodController : MonoBehaviour {

	//travellerMovement travellerMovement;

	Animator anim;

	[SerializeField]
	private List<GameObject> nearbyMonsters;

	private bool isScared;


	// Use this for initialization
	void Start () {

		anim = gameObject.transform.parent.GetComponent<Animator>();
		if (anim == null)
			Debug.Log("Could not find anim");
		
		isScared = false;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (nearbyMonsters.Count >= 1 && !isScared) {
			setTravellerScared(true);
		}

		else if (nearbyMonsters.Count == 0 && isScared) {
			setTravellerScared(false);
		}
		
	}

	 private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
          
            if (!nearbyMonsters.Contains(other.gameObject)){
				nearbyMonsters.Add(other.gameObject);
			}

		}

      
    }

	 private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
          
            if (nearbyMonsters.Contains(other.gameObject)){
				nearbyMonsters.Remove(other.gameObject);
			}

		}

      
    }

	
    public void setTravellerScared(bool _isScared) {
        isScared = _isScared;
        anim.SetBool("isScared", isScared);
    }


}
