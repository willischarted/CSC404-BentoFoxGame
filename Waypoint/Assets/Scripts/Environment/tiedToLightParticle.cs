using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiedToLightParticle : MonoBehaviour
{
    public Light parentLight;
    private ParticleSystem obj;
    private ParticleSystem.MainModule ma;
    // Use this for initialization
    void Start()
    {
        obj = GetComponent<ParticleSystem>();
        ma = obj.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentLight.intensity > 0 && parentLight.enabled == true)
        {
            ma.startColor = parentLight.color;
        }
        else
        {
            ma.startColor = new Color(0, 0, 0, 0);
        }
    }
}
