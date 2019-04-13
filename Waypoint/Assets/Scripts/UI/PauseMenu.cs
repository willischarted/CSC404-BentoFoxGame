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
    public GameObject pMResume;
    public GameObject pMRestart;
    public GameObject pMSM;
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
        resumeBtn.Select();
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
            if(SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                pMResume.SetActive(true);
                pMRestart.SetActive(false);
                pMSM.SetActive(false);
            }
        }
        if (optionNum == 1)
        {
            restartBtn.Select();
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                pMResume.SetActive(false);
                pMRestart.SetActive(true);
                pMSM.SetActive(false);
            }
        }
        if (optionNum == 2)
        {
            startMenuBtn.Select();
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                pMResume.SetActive(false);
                pMRestart.SetActive(false);
                pMSM.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("X"))
        {
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                pMResume.SetActive(false);
                pMRestart.SetActive(false);
                pMSM.SetActive(false);
            }
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
                restartBtn.Select();
                resume();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Circle")
            || Input.GetButtonUp("Start"))
        {
            restartBtn.Select();
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                pMResume.SetActive(false);
                pMRestart.SetActive(false);
                pMSM.SetActive(false);
            }
            resume();
        }
        
    }

    public void resume()
    {
        Debug.Log("resume");
        Time.timeScale = 1f;
        optionNum = 0;
        pauseMenu.SetActive(false);
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().lowpassOff();
    }

    public void restartLevel()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().lowpassOff();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void startMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().lowpassOff();
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopDark();
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopLight();
        SceneManager.LoadScene("Start Menu");
    }

    public void pause()
    {
        //Debug.Log("pause");
        pauseMenu.SetActive(true);
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().lowpassOn();
        Time.timeScale = 0f;
    }
}
