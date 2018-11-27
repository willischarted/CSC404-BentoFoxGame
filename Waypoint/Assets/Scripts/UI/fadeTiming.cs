using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fadeTiming : MonoBehaviour {
    
    public string nextLevelName;
    public GameObject levelUp;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void whenFadeStart()
    {
        if (!(levelUp == null))
        {
            levelUp.SetActive(false);
        }
    }

    public void whenFadeComplete()
    {
        if (!(levelUp == null))
        {
            levelUp.SetActive(false);
            Time.timeScale = 1f;
            SceneManager.LoadScene(nextLevelName);
        }
    }

    public void whenFadeCompleteRestart()
    {
        if (!(levelUp == null))
        {
            levelUp.SetActive(false);
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void whenFadeCompleteStartMenu()
    {
        if (!(levelUp == null))
        {
            levelUp.SetActive(false);
            Time.timeScale = 1f;
            SceneManager.LoadScene("Start Menu");
            GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopDark();
            GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopLight();
        }
    }
}
