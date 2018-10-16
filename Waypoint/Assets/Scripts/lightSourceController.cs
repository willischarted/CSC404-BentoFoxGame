using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightSourceController : MonoBehaviour {
	/*
		1 = default -> attracts both
		2 = traveller -> attracts traveller only
		3 = monster	-> attracts monster
	 */
	[SerializeField]
	private int currentLightType = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCurrentLightType(int type) {
		currentLightType = type;
	}

	public int getCurrentLightType() {
		return currentLightType;
	}
}
