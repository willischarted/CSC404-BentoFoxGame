using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour {

	public Transform player;
	public Transform trav;

	public Transform playerCamera;

	//public Sprite travellerIcon;

	//public GameObject indicatorIcon;
	public GameObject onscreenIcon;
	public GameObject onscreenIconTraveller;

	public Canvas canvas;

	private GameObject miniMapUI;
	private Animator miniMapAnim;
	private GameObject miniMapCamera;
	private Animator miniMapCameraAnim;

	private GameObject playerGameObject;
	private playerControllerCopy pScript;

	//public Camera miniMapCamera;
	
	
	void Start() {

		playerGameObject = GameObject.FindGameObjectWithTag("Player");
		if (playerGameObject == null)
			Debug.Log("Could not find playergameobject");

		pScript = playerGameObject.GetComponent<playerControllerCopy>();
		if (pScript == null)
			Debug.Log("Could not find pscript");

		miniMapUI = GameObject.FindGameObjectWithTag("MiniMap");
		if (miniMapUI == null)
			Debug.Log("Could not find the minimapUI");
		
		miniMapAnim = miniMapUI.GetComponent<Animator>();
		if (miniMapAnim == null)
			Debug.Log("Could not find minimapAnim");

		miniMapCamera = GameObject.FindGameObjectWithTag("MiniMapCamera");
		
		if (miniMapCamera == null)
			Debug.Log("Could not find the minimapCamera");
		miniMapCameraAnim = miniMapCamera.GetComponent<Animator>();
		
		//onscreenIcon = Instantiate(indicatorIcon, indicatorIcon.transform.position, indicatorIcon.transform.rotation);
		//onscreenIcon.SetActive(false);
		//onscreenIcon.transform.parent = canvas.transform;
	}

	void Update() {								//temp value to check	
		//if (Vector3.Distance(player.position, trav.position) > 10f) {	
			//set the icon to the edge of the screen 
		//}
		//get the angle of rotation from the player
		if (Input.GetKeyDown(KeyCode.L)) {
			Debug.Log("the angle between the trav and the player is: ");
			//Debug.Log(Vector3.Angle(player.position, trav.position));
			float fAngle = Vector3.Cross(player.position.normalized,trav.position.normalized).y;
			fAngle *= 180.0f;
			Debug.Log(fAngle);

		}

		if (Input.GetButtonDown("TouchPad") || Input.GetKeyDown(KeyCode.M)) {

			

			miniMapAnim.SetBool("isExpand", !miniMapAnim.GetBool("isExpand"));
			miniMapCameraAnim.SetBool("isZoom", !miniMapCameraAnim.GetBool("isZoom"));

			bool shouldPause = miniMapAnim.GetBool("isExpand");
			if (shouldPause)
				//restrict player movement
				restrictPlayer();
			else
				//unrestrict player movement
				Invoke("unrestrictPlayer", 1.2f);
		
		}


		Vector3 travScreenPos = Camera.main.WorldToScreenPoint(trav.position);

		Vector3 travIconScreenPos = Camera.main.WorldToScreenPoint(trav.position);
	//	Vector3 travScreenPos = miniMapCamera.WorldToScreenPoint(trav.position);
		
		
		
		//Debug.Log(travScreenPos);

		if (travScreenPos.z > 0 && 
			travScreenPos.x > 0 && travScreenPos.x < Screen.width &&
			travScreenPos.y > 0 && travScreenPos.y < Screen.height)
	//	RectTransform objectRectTransform = canvas.GetComponent<RectTransform> ();
	//	if (travScreenPos.z > 0 && 
	//		travScreenPos.x > 0 && travScreenPos.x < objectRectTransform.rect.width &&
	//		travScreenPos.y > 0 && travScreenPos.y < objectRectTransform.rect.height)
		{
			//Debug.Log("Onscreen");

			//get rid of the arrow indicator?
			onscreenIcon.SetActive(false);
			onscreenIconTraveller.SetActive(false);
			//onscreenIcon.transform.parent = null;
			//indicatorIcon.SetActive(false);
		}
		else 
		{
			//Debug.Log("Offscreen");
			if (travScreenPos.z < 0)
				travScreenPos *= -1;
			// flip the coordinates, everything is mirrored. Arrows point to something then flip around

			Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;
			
		
		//	Vector3 screenCenter = new Vector3(objectRectTransform.rect.width, objectRectTransform.rect.height, 0) / 2;

			// make the center point of the screen 0,0 instead of bottom left
			travScreenPos -= screenCenter;
			travIconScreenPos -= screenCenter;

			//find angle between center of screen and where arrow will be pointing
			float indicatorAngle = Mathf.Atan2(travScreenPos.y, travScreenPos.x);
			indicatorAngle -= 90 * Mathf.Deg2Rad;

			//tells you roghly where to look at
			float angleCos = Mathf.Cos(indicatorAngle);
			float angleSin = -Mathf.Sin(indicatorAngle);

			//now we must find out where the put the arrow
			travScreenPos = screenCenter + new Vector3(angleSin*150, angleCos * 150, 0);

			travIconScreenPos = screenCenter + new Vector3(angleSin*150, angleCos * 150, 0);

			//slope formula
			float m = angleCos / angleSin;

			Vector3 screenBounds = screenCenter * 0.85f;
			Vector3 iconScreenBounds = screenCenter * 0.70f;

			//up/down
			if (angleCos >0) {
				travScreenPos = new Vector3(screenBounds.y/m, screenBounds.y, 0);
				travIconScreenPos = new Vector3(iconScreenBounds.y/m, iconScreenBounds.y, 0);
			}
			//down
			else {
				travScreenPos = new Vector3(-screenBounds.y/m, -screenBounds.y, 0);
				travIconScreenPos = new Vector3(-iconScreenBounds.y/m, -iconScreenBounds.y, 0);
			}
			// out of bounds, 
			if (travScreenPos.x > screenBounds.x) {
				travScreenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
				travIconScreenPos = new Vector3(iconScreenBounds.x, iconScreenBounds.x * m, 0);
			}
			else if (travScreenPos.x < -screenBounds.x) {
				travScreenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
				travIconScreenPos = new Vector3(-iconScreenBounds.x, -iconScreenBounds.x * m, 0);
			}

			travScreenPos += screenCenter;
			travIconScreenPos += screenCenter;


		
			onscreenIcon.transform.position = travScreenPos;
			//Vector3 iconScreenPos = travScreenPos + new Vector3 (0f,10f,0f);
			onscreenIconTraveller.transform.position = travIconScreenPos;
			
			onscreenIcon.transform.rotation = Quaternion.Euler(0,0,indicatorAngle * Mathf.Rad2Deg);
			
			
			//onscreenIcon.transform.parent = canvas.transform;
			onscreenIcon.SetActive(true);
			onscreenIconTraveller.SetActive(true);
			//indicatorIcon.transform.localPosition = travScreenPos;
			//indicatorIcon.transform.localRotation = Quaternion.Euler(0,0,indicatorAngle*Mathf.Rad2Deg);
			//indicatorIcon.SetActive(true);
		}
	}

	void restrictPlayer() {
		//Time.timeScale = 0f;
		pScript.setRestrictMovement(true);
	}

	void unrestrictPlayer() 
	{
		pScript.setRestrictMovement(false);
	}
	
	// Update is called once per frame
	void LateUpdate () {

		Vector3 newPosition = player.position;
		newPosition.y = transform.position.y;
		transform.position = newPosition;

		//want to roate according to maincamera...
		transform.rotation = Quaternion.Euler(90f, playerCamera.eulerAngles.y,0f);
		
	}
}
