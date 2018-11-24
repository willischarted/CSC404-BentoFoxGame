using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{

    //Button Array
    //place array
    public GameObject levelUp;
    private int optionNum;
    private bool xAxisInUse = false;
    private bool yAxisInUse = false;
    private float gameSpeed;
    private Button restartBtn;
    private Button startMenuBtn;
    private Button nextLevelBtn;
    public Transform fadeScreen;
    private Animator fade;
    public GameObject canvas;
    //private Transform selector;


    private void Start()
    {
        levelUp.SetActive(false);
        optionNum = 0;
        restartBtn = levelUp.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        startMenuBtn = levelUp.transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        nextLevelBtn = levelUp.transform.GetChild(0).transform.GetChild(3).GetComponent<Button>();
        fade = fadeScreen.GetChild(0).GetComponent<Animator>();
        fade.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        float yValue = Input.GetAxis("DPadY");
        if (yValue != 0f)
        {
            if (!yAxisInUse)
            {
                if (yValue == 1f) {
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




        if (Input.GetKeyDown(KeyCode.DownArrow))//|| (yAxisInUse && yValue == 1)
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
        if (Input.GetKeyDown(KeyCode.UpArrow) )//|| (yAxisInUse && yValue != 1)
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

        if (optionNum == 0)
        {
            nextLevelBtn.Select();
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
                restartLevel();
            }
            else if (optionNum == 2)
            {
                startMenu();
            }
            else if (optionNum == 0)
            {
                nextLevel();
            }
        }
    }

    public void levelComplete()
    {
        Time.timeScale = 0f;
        levelUp.SetActive(true);
    }

    public void restartLevel()
    {
        Time.timeScale = 1f;
        levelUp.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void startMenu()
    {
        Time.timeScale = 1f;
        levelUp.SetActive(false);
        SceneManager.LoadScene("Start Menu");
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopDark();
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopLight();
    }
    public void nextLevel()
    {
        /*     if (GameController.level == 4)
             {

                 GameController.level = 1;
                 Time.timeScale = 1f;
                 levelUp.SetActive(false);
                 SceneManager.LoadScene("Start Menu");
                 GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopDark();
                 GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopLight();
             }
             else
             {
                 string currScene = SceneManager.GetActiveScene().name;
                 Debug.Log("currscene: " + currScene);
                 if (currScene == "Level3.5")
                 {
                     GameController.level = 4;
                     Time.timeScale = 1f;
                     levelUp.SetActive(false);
                     SceneManager.LoadScene("Level" + GameController.level);
                 }
                 else if (currScene == "Level3")
                 {
                     Time.timeScale = 1f;
                     levelUp.SetActive(false);
                     SceneManager.LoadScene("Level3.5");
                 }
                 if (currScene == "Level2.5")
                 {
                     Time.timeScale = 1f;
                     levelUp.SetActive(false);
                     GameController.level = 3;
                     SceneManager.LoadScene("Level" + GameController.level);
                 }
                 else if (currScene == "Level2")
                 {
                     Time.timeScale = 1f;
                     levelUp.SetActive(false);
                     SceneManager.LoadScene("Level2.5");
                 }
                 else
                 {
                     GameController.level++;
                     Debug.Log(GameController.level);
                     Time.timeScale = 1f;
                     levelUp.SetActive(false);
                     //SceneManager.LoadScene("Level" + GameController.level);
                     SceneManager.LoadScene(nextLevelName);
                 }
             }*/
        canvas.SetActive(false);
        fade.SetTrigger("fadeOut");
    }
}