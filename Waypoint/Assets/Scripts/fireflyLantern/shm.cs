using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shm : MonoBehaviour {

    public GameObject firefly;
    public Transform pivot;
    public float separation;
    public float speed = 0.5f;
    private float startAngle;
    private float endAngle;
    private float fTimer = 0.0f;
    private Vector3 v3T = Vector3.zero;
    public float angleUpLimit;
    public float angleLowLimit;

    void FixedUpdate()
    {
        float moveVertical = Input.GetAxis("Vertical");
        startAngle = angleUpLimit - moveVertical * 20;
        endAngle = angleLowLimit - moveVertical * 20;
        float f = (Mathf.Sin(fTimer * speed - Mathf.PI / 2.0f) + 1.0f) / 2.0f;
        v3T.Set(Mathf.Lerp(startAngle, endAngle, f),firefly.transform.rotation.y,0f);
        pivot.eulerAngles = v3T;
        fTimer += Time.deltaTime;
    }
}
