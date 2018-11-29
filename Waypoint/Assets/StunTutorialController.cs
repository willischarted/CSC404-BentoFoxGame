using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTutorialController : MonoBehaviour {

	public EnemyMovement monsterScript;

	public int numSucess;

	public bool increment;

	public int numNeeded;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (monsterScript.getIsStunned()) {
			if (!increment){
				numSucess ++;
				increment = true;
				//update UI
			}
			
		}

		if (!monsterScript.getIsStunned()) {
			if (increment) {
				increment = false;
			}
		}

		if ()

		
		
	}
}
