using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    //Button Array
    //place array
    public GameObject pauseMenu;
    private int[] options;
    private int optionNum;
    private bool xAxisInUse = false;
    private bool yAxisInUse = false;
    private float gameSpeed;
    private Button resumeBtn;
    private Button restartBtn;
    private Button startMenuBtn;
    //private Transform selector;


    private void Start()
    {
        pauseMenu.SetActive(false);
        options = new int[3];
        optionNum = 0;
        resumeBtn = transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        restartBtn = transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        startMenuBtn = transform.GetChild(0).transform.GetChild(3).GetComponent<Button>();
    }

    private void Update()
    {
        Debug.Log(optionNum);
        float yValue = Input.GetAxis("DPadY");
          if (yValue != 0f)
        {
            if (!yAxisInUse)
            {
                if (yValue == 1f) {
                    Debug.Log("Pressed dpad down");
                    yAxisInUse = true;

                    if (optionNum != 2)
                    {
                        optionNum += 1;
                    }
                    else
                    {
                    optionNum = 0;
                    }
                }
                else if (yValue == -1f) {
                    Debug.Log("Pressed dpad up");
                    yAxisInUse = true;
                    if (optionNum != 0)
                    {
                        optionNum -= 1;
                    }
                    else
                    {
                        optionNum = 2;
                    }

                }
            }
        }

        if (yValue == 0)
            yAxisInUse = false;

        if (Input.GetKeyDown(KeyCode.DownArrow)) // || (yAxisInUse && yValue == 1)
        {
            if (optionNum != 2)
            {
                optionNum += 1;
            }
            else
            {
                optionNum = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))// || (yAxisInUse && yValue != 1)
        {
            if (optionNum != 0)
            {
                optionNum -= 1;
            }
            else
            {
                optionNum = 2;
            }
        }

        if(optionNum  == 0)
        {
            resumeBtn.Select();
        }
        if (optionNum == 1)
        {
            restartBtn.Select();
        }
        if (optionNum == 2)
        {
            startMenuBtn.Select();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("X"))
        {
            if (optionNum == 1)
            {
                Debug.Log("restart");
                restartLevel();
            }
            else if (optionNum == 2)
            {
                startMenu();
            }
            else if (optionNum == 0)
            {
                resume();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Circle")
            || Input.GetButtonUp("Start"))
        {
            resume();
        }
        
    }

    public void resume()
    {
        Debug.Log("resume");
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void restartLevel()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void startMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("Start Menu");
    }

    public void pause()
    {
        //Debug.Log("pause");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

}
