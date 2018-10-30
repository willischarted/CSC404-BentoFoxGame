using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
 public class tutorialVideoPlayer : MonoBehaviour {
	RenderTexture rTexture;
 	private VideoPlayer player;
	private RawImage image;
 	void Awake(){
	
	}
	// Use this for initialization
	void Start () {
		
		
 		//player.
		//
		player = GetComponent<VideoPlayer>();
		if (player == null)
			Debug.Log("Could not find video player");
		image = GetComponent<RawImage>();
		if (image == null) 
			Debug.Log("Could not find image");
 		Debug.Log((int)player.clip.width);
		Debug.Log((int)player.clip.height);
		
		rTexture = new RenderTexture((int)player.clip.width, (int)player.clip.height, 0);
		rTexture = new RenderTexture((int)player.clip.width, (int)player.clip.height, 0);
		
		player.targetTexture = rTexture;
        image.texture = rTexture;
 
        Vector3 scale = image.transform.localScale;
 
        scale.y = player.clip.height / (float)player.clip.width * scale.y;
 
        image.transform.localScale = scale;
 		
	
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			player.Play();
		}	
		if (Input.GetKeyDown(KeyCode.S)) {
			player.Stop();
		}	
	}
 }