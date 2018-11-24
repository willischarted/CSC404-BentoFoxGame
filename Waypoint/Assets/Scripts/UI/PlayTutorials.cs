using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTutorials : MonoBehaviour {
    public GameObject levelStartPopup;
    private float timer;
    private bool done;
	// Use this for initialization
	void Start () {
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
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
            }
        }
	}
}
