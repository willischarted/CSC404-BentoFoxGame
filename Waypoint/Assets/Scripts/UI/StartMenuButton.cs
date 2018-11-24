using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuButton : MonoBehaviour
{

    //Button Array
    //place array
    public GameObject canvas;
    private int[] options;
    private int optionNum;
    private bool xAxisInUse = false;
    private bool yAxisInUse = false;
    private float gameSpeed;
    private Button newGameBtn;
    private Button exitBtn;
    public AudioSource MenuTheme;
    public AudioSource newGameSound;
    public Transform fadeScreen;
    private Animator fade;
    //private Transform selector;


    private void Start()
    {
        options = new int[2];
        optionNum = 0;
        newGameBtn = canvas.transform.GetChild(1).GetComponent<Button>();
        exitBtn = canvas.transform.GetChild(2).GetComponent<Button>();
        fade = fadeScreen.GetChild(0).GetComponent<Animator>();
        //nextLevelBtn = transform.GetChild(0).transform.GetChild(3).GetComponent<Button>();
    }

    private void Update()
    {
//        Debug.Log(optionNum);
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

        if (yValue == 0) {
            yAxisInUse = false;

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))// || (yAxisInUse && yValue == 1)

        {
            Debug.Log("Incrementing");
            if (optionNum != 1)
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
                optionNum = 1;
            }
        }

        if (optionNum == 0)
        {
            newGameBtn.Select();
        }
        if (optionNum == 1)
        {
            exitBtn.Select();
        }
        /* uncomment when the next Level button exists
        if (optionNum == 2)
        {
            nextLevelBtn.Select();
        }*/

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("X"))
        {
            if (optionNum == 0)
            {
                startGame();
            }
            else if (optionNum == 1)
            {
                Quit();
            }
            /*   else if (optionNum == 2)
               {
                   nextLevel();
               }*/
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void startGame()
    {
        MenuTheme.Stop();
        StartCoroutine(loadLevel());
    }
    IEnumerator loadLevel()
    {
        newGameSound.PlayOneShot(newGameSound.clip);
        GameController.level = 1;
        Time.timeScale = 1f;
        fade.SetTrigger("fadeOut");
        canvas.SetActive(false);
        yield return new WaitForSeconds(2.0f);        
        SceneManager.LoadScene("Level2");
        GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioController>().checkPlay();

    }
}