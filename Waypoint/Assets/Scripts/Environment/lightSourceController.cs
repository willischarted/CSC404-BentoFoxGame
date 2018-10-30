using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightSourceController : MonoBehaviour {
	/*
		0 = turned off
		1 = default -> attracts both
		2 = traveller -> attracts traveller only
		3 = monster	-> attracts monster
	 */
	[SerializeField]
	private int currentLightType = 0;

	public float lightDuration;

	[SerializeField]
	private float timeRemaining;


	public GameObject[] adjacentSources;

	public GameObject mapPath;

	Light lampLight;

	float startIntensity;

	private playerControllerCopy pScript;


	// Use this for initialization
	void Start () {

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		pScript = player.GetComponent<playerControllerCopy>();

		if (pScript == null) {
			Debug.Log("pScript is nnull");
		}
		setMiniMapPaths();
		lampLight = GetComponentInChildren<Light>();
		if (lampLight == null)
			Debug.Log("Could not find light in child!");

		//startIntensity = lampLight.intensity;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (getCurrentLightType() > 0) {
			if(timeRemaining <= 0f) 
			{
				setLightOff();
				timeRemaining = lightDuration;
			}
			else 
			{
				timeRemaining -= Time.deltaTime;

				//reduce the visual visibility (not gameplay visibilty) of light 
				float lightRatio = timeRemaining / lightDuration;
				//Debug.Log(lightRatio);
				lampLight.intensity = startIntensity * lightRatio;

			}

		}
		
	}

	//function to immediately turn light off
	public void setLightOff() {
		//Debug.Log("Turn off light here");
		//Light lampLight = GetComponentInChildren<Light>();
		Renderer[] bulbs = GetComponentsInChildren<Renderer>();
		if (bulbs == null)
            Debug.Log("HHH");

		
        lampLight.intensity = 0;
	
		//Sound fx should play in here
        //audioSource.clip = offSoundEffect;
        //audioSource.Play();

		/* 
        if (currentLightType == 1 || currentLightType == 2){
            //tScript.setTarget(lightSource.transform.parent.transform, lampLight.intensity);
			GameObject trav = GameObject.FindGameObjectWithTag("Traveller");
			if (trav == null)
				Debug.Log("Could not find the traveller");

			travellerScript tScript= trav.GetComponent<travellerScript>();
			tScript.setTarget(transform.parent.transform, lampLight.intensity);

		}
		*/

		foreach (Renderer r in bulbs) {
			//Debug.Log(r.gameObject.name);//r.gameObject.name == "Bulb"
			if (string.Equals(r.gameObject.name, "Bulb")) {
				Material bulb = r.material;
        		bulb.DisableKeyword("_EMISSION");
			}
		}
	

		setCurrentLightType(0);

	}

	public void setCurrentLightType(int type) {
		
		
		if (type == 0) {

			Debug.Log("Returning some resource");
			float percentageReturn = timeRemaining / lightDuration;
			Debug.Log(percentageReturn);

			if (currentLightType == 1) {
				pScript.addResource(pScript.light1Value * percentageReturn);
			}
			else if (currentLightType == 2) {
				pScript.addResource(pScript.light1Value * percentageReturn);
			}
			else if (currentLightType == 3 ) {
				pScript.addResource(pScript.light1Value * percentageReturn);

			}
			currentLightType = type;
			return;
		}
		
		currentLightType = type;
		timeRemaining = lightDuration;

        if (lampLight){
            startIntensity = lampLight.intensity; 
			
        }

	
	}

	public int getCurrentLightType() {
		return currentLightType;
	}

	public GameObject[] getAdjacentSources() {
		return adjacentSources;
	}

	public void setMiniMapPaths() {
		foreach(GameObject g in adjacentSources) {
			GameObject m = Instantiate(mapPath, transform.position, Quaternion.identity);
			m.transform.parent = transform;
			LineRenderer lRenderer = m.GetComponent<LineRenderer>();
			if (lRenderer == null) {
				Debug.Log("Couldn't find linerederer");
			}

			Vector3[] positions = new Vector3[2];
			positions[0] = transform.position;
        	positions[1] = g.transform.position;
			lRenderer.positionCount = positions.Length;
			lRenderer.SetPositions(positions);
		}
	}
}
