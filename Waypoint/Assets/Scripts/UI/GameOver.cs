using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    //Button Array
    //place array
    public GameObject gameOver;
    public GameObject gORestart;
    public GameObject gOSM;
    private int[] options;
    private int optionNum;
    private bool xAxisInUse = false;
    private bool yAxisInUse = false;
    private float gameSpeed;
    private Button restartBtn;
    private Button startMenuBtn;
    //private Transform selector;


    private void Start()
    {
        gameOver.SetActive(false);
        options = new int[2];
        optionNum = 0;
        restartBtn = transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        startMenuBtn = transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
    }

    private void Update()
    {
        //Debug.Log(optionNum);
        float yValue = Input.GetAxis("DPadY");
        if (yValue != 0f)
        {
            if (!yAxisInUse)
            {
                Debug.Log("pressed dpad");
                if (yValue == 1f) {
                    yAxisInUse = true;
                    if (optionNum != 1)
                    {
                        optionNum += 1;
                    }
                else
                {
                    optionNum = 0;
                }
            }
                else if (yValue == -1f)
                {
                    yAxisInUse = true;
                    if (optionNum != 0)
                    {
                        optionNum -= 1;
                    }
                    else
                    {
                        optionNum = 1;
                    }
                }
            }   
           
        }
        
        if (yValue == 0)
            yAxisInUse = false;

        if (Input.GetKeyDown(KeyCode.DownArrow) ) //|| (yAxisInUse && yValue == 1)
        {
            if (optionNum != 1)
            {
                optionNum += 1;
            }
            else
            {
                optionNum = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) ) //|| (yAxisInUse && yValue != 1)
        {
            if (optionNum != 0)
            {
                optionNum -= 1;
            }
            else
            {
                optionNum = 1;
            }
        }

        if (optionNum == 0)
        {
            restartBtn.Select();
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                gORestart.SetActive(true);
                gOSM.SetActive(false);
            }
        }
        if (optionNum == 1)
        {
            startMenuBtn.Select();
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                gORestart.SetActive(false);
                gOSM.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("X"))
        {
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                gORestart.SetActive(false);
                gOSM.SetActive(false);
            }
            if (optionNum == 0)
            {
                restartLevel();
            }
            else if (optionNum == 1)
            {
                startMenu();
            }
        }
    }

    public void gameOverr()
    {
        
        Time.timeScale = 0f;
        gameOver.SetActive(true);
    }

    public void restartLevel()
    {
        optionNum = 4;
        Time.timeScale = 1f;
        gameOver.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().checkPlayRestart();


    }

    public void startMenu()
    {
        optionNum = 4;
        Time.timeScale = 1f;
        gameOver.SetActive(false);
        SceneManager.LoadScene("Start Menu");
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopMusic();
    }
}



