using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanterAnchorPosition : MonoBehaviour {
    public Transform leftArm;
    public Transform rightArm;
    public Transform handL;
    public Transform handR;
    public float x, y, z;
	// Use this for initialization
	void Start () {
        handL.position = leftArm.position+ new Vector3(x, y, z);
        handL.rotation = leftArm.rotation;
        handR.position = rightArm.position + new Vector3(x, y, z);
        handR.rotation = rightArm.rotation;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        handL.position = leftArm.position + new Vector3(x,y,z);
        handL.rotation = leftArm.rotation ;
        handR.position = rightArm.position+ new Vector3(x, y, z);
        handR.rotation = rightArm.rotation;
    }
}
