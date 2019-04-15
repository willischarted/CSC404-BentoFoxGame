using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour {
    private AudioSource [] ThemeMusic;
    private static AudioController instance = null;
    public static AudioController Instance{
        get { return instance; }
    }

	// Use this for initialization
	void Awake() {
        ThemeMusic = new AudioSource[4];
        ThemeMusic[0] = transform.Find("Theme0").gameObject.GetComponent<AudioSource>();
        ThemeMusic[1] = transform.Find("Theme1").gameObject.GetComponent<AudioSource>();
        ThemeMusic[2] = transform.Find("Theme2").gameObject.GetComponent<AudioSource>();
        ThemeMusic[3] = transform.Find("Theme3").gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);    
        StopMusic();
 
    }
    public void checkPlay()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (index <= 2)
        {
                PlayMusic(ThemeMusic[0]);
        }

        if (index > 2 && index <= 4)
        {
                PlayMusic(ThemeMusic[1]);
        }

        if (index > 4 && index <= 6)
        {
                PlayMusic(ThemeMusic[2]);
        }

        if (index > 6 && index <= 9)
        {
                PlayMusic(ThemeMusic[3]);
        }

    }
    public void PlayMusic(AudioSource music)
    {
        if (music.isPlaying) return;
        StopMusic();
        music.Play();
    }

    public void StopMusic()
    {
        for (int i = 0; i < 4; i++){
            ThemeMusic[i].Stop();
        }
        
    }


    public void lowpassOn(){
        for (int i = 0; i < 4; i++)
        {
            ThemeMusic[i].GetComponentInParent<AudioLowPassFilter>().enabled = true;
        }
    }

    public void lowpassOff()
    {
        for (int i = 0; i < 4; i++)
        {
            ThemeMusic[i].GetComponentInParent<AudioLowPassFilter>().enabled = false;
        }
    }
}

