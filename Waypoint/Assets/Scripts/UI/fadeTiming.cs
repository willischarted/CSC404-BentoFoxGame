﻿using System.Collections;
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
        levelUp.SetActive(false);
    }

    public void whenFadeComplete()
    {
        levelUp.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelName);
    }
}
