using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour {

	public Transform player;
	public Transform trav;

	public Transform playerCamera;

	public Sprite travellerIcon;

	
	void Start() {

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
