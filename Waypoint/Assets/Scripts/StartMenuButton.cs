﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButton : MonoBehaviour {
    public void newGameBtn (string newGameLevel){
        SceneManager.LoadScene(newGameLevel);
    }
    public void exitBtn()
    {
        Application.Quit();
    }
}