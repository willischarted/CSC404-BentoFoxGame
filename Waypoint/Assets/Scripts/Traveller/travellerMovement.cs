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
    public GameObject currentLight;
    public Transform exitPoint;
    public GameObject justVisited;
    public GameObject levelUpMenu;
    private EndLevel endlvl;

    GameObject latestLight;
    GameObject targetLight;
    Animator anim;
    NavMeshAgent nav;
    travellerHealth travellerHealth;
    GameObject[] lamps;
    List<GameObject> history = new List<GameObject>();
    bool finishLevel;



    // Use this for initialization
    void Awake()
    {
        endlvl = levelUpMenu.GetComponent<EndLevel>();
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
            FindCurrent();
            FindJustVisited();
            MoveToTarget();
            Animating();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exit"))
        {
            finishLevel = true;
            nav.SetDestination(exitPoint.position);
            loadNextLevel();
        }
        else if (other.gameObject.CompareTag("Monster")){
            if (other.GetType() == typeof(CapsuleCollider)){
                travellerHealth.TakeBasicDamage(5);
            }     
        }
    }

    public void loadNextLevel(){
        //SceneManager.LoadScene(0);
        endlvl.levelComplete();
    }

    private void MoveToTarget(){
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

        if (possibleTargets.Count > 0){
            //always go to the latest light
            if (latestLight != null && (possibleTargets.Contains(latestLight))){
                targetLight = latestLight;
            }
            //if the justVisited in the targetLamps array, ignore it
            else if (possibleTargets.Contains(justVisited)){
                possibleTargets.Remove(justVisited);
            }
            //else go to the default one
            else{
                targetLight = possibleTargets[0];
            }

            nav.SetDestination(targetLight.transform.position - offset);
        }
    }

    private void FindCurrent(){
        foreach (GameObject lamp in lamps){
            if (Vector3.Distance(transform.position, lamp.transform.position) < lampDistance){
                currentLight = lamp;
                if (history.Count == 0){
                    history.Add(currentLight);
                } else if (history[history.Count - 1] != currentLight){
                    history.Add(currentLight);
                }
            }
        }
    }

    private void FindJustVisited(){
        //the second-last in the history list
        if (history.Count >= 2)
        {
            justVisited = history[history.Count - 2];
        }
    }


    public void findLatest(GameObject lightSource){
        latestLight = lightSource.gameObject;
    }

    void Animating()
    {

    }


}
