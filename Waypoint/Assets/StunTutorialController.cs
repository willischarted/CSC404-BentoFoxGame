using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StunTutorialController : MonoBehaviour {

	public EnemyMovement monsterScript;

	


	public int numSuccess;

	public bool increment;

	public int numNeeded;

	public Text numUI;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (monsterScript.getAttackInterrupt()) {
			if (!increment){
				numSuccess ++;
				increment = true;

				numUI.text = numSuccess + "/" + numNeeded;
				//update UI
			}
			
		}

		if (!monsterScript.getAttackInterrupt()) {
			if (increment) {
				increment = false;
			}
		}

		if (numSuccess == numNeeded) {
			//Update UI
			numUI.text = "COMPLETE";
			return;
		}

		
		
	}
}
