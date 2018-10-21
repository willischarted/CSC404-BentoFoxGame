using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour {

	public Transform player;

	public Transform playerCamera;


	
	// Update is called once per frame
	void LateUpdate () {

		Vector3 newPosition = player.position;
		newPosition.y = transform.position.y;
		transform.position = newPosition;

		//want to roate according to maincamera...
		transform.rotation = Quaternion.Euler(90f, playerCamera.eulerAngles.y,0f);
		
	}
}
