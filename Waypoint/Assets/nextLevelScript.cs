using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevelScript : MonoBehaviour {

    public void nextLevelBtn()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current + 1, LoadSceneMode.Single);
    }
}
