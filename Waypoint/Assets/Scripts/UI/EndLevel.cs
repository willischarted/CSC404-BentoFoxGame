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
    public GameObject lUNext;
    public GameObject lURestart;
    public GameObject lUSM;
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
    public AudioSource zinger;
    public AudioSource buttonSound;
    private float timer;
    private bool zingerPlayed;
    //private Transform selector;


    private void Start()
    {
        levelUp.SetActive(false);
        optionNum = 0;
        timer = 0f;
        restartBtn = levelUp.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        startMenuBtn = levelUp.transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        nextLevelBtn = levelUp.transform.GetChild(0).transform.GetChild(3).GetComponent<Button>();
        fade = fadeScreen.GetChild(0).GetComponent<Animator>();
        fade.updateMode = AnimatorUpdateMode.UnscaledTime;
        zinger = transform.Find("Zinger").transform.GetComponent<AudioSource>();
        buttonSound = transform.Find("ButtonSound").transform.GetComponent<AudioSource>();
        zingerPlayed = false;
    }

    private void Update()
    {
        if(zinger.volume > 0.3)
        {
            zinger.volume -= 0.003f;
        }
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
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                lUNext.SetActive(true);
                lURestart.SetActive(false);
                lUSM.SetActive(false);
            }
        }
        if (optionNum == 1)
        {
            restartBtn.Select();
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                lUNext.SetActive(false);
                lURestart.SetActive(true);
                lUSM.SetActive(false);
            }
        }
        if (optionNum == 2)
        {
            startMenuBtn.Select();
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                lUNext.SetActive(false);
                lURestart.SetActive(false);
                lUSM.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("X"))
        {
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
            {
                lUNext.SetActive(false);
                lURestart.SetActive(false);
                lUSM.SetActive(false);
            }
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
        levelUp.SetActive(true);
        zinger.enabled = true;
        if (!zinger.isPlaying && !zingerPlayed)
        {
            zingerPlayed = true;
            zinger.PlayOneShot(zinger.clip);            
        }
        Time.timeScale = 0f;
    }

    public void restartLevel()
    {
        optionNum = 4;
        buttonSound.enabled = true;
        buttonSound.PlayOneShot(buttonSound.clip);
        /*  Time.timeScale = 1f;
          levelUp.SetActive(false);
          SceneManager.LoadScene(SceneManager.GetActiveScene().name);*/
        canvas.SetActive(false);
        fade.SetTrigger("fadeOutRestart");
    }

    public void startMenu()
    {
        optionNum = 4;
        buttonSound.enabled = true;
        buttonSound.PlayOneShot(buttonSound.clip);
        Time.timeScale = 1f;
        levelUp.SetActive(false);
        SceneManager.LoadScene("Start Menu");
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().StopMusic();
        /*canvas.SetActive(false);
        fade.SetTrigger("fadeOutStartMenu");*/
    }

    public void nextLevel()
    {
        optionNum = 4;
        buttonSound.enabled = true;
        buttonSound.PlayOneShot(buttonSound.clip);
        canvas.SetActive(false);
        fade.SetTrigger("fadeOut");
    }
}