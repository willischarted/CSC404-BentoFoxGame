using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class traveller_healthmaterial : MonoBehaviour {
    public Material highMat;
    public Material lowMat;
    public Light highLight;
    public Light lowLight;
    private MeshRenderer my_renderer;

    // Use this for initialization
    void Start () {
        my_renderer = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
