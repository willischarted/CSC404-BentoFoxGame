using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterFireController : MonoBehaviour {

	
	ParticleSystem[] fireFX;


	// Use this for initialization
	void Start () {
		fireFX = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void turnOnFX() {
		foreach (ParticleSystem p in fireFX) {
			p.Play();
		}
	}

	public void turnOffFX() {
		foreach (ParticleSystem p in fireFX) {
			p.Stop();
		}
	}
}
