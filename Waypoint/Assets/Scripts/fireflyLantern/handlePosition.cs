using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handlePosition : MonoBehaviour {
    public Transform handle;
    public Transform handL;
    public Transform handR;
	// Use this for initialization
	void Start () {
        handle.position = Vector3.Lerp(handL.position, handR.transform.position, 0.5f);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        handle.position = Vector3.Lerp(handL.position, handR.transform.position, 0.5f);
    }
}
