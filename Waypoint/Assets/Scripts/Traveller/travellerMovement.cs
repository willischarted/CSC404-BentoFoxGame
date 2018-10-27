using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEditor;

public class travellerMovement : MonoBehaviour
{

    public GameObject[] startAdjacent;
    public Vector3 offset;

    //Transform startPoint;
    GameObject currentLight;
    GameObject latestLight;
    GameObject targetLight;
    Animator anim;
    NavMeshAgent nav;
    travellerHealth travellerHealth;


    // Use this for initialization
    void Awake()
    {
        //startPoint = GameObject.FindGameObjectWithTag("StartArea").transform;
        currentLight = null;
        latestLight = null;
        targetLight = null;
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        travellerHealth = GetComponent<travellerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] targetLamps = findPossible();
        if (targetLamps.Length > 0){
            moveToTarget(targetLamps);
        }
        Animating();

    }
      
    GameObject[] findPossible(){
        GameObject[] adjacent;
        List<GameObject> possibleTargets = new List<GameObject>();
        if (currentLight == null){//Initial State
            adjacent = startAdjacent;
        }else{
            adjacent = currentLight.GetComponentInParent<lightSourceController>().adjacentSources;
        }
        foreach (GameObject lamp in adjacent){
            int lightType = lamp.GetComponentInParent<lightSourceController>().getCurrentLightType();
            if (lightType == 1 || lightType == 2)
            {
                possibleTargets.Add(lamp);
            }
        }
        GameObject[] targetLamps = possibleTargets.ToArray();
        return targetLamps;
    }

    void moveToTarget(GameObject[] targetLamps){
        if (currentLight == null){
            int ran = Random.Range(0, targetLamps.Length);
            targetLight = targetLamps[ran];
            nav.SetDestination(targetLight.transform.position);
        }
    }

    void Animating(){
        
    }
}
