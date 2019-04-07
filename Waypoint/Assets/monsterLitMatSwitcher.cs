using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterLitMatSwitcher : MonoBehaviour {

    public Material litMaterial;
    [SerializeField]
    private Material defaultMaterial;
    private SkinnedMeshRenderer smRenderer;
    // Use this for initialization

    void Start () {
        smRenderer = GetComponent<SkinnedMeshRenderer>();
        defaultMaterial = smRenderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setLitMat()
    {
        smRenderer.material = litMaterial;
    }

    public void resetMat()
    {
        smRenderer.material = defaultMaterial;
    }
}
