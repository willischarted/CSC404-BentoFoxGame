using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleVerticleMovement : MonoBehaviour {

    private float defaultY;

    public float targetY;

    public float speed;

 

    [SerializeField]
    private bool movingToTarget;
	// Use this for initialization
	void Start () {
        movingToTarget = true;
        defaultY = transform.localPosition.y;

    }
	
	// Update is called once per frame
	void Update () {

            if (transform.localPosition.y >= targetY)
            {
                //Debug.Log(transform.localPosition.y);
                movingToTarget = false;
            }
            else if (transform.localPosition.y <= defaultY)
            {
                movingToTarget = true;
            }

            if (movingToTarget)
            {
                transform.localPosition += new Vector3(0, 1, 0) * Time.deltaTime * speed;
            }
            else
            {
                transform.localPosition -= new Vector3(0, 1, 0) * Time.deltaTime * speed;
            }
        
       
	}
}
