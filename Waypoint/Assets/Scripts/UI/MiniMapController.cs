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

	public Canvas canvas;

	//public Camera miniMapCamera;

	
	void Start() {

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


		Vector3 travScreenPos = Camera.main.WorldToScreenPoint(trav.position);
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

			//find angle between center of screen and where arrow will be pointing
			float indicatorAngle = Mathf.Atan2(travScreenPos.y, travScreenPos.x);
			indicatorAngle -= 90 * Mathf.Deg2Rad;

			//tells you roghly where to look at
			float angleCos = Mathf.Cos(indicatorAngle);
			float angleSin = -Mathf.Sin(indicatorAngle);

			//now we must find out where the put the arrow
			travScreenPos = screenCenter + new Vector3(angleSin*150, angleCos * 150, 0);

			//slope formula
			float m = angleCos / angleSin;

			Vector3 screenBounds = screenCenter * 0.85f;

			//up/down
			if (angleCos >0) {
				travScreenPos = new Vector3(screenBounds.y/m, screenBounds.y, 0);
			}
			//down
			else {
				travScreenPos = new Vector3(-screenBounds.y/m, -screenBounds.y, 0);
			}
			// out of bounds, 
			if (travScreenPos.x > screenBounds.x) {
				travScreenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
			}
			else if (travScreenPos.x < -screenBounds.x) {
				travScreenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
			}

			travScreenPos += screenCenter;

		
			onscreenIcon.transform.position = travScreenPos;
			onscreenIcon.transform.rotation = Quaternion.Euler(0,0,indicatorAngle*Mathf.Rad2Deg);
			//onscreenIcon.transform.parent = canvas.transform;
			onscreenIcon.SetActive(true);
			//indicatorIcon.transform.localPosition = travScreenPos;
			//indicatorIcon.transform.localRotation = Quaternion.Euler(0,0,indicatorAngle*Mathf.Rad2Deg);
			//indicatorIcon.SetActive(true);
		}
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
