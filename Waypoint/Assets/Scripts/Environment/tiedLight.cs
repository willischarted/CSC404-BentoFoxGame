using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiedLight : MonoBehaviour {
    public Light parentLight;
    private Light lt;
	// Use this for initialization
	void Start () {
        lt = GetComponent<Light>();

    }
	
	// Update is called once per frame
	void Update () {
        lt.intensity = parentLight.intensity;
		lt.color = parentLight.color;
    }
}
