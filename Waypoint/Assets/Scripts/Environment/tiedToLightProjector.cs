using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiedToLightProjector : MonoBehaviour {
    public GameObject light;
    private lightSourceController LSC;
    public Material defaultLight;
    public Material travLight;
    public Material monLight;
    private Projector obj;
	// Use this for initialization
	void Start () {
        LSC = light.GetComponent<lightSourceController>();
        obj = GetComponent<Projector>();
    }
	
	// Update is called once per frame
	void Update () {
        int lightType = LSC.getCurrentLightType();
        switch (lightType)
        {
            case 0:
                obj.enabled = false;
                break;
            case 1:
                obj.material = defaultLight;
                obj.enabled = true;
                break;
            case 2:
                obj.material = travLight;
                obj.enabled = true;
                break;
            case 3:
                obj.material = monLight;
                obj.enabled = true;
                break;
        }

        /*
        if (parentLight.intensity > 0 && parentLight.enabled == true)
        {
            obj.material.SetColor("_Color", parentLight.color);
            obj.enabled = true;
        }
        else
        {
            obj.enabled = false;
        }
        */
    }
}
