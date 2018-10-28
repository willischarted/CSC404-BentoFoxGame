using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceObjectController : MonoBehaviour {

	private GameObject player;


	public Sprite travellerLureIcon;
	public Sprite monsterLureIcon;
	public Image lureImage; 

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
		//transform.rotation = Quaternion.LookRotation(Camera.main.transform.position);
		//Quaternion lookRotation = Quaternion.LookRotation(Camera.main.transform.position);
		//Quaternion newRotation = new Quaternion(0f,lookRotation.eulerAngles.y,0f,0f);
		//transform.rotation = newRotation;
		//transform.rotation = lookRotation;
		transform.LookAt(Camera.main.transform.position);

		
	}


	// want to update the world text to whatever object we are currently near
	public void updateWorldObjectTransform(Vector3 newPosition) {
		transform.position = newPosition;

	}

	public void setTravelIcon(GameObject lamp) {
		GameObject trav = GameObject.FindGameObjectWithTag("Traveller");
		if (trav == null)
			Debug.Log("Could not find traveller");
		travellerMovement tScript = trav.GetComponent<travellerMovement>();
		if (tScript == null)
			Debug.Log("could not find travellerscript");
		
	

		if (tScript.currentLight == null) {

			if (checkAdjacent(tScript.startAdjacent, lamp)) {
				lureImage.sprite = travellerLureIcon;
				lureImage.enabled = true;
			}
			else {
				lureImage.enabled = false;
			}

		}

		else {
				
			GameObject travLamp = tScript.currentLight;
			lightSourceController lController = travLamp.GetComponent<lightSourceController>();
			if (lController == null) 
				Debug.Log("Could not find the lights controller script");
		
			if (checkAdjacent(lController.adjacentSources, lamp)) {
				lureImage.sprite = travellerLureIcon;
				lureImage.enabled = true;
			}
			else {
				lureImage.enabled = false;
			}
		}
		
	}

	bool checkAdjacent(GameObject[] lamps, GameObject Lamp) {
		foreach (GameObject g in lamps) {
			if (g.Equals(Lamp)) {
				return true;
			}
		}
		return false;
	}
	
}
