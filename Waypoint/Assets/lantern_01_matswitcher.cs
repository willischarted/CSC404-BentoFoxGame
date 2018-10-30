﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lantern_01_matswitcher : MonoBehaviour {
    public Material lit_mat;
    private GameObject lightObject;
    private lightSourceController lightScript;
    private MeshRenderer my_renderer;
    private Material default_mat;

    // Use this for initialization
    void Start () {
        lightObject = this.transform.parent.gameObject;
        lightScript = lightObject.GetComponent<lightSourceController>();
        my_renderer = GetComponent<MeshRenderer>();
        if (my_renderer != null)
        {
            default_mat = my_renderer.material;
        }
    }

    // Update is called once per frame
    void Update () {
		if (lightScript.getCurrentLightType() == 0)
        {
            my_renderer.material = default_mat;
        } else
        {
            my_renderer.material = lit_mat;
        }
	}
}
