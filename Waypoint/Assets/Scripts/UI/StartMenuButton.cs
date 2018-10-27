using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuButton : MonoBehaviour {
    public void newGameBtn (){
        SceneManager.LoadScene(1);
    }
    public void exitBtn()
    {
        Application.Quit();
    }
}
