using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class abilityIconController : MonoBehaviour {

	public Image abilityIcon;
//	public Image backgroundIcon;

	public Sprite icon1;
	public Sprite icon2;
	public Sprite icon3;

//	public Sprite background1;
//	public Sprite background2;
//	public Sprite background3;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void updateAbilityIcon(int ability) {
		if (ability == 1) {
			abilityIcon.sprite = icon1;
			//backgroundIcon.sprite = background1;
		}
		else if (ability == 2) {
			abilityIcon.sprite = icon2;
		//	backgroundIcon.sprite = background2;

		}

		else if (ability == 3) {
			abilityIcon.sprite = icon3;
			//backgroundIcon.sprite = background3;
		}
	}
}
