using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{   
    // Minimum and Maximum to the camera rotation y value
    private const float yAngleMin = 0.0f;
    private const float yAngleMax = 25.0f; //starting rotation value


    // Transform of the gameobject we are following
    public Transform target;

    // Current values for the camera rotation
    private float currentX = 0.0f;
    private float currentY = 45.0f;


    // Sensitivity multiplier for panning camera
    public float turnSpeedX;
    public float turnSpeedY;

    // Offset from the target's location
    public Vector3 offset;

    private void Start()
    {
        
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X") * turnSpeedX;
        
        // -1f to make it NOT inverted
        currentY += Input.GetAxis("Mouse Y") * turnSpeedY * -1f;
        // Ensure it is within min and max y camera bounds
        currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
    }

    private void LateUpdate()
    {

       
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position);
    }
}
