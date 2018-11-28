using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayTutorials : MonoBehaviour
{
    public GameObject levelStartPopup;
    private float timer;
    private bool done;
    private Button startButton;
    private Button exitButton;
    private bool started;
    // Use this for initialization
    void Start()
    {
        timer = 0f;
        started = false;
        if (SceneManager.GetActiveScene().name == "Start Menu")
        {
            startButton = levelStartPopup.transform.Find("NewGameButton").GetComponent<Button>();
            exitButton = levelStartPopup.transform.Find("ExitButton").GetComponent<Button>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (levelStartPopup != null && done != true)
        {
            if (timer < 1f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                levelStartPopup.SetActive(true);
                done = true;
                if (startButton != null && exitButton != null)
                {
                    exitButton.Select();
                    startButton.Select();
                }
            }
        }
    }
}
