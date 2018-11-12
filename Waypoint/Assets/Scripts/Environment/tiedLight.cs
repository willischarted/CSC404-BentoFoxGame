using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiedLight : MonoBehaviour {
    public Light parentLight;
    public float intensity = 0;
    private Light lt;
	// Use this for initialization
	void Start () {
        lt = GetComponent<Light>();

    }
	
	// Update is called once per frame
	void Update () {
        if (parentLight.intensity == 0)
        {
            lt.intensity = parentLight.intensity;
        }
        else
        {
            if (intensity == 0)
            {
                lt.intensity = parentLight.intensity;
            }
            else
            {
                lt.intensity = intensity;
            }
        }
		lt.color = parentLight.color;
    }
}
