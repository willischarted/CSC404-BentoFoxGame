using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAudioController : MonoBehaviour {
    public GameObject gameOver;
    public AudioSource gameOverDroneLoop;
    public AudioSource gameOverZinger;
    public AudioSource darkTheme;
    public AudioSource travellerTheme;
    bool clipsPlayed;
	// Use this for initialization
	void Start () {
        clipsPlayed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver.activeSelf == true && clipsPlayed == false)
        {
            Debug.Log("in here trying to play gameoveraudio");
            
            darkTheme.Stop();
            travellerTheme.Stop();
            gameOverZinger.PlayOneShot(gameOverZinger.clip);
            gameOverDroneLoop.PlayDelayed(0.5f);  
            clipsPlayed = true;
        }
	}
}
