using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lantern_01_matswitcher : MonoBehaviour {
    public Material lit_mat;
    public Light lt;
    private GameObject lightObject;
    private lightSourceController lightScript;
    private MeshRenderer my_renderer;
    private Material default_mat;

    public GameObject litOutline;
   
    //public Material litOutline;
    private Material currentDefault;
    private Material currentLit;
    Material[] materials;

    private MeshRenderer meshRenderer;
   
    // Use this for initialization
    void Start () {
        lightObject = this.transform.parent.gameObject;
        lightScript = lightObject.GetComponent<lightSourceController>();
        my_renderer = GetComponent<MeshRenderer>();
        if (my_renderer != null)
        {
            default_mat = my_renderer.material;
        }

        //currentDefault
        //      currentLit = 
        materials = GetComponent<Renderer>().materials;
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.Log("Could nto find msh renderer");
        }
        // materials[1].SetFloat("_FirstOutlineWidth", 0);
        currentDefault = default_mat;
        currentLit = lit_mat;
        //my_renderer.ma
    
        Debug.Log(materials.Length);
        /*
        if (materials.Length <2)
        {
            Material[] newMaterials = new Material[2];
            newMaterials[0] = default_mat;
            newMaterials[1] = litOutline;
            GetComponent<Renderer>().materials = newMaterials;
            materials = GetComponent<Renderer>().materials;
            setDefault();
        }
        */
    
}

    // Update is called once per frame
    void Update () {
		if (lightScript.getCurrentLightType() == 0)
        {
            my_renderer.material = default_mat;
            //meshRenderer.enabled = false;
        } else
        {
            meshRenderer.enabled = true;

            my_renderer.material = lit_mat;
            
            my_renderer.material.SetColor("_EmissionColor", lt.color);

        }
	}

    public void sethighlight()
    {
        //materials[1].SetFloat("_FirstOutlineWidth", 0.05f);
        //currentDefault = litOutline;
        // currentLit = litOutline;
        litOutline.SetActive(true);
        meshRenderer.enabled = false;
    }

    public void setDefault()
    {

        //materials[1].SetFloat("_FirstOutlineWidth", 0);
        //currentDefault = default_mat;
        // currentLit = lit_mat;
        litOutline.SetActive(false);
        meshRenderer.enabled = true;
    }


}
