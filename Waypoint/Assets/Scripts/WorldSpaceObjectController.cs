using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceObjectController : MonoBehaviour {

	private GameObject player;


	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag("Player");
		if (player == null) {
			Debug.Log("Could not find player");
		}

		
		
	}
	
	// Update is called once per frame
	void Update () {

		//transform.rotation = Quaternion.LookRotation(player.transform.position);
		transform.rotation = Quaternion.LookRotation(Camera.main.transform.position);
		
	}


	// want to update the world text to whatever object we are currently near
	public void updateWorldObjectTransform(Vector3 newPosition) {
		transform.position = newPosition;

	}
}
