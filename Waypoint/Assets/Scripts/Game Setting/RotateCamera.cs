using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCamera : MonoBehaviour {

    ////////////////////////////////////////////////////////////////////////////////////////////////
     // Minimum and Maximum to the camera rotation y value
    private const float yAngleMin = 0.0f;
    private const float yAngleMax = 50.0f; 


    // Transform of the gameobject we are following
    public Transform target;
    private playerController targetController;

    // Current values for the camera rotation
    private float currentX = 0.0f;
    private float currentY = 25.0f;  //starting rotation value


    // Sensitivity multiplier for panning camera
    public float turnSpeedX;
    public float turnSpeedY;

    // Offset from the target's location
    public Vector3 offset;

    ////////////////////////////////////////////////////////////////////////////////////////////////

  



    bool tacticalView;

    //Previous implementation -> static secondary camera 
    private GameObject tacticalCamera;
    public Text cameraText;
    
    
    // Current transition between default/tactical
    private bool cameraMoving;
    private bool cameraMovingBack;

    // Camera transition speed and camera pan speed.
    public float speed;
    public float cameraSpeed;

    Vector3 relativeForward;
     Vector3 relativeRight;
    

    // Used for camera transitions
    ///////////////////////////////////////////////////////////////////////////////////////////////
    private Vector3 tacticleDestination;
    private Vector3 defaultDestination;
    Quaternion tacticleRotation;
    private Quaternion defaultRotation;
   // public GameObject ground;
    ///////////////////////////////////////////////////////////////////////////////////////////////
    
    void Start () {
       
         tacticalView = false;
         cameraMoving = false;
         cameraMovingBack = false;


        targetController = target.GetComponent<playerController>();
        if (targetController == null)
            Debug.Log("Could not find targetcontroller!");
     }
    

    void Update() {

        if (!tacticalView && !cameraMoving && !cameraMovingBack) {
        currentX += Input.GetAxis("Mouse X") * turnSpeedX;
        
        // * -1f to make it  inverted/Notinverted
        currentY += Input.GetAxis("Mouse Y") * turnSpeedY * -1f;
        // Ensure it is within min and max y camera bounds
        currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);

        }
        

       // if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("L2")) {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Square")){
            tacticalView = !tacticalView;
            
            if (tacticalView) {
            targetController.setRestrictMovement(true);
            relativeForward = transform.forward;
            relativeRight = transform.right;
            tacticleDestination = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
            tacticleRotation = new Quaternion(transform.rotation.x + 30f,transform.rotation.y,transform.rotation.z,transform.rotation.w);
            cameraMoving = true;
            transform.LookAt(target.position);
            
            defaultDestination = transform.position;
            defaultRotation = transform.rotation;
            //cameraText.text = "Tactical View";
            cameraText.enabled = false;
            }
            else {
                cameraMoving = false;
                cameraMovingBack = true;

                targetController.setRestrictMovement(true);
            }
        }

/* 
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("L2")) {
            cameraMoving = false;
            cameraMovingBack = true;

            targetController.setRestrictMovement(true);
        }
*/
        
        if (cameraMoving) {
            moveCamera();
            if (Vector3.Distance(transform.position, tacticleDestination) < 1f) {
                cameraMoving = false;
                //targetController.setRestrictMovement(false);
            }
        }
 
        if (cameraMovingBack) {
            resetCamera();
             if (Vector3.Distance(transform.position, defaultDestination) < 1f) {
                cameraMovingBack = false;
                tacticalView = false;
                //cameraText.text = "";
                cameraText.enabled = true;
                targetController.setRestrictMovement(false);
            }

        }

        //ONLY CAMERA PAN WHEN IN TACTICAL AFTER CAMERA DONE MOVING
        if (tacticalView && !cameraMoving && !cameraMovingBack) {

            //camera pan direction should match players current direcionality
          //  relativeForward = transform.forward;
            //  relativeRight = transform.right;

            relativeForward.y = 0f;
            relativeRight.y = 0f;
            relativeForward.Normalize();
            relativeRight.Normalize();
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");




            Vector3 moveDirection = relativeForward * moveVertical + relativeRight * moveHorizontal;
            //transform.Translate(moveDirection * Time.deltaTime * cameraSpeed);
            transform.position = transform.position += moveDirection;
        }

    }



    void moveCamera() {
        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, tacticleDestination, step);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, tacticleRotation, step);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, ground.transform.rotation, step);
       
       
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ground.transform.position - transform.position), step);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), step);

    }

     void resetCamera() {
       
        float step = speed * 3 * Time.deltaTime; //moving back should be quicker
        //transform.rotation = defaultRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), step);
        
        gameObject.transform.position = Vector3.MoveTowards(transform.position, defaultDestination, step);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, tacticleRotation, step);
      //  transform.rotation = Quaternion.RotateTowards(transform.rotation, ground.transform.rotation, step);
       // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ground.transform.position - transform.position), step);
        
       
    }
     void LateUpdate()
     {  
         // camerarotation
         if (!tacticalView && !cameraMoving && !cameraMovingBack) {
             
     
            
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = target.position + rotation * offset;
            transform.LookAt(target.position);
             

         }
     }
}
