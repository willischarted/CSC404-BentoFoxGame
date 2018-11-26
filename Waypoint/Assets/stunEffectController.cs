using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunEffectController : MonoBehaviour {

	ParticleSystem pSystem;

    private AudioSource stunSFX;

	// Use this for initialization
	void Start () {
		pSystem = GetComponent<ParticleSystem>();
		if (pSystem == null)
			Debug.Log("Could not find pSystem");

        stunSFX = GetComponent<AudioSource>();
        if (stunSFX == null)
            Debug.Log("Could not find audio");
		
	}
	
	public void playStunEffect() {
		pSystem.Play();
        stunSFX.Play();
	}

	public void stopStunEffect() {
		pSystem.Stop();
	}
}
