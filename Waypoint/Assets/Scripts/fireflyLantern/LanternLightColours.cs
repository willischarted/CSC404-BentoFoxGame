using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternLightColours : MonoBehaviour { 
    public float lightFactor;
    float lightValue;
    public Material Lantern;
    public GameObject player;
    public float sphereNum;
    private float startingResource;
    private float currentResource;

    void Start () {
        startingResource= player.GetComponent<playerControllerCopy>().startingResource;
        currentResource = player.GetComponent<playerControllerCopy>().lightResource;
        lightFactor = 4.0f * (currentResource / startingResource);
        Lantern.SetColor("_EmissionColor", Color.yellow * lightFactor);
    }
	
	// Update is called once per frame
	void Update () {
        currentResource = player.GetComponent<playerControllerCopy>().lightResource;
        float sphereFactor = ((currentResource / startingResource)-((5f - sphereNum) / 5f))/((0.2f)*sphereNum);
        if (sphereFactor < 0f) { sphereFactor = 0f; }
        lightFactor = 3.0f * sphereFactor;
        if (player.GetComponent<playerControllerCopy>().equippedLight == 1)
        {
            Lantern.SetColor("_EmissionColor", Color.yellow * lightFactor);
        }
        else if (player.GetComponent<playerControllerCopy>().equippedLight == 2)
        {
            Lantern.SetColor("_EmissionColor", Color.blue * lightFactor);

        }
        else
        {
            Lantern.SetColor("_EmissionColor", new Color(0.5803f,0.1294f,0.6588f,1f) * lightFactor);
        }
        
    }
}
