using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunEffectController : MonoBehaviour {

	ParticleSystem pSystem;

	// Use this for initialization
	void Start () {
		pSystem = GetComponent<ParticleSystem>();
		if (pSystem == null)
			Debug.Log("Could not find pSystem");
		
	}
	
	public void playStunEffect() {
		pSystem.Play();
	}

	public void stopStunEffect() {
		pSystem.Stop();
	}
}
