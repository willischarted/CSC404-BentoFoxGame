using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerController : MonoBehaviour {

	public GameObject target;
     public float rotateSpeed = 5;
     Vector3 offset;
     
     void Start() 
     {
         offset = target.transform.position - transform.position;
     }
     
     void LateUpdate() 
     {

		 Debug.Log (Input.GetAxisRaw("Mouse X"));

         float horizontal = Input.GetAxisRaw("Mouse X") * rotateSpeed;
         float vertical = Input.GetAxisRaw("Mouse Y") * rotateSpeed * -1f;
 
             target.transform.Rotate(vertical, horizontal, 0);
 
             float desiredAngle_X = target.transform.eulerAngles.x;
             float desiredAngle_Y = target.transform.eulerAngles.y;
 
             Quaternion rotation = Quaternion.Euler(desiredAngle_X, desiredAngle_Y, 0);
             transform.position = target.transform.position - (rotation * offset);
 
             transform.LookAt (target.transform);
    	 }
	 
 }
