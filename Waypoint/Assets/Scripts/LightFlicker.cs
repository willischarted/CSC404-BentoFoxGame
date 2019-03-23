using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {

    public Light brokenBulb;


    // Use this for initialization
    void Start () {
        StartCoroutine(Flickering());
	}
	
    IEnumerator Flickering()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.value);
            brokenBulb.enabled = !brokenBulb.enabled;
        }
    }
}
