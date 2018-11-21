using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMapPathController : MonoBehaviour {

	public Material headsUp;
	public Material defaultLight;
	public Material travLight;

	public Material monsLight;

	

	LineRenderer path;

	 void Start() {
		 path = GetComponent<LineRenderer>();
		 if (path == null)
		 	Debug.Log("Could not find line renderer");
	}

	public void turnOnPath() {
		path.enabled = true;
	}

	public void turnOffPath() {
		path.enabled = false;
	}


	public void setPathColor(int col) {
		if (col == 0){
			path.material = headsUp;
		}
		else if (col == 1) {
			path.material = defaultLight;
		}
		else if (col == 2) {
			path.material = travLight;
		}
		else if (col == 3) {
			path.material = monsLight;
		}
 
	}
}
