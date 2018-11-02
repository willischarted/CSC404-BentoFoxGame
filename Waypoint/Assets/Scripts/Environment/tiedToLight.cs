using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiedToLight : MonoBehaviour {
    public Light parentLight;
    private Projector obj;
	// Use this for initialization
	void Start () {
        obj = GetComponent<Projector>();
    }
	
	// Update is called once per frame
	void Update () {
        if (parentLight.intensity > 0 && parentLight.enabled == true)
        {
            obj.enabled = true;
        }
        else
        {
            obj.enabled = false;
        }
    }
}
