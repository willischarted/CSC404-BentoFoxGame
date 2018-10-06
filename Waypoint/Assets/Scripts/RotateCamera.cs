using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateCamera : MonoBehaviour {
    
    // Variables needed for player cenetered camera control 
    ///////////////////////////////////////////////////////////////////////////////////////////////
	public float turnSpeed = 4.0f;
    public Transform player;
 
    private Vector3 offsetHorizontal;

    private Vector3 offsetVertical;

    public float yOffsetHorizontal;

    public float zOffsetHorizontal;
    
    public float yOffsetVertical;

    public float zOffsetVertical;
    ///////////////////////////////////////////////////////////////////////////////////////////////



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

    // Used for camera transitions
    ///////////////////////////////////////////////////////////////////////////////////////////////
    private Vector3 tacticleDestination;
    private Vector3 defaultDestination;
    Quaternion tacticleRotation;
    private Quaternion defaultRotation;
    public GameObject ground;
    ///////////////////////////////////////////////////////////////////////////////////////////////
    
    void Start () {
         offsetHorizontal = new Vector3(player.position.x, player.position.y + yOffsetHorizontal, player.position.z + zOffsetHorizontal);
         offsetVertical = new Vector3(player.position.x, player.position.y + yOffsetHorizontal, player.position.z + zOffsetHorizontal);
         tacticalView = false;
         cameraMoving = false;
         cameraMovingBack = false;
     }
    

    void Update() {

        if (Input.GetKeyDown(KeyCode.R)) {
            offsetHorizontal = new Vector3(player.position.x, player.position.y + yOffsetHorizontal, player.position.z + zOffsetHorizontal);
            offsetVertical = new Vector3(player.position.x, player.position.y + yOffsetHorizontal, player.position.z + zOffsetHorizontal);
        }


    /* Previous Implementation of "tactical" cam
        if (Input.GetKey(KeyCode.Space)){
            tacticalView = true;
            Camera cam = tacticalCamera.GetComponent<Camera>();
            if (cam ==null)
                Debug.Log("Couldnt find");
            cam.enabled = true;
            
            GetComponent<Camera>().enabled = false;
            cameraText.text = "Tactical View";
        }

        if (Input.GetKeyUp(KeyCode.Space)){
            tacticalView = false;
            GetComponent<Camera>().enabled = true;
             Camera cam = tacticalCamera.GetComponent<Camera>();
            if (cam ==null)
                Debug.Log("Couldnt find");
            cam.enabled = false;
            //GetComponentInChildren<Camera>().enabled = false;
          //  
             cameraText.text = "";
        }
        */

        if (Input.GetKeyDown(KeyCode.Space)) {
            tacticleDestination = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
            tacticleRotation = new Quaternion(transform.rotation.x + 30f,transform.rotation.y,transform.rotation.z,transform.rotation.w);
            cameraMoving = true;
            transform.LookAt(player.position);
            tacticalView = true;
            defaultDestination = transform.position;
            defaultRotation = transform.rotation;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            cameraMoving = false;
            cameraMovingBack = true;
        }

        
        if (cameraMoving) {
            moveCamera();
            if (Vector3.Distance(transform.position, tacticleDestination) < 1f) {
                cameraMoving = false;
            }
        }
 
        if (cameraMovingBack) {
            resetCamera();
             if (Vector3.Distance(transform.position, defaultDestination) < 1f) {
                cameraMovingBack = false;
                tacticalView = false;
            }

        }

        //TODO make sure camera panning is relative to current directionality
        if (tacticalView) {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
          //  var camera = Camera.main;
           // Vector3 relativeForward = camera.transform.forward;
           // Vector3 relativeRight = camera.transform.right;
            
           // relativeForward.y = 0f;
           // relativeRight.y = 0f;
           // relativeForward.Normalize();
           // relativeRight.Normalize();

           // Vector3 moveDirection = relativeForward * moveVertical + relativeRight * moveHorizontal;
            Vector3 moveDirection = new Vector3(moveHorizontal, 0f,moveVertical );
            //transform.Translate(moveDirection * Time.deltaTime * cameraSpeed);
            transform.position = transform.position += moveDirection;
        }

    }

    void moveCamera() {
        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(transform.position, tacticleDestination, step);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, tacticleRotation, step);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, ground.transform.rotation, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ground.transform.position - transform.position), step);
    
    }

     void resetCamera() {
       
        float step = speed * Time.deltaTime;
        //transform.rotation = defaultRotation;
        
        gameObject.transform.position = Vector3.MoveTowards(transform.position, defaultDestination, step);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, tacticleRotation, step);
      //  transform.rotation = Quaternion.RotateTowards(transform.rotation, ground.transform.rotation, step);
       // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ground.transform.position - transform.position), step);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), step);
    }
     void LateUpdate()
     {
         if (!tacticalView && !tacticalView) {
             

            offsetHorizontal = Quaternion.AngleAxis (Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offsetHorizontal;
            offsetVertical= Quaternion.AngleAxis (Input.GetAxis("Mouse Y") * turnSpeed, Vector3.up) * offsetHorizontal;
            transform.position = player.position + offsetHorizontal; 
            transform.LookAt(player.position);
         }
     }
}
