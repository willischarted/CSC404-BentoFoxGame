using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAudioController : MonoBehaviour {
    public GameObject gameOver;
    public AudioSource gameOverDroneLoop;
    public AudioSource gameOverTopLoop;
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
            //AudioListener.pause = true;
            // darkTheme.Stop();
            // travellerTheme.Stop();


            GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopMusic();
            gameOverZinger.PlayOneShot(gameOverZinger.clip);
            gameOverDroneLoop.PlayDelayed(0.5f);
            gameOverTopLoop.PlayDelayed(0.5f);
            clipsPlayed = true;
        }
	}
}
