using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathController : MonoBehaviour {

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
}
