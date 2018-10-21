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


	public GameObject[] adjacentSources;

	public GameObject mapPath;


	// Use this for initialization
	void Start () {
		setMiniMapPaths();

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCurrentLightType(int type) {
		currentLightType = type;
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
