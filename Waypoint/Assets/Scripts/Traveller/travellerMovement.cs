using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class travellerMovement : MonoBehaviour
{

    public GameObject[] startAdjacent;
    public Vector3 offset;
    public float lampDistance = 1f;

    GameObject currentLight;
    GameObject latestLight;
    GameObject justVisited;
    GameObject targetLight;
    Animator anim;
    NavMeshAgent nav;
    travellerHealth travellerHealth;
    GameObject[] lamps;
    bool finishLevel;



    // Use this for initialization
    void Awake()
    {
        currentLight = null;
        latestLight = null;
        justVisited = null;
        targetLight = null;
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        travellerHealth = GetComponent<travellerHealth>();
        lamps = GameObject.FindGameObjectsWithTag("LampLight");
        finishLevel = false;
    }

    void Update()
    {
        if (!finishLevel){
            findCurrent();
            MoveToTarget();
            Animating();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exit"))
        {
            finishLevel = true;
            nav.SetDestination(other.gameObject.transform.position);
            loadNextLevel();
        }
    }

    public void loadNextLevel(){
        SceneManager.LoadScene(0);
    }

    void MoveToTarget(){
        GameObject[] adjacent;
        List<GameObject> possibleTargets = new List<GameObject>();
        if (currentLight == null){
            adjacent = startAdjacent;
        }else{
            adjacent = currentLight.GetComponentInParent<lightSourceController>().adjacentSources;
        }

        foreach (GameObject lamp in adjacent){
            int lightType = lamp.GetComponentInParent<lightSourceController>().getCurrentLightType();
            if (lightType == 1 || lightType == 2){
                possibleTargets.Add(lamp);
            }
        }
        GameObject[] targetLamps = possibleTargets.ToArray();

        if (targetLamps.Length > 0){
            //unless the latest is light up in the array, otherwise always go to the default one
            if (latestLight != null && (System.Array.IndexOf(targetLamps, latestLight) >= 0)){
                targetLight = latestLight;
            }else{
                targetLight = targetLamps[0]; 
            }

            nav.SetDestination(targetLight.transform.position - offset);
        }
    }

    void findCurrent(){
        foreach (GameObject lamp in lamps){
            if (Vector3.Distance(transform.position, lamp.transform.position) < lampDistance){
                currentLight = lamp;
            }
        }
    }

    public void findLatest(GameObject lightSource){
        latestLight = lightSource.gameObject;
    }

    void Animating()
    {

    }


}
