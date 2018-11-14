using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour {
    private AudioSource lightTheme;
    private AudioSource darkTheme;
    private static AudioController instance = null;
    public static AudioController Instance{
        get { return instance; }
    }

	// Use this for initialization
	void Awake() {
        lightTheme = transform.Find("lightTheme").gameObject.GetComponent<AudioSource>();
        darkTheme = transform.Find("darkTheme").gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 1){
            PlayLight();
            PlayDark();
        }
    }
	
    public void PlayLight(){
        if (lightTheme.isPlaying) return;
        lightTheme.Play();
    }

    public void PlayDark()
    {
        if (darkTheme.isPlaying) return;
        darkTheme.Play();
    }

    public void StopLight(){
        lightTheme.Stop();
    }

    public void StopDark()
    {
        darkTheme.Stop();
    }

    public void lowpassOn(){
        lightTheme.GetComponentInParent<AudioLowPassFilter>().enabled = true;
        darkTheme.GetComponentInParent<AudioLowPassFilter>().enabled = true;
    }

    public void lowpassOff()
    {
        lightTheme.GetComponentInParent<AudioLowPassFilter>().enabled = false;
        darkTheme.GetComponentInParent<AudioLowPassFilter>().enabled = false;
    }
}

