using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
 public class tutorialVideoPlayer : MonoBehaviour {
	RenderTexture rTexture;
 	private VideoPlayer vPlayer;
	private RawImage image;

	private playerControllerCopy pScript;
	private InteractionControllerCopy iScript;

	public GameObject nextTutorial;

 	void Awake(){
	
	}
	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
			Debug.Log("Could not find player");
		pScript = player.GetComponent<playerControllerCopy>();
		if (pScript == null)
			Debug.Log("Could not find pScript");
		
		iScript = player.GetComponentInChildren<InteractionControllerCopy>();
		if (iScript == null) {
			Debug.Log("Could not find the ineteractioncontroller");
		}

		pScript.setInTutorial(true);
		iScript.setInTutorial(true);
		
		
 		//player.
		//
		vPlayer = GetComponentInChildren<VideoPlayer>();
		if (vPlayer == null)
			Debug.Log("Could not find video player");
		image = GetComponentInChildren<RawImage>();
		if (image == null) 
			Debug.Log("Could not find image");
 		Debug.Log((int)vPlayer.clip.width);
		Debug.Log((int)vPlayer.clip.height);
		
		rTexture = new RenderTexture((int)vPlayer.clip.width, (int)vPlayer.clip.height, 0);
		rTexture = new RenderTexture((int)vPlayer.clip.width, (int)vPlayer.clip.height, 0);
		
		vPlayer.targetTexture = rTexture;
        image.texture = rTexture;
 
        Vector3 scale = image.transform.localScale;
 
        scale.y = vPlayer.clip.height / (float)vPlayer.clip.width * scale.y;
 
        image.transform.localScale = scale;
 		
	
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			vPlayer.Play();
		}	
		if (Input.GetKeyDown(KeyCode.S)) {
			vPlayer.Stop();
		}	

		if (Input.GetButtonDown("X") || Input.GetMouseButton(0) ) {
			//x out of th
			
		

			if (nextTutorial != null) {
				nextTutorial.SetActive(true);

			}
			else{
				pScript.setInTutorial(false);
				iScript.setInTutorial(false);
			}
			this.gameObject.SetActive(false);


			//call player -> not in tutorial anymore.

		}
	}
 }