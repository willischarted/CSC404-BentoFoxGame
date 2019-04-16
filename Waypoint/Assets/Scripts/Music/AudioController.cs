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
        Debug.Log("get acive scene " + index);

        // BECAUSE THIS IS CALLED BEFORE THE NEXT SCENE LOADS WE HAVE
        // LOOK AT THE CURRENT SCENE TO DECIDE WHAT TO PLAY FOR THE NEXT ONE
        // We also have to note see level index key below (index unlike level are not
        // subsequent

        //the first two levels
        if (index <= 2)
        {
                PlayMusic(ThemeMusic[0]); 
        }

        // mon tut and lv 2.5 -> LV 2 COMES BEFORE LV '5' SO WE MUST USE INDEX 2 TO PREP FOR IT
        if (index ==  2 ||  index == 7)
        {
                PlayMusic(ThemeMusic[1]);
        }

        // trav tut and lv 3
        if (index == 3 || index == 8)
        {
                PlayMusic(ThemeMusic[2]);
        }


        //final three levels
        if (index == 4 || index ==9 || index == 5)
        {
                PlayMusic(ThemeMusic[3]);
        }

        //final level loop back to intro
        if (index == 6)
        {
            //if (music.isPlaying) //return;
            StopMusic();
            return;
        }

       

        /*LEVEL ORDER 
         * Level - INDEX
         * START - 0
         * 1 - 1
         * 2 - 2
         * 5 - 7
         * 2.5 - - 3 
         * 5.5 - 8
         * 3 - 4
         * 7 - 9
         * 3.5 - 5
         * 4 - 6
         * /
         */
    }
    public void checkPlayRestart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("get acive scene " + index);

        // BECAUSE THIS IS CALLED BEFORE THE NEXT SCENE LOADS WE HAVE
        // LOOK AT THE CURRENT SCENE TO DECIDE WHAT TO PLAY FOR THE NEXT ONE
        // We also have to note see level index key below (index unlike level are not
        // subsequent

        //the first two levels
        if (index <= 2)
        {
            PlayMusic(ThemeMusic[0]);
        }

        // mon tut and lv 2.5 -> LV 2 COMES BEFORE LV '5' SO WE MUST USE INDEX 2 TO PREP FOR IT
        if (index == 3 || index == 7)
        {
            PlayMusic(ThemeMusic[1]);
        }

        // trav tut and lv 3
        if (index == 4 || index == 8)
        {
            PlayMusic(ThemeMusic[2]);
        }


        //final three levels
        if (index == 6 || index == 9 || index == 5)
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

