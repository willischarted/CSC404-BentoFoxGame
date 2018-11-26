using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class travellerMovement : MonoBehaviour
{
    public GameObject startingPoint;
    private Vector3 startingPointTransform;
    
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
    bool closeToExit;
    public bool beatLevel = false;


    // We want to use different states

    private bool isScared;
    



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
        closeToExit = false;

        isScared = false;

        startingPointTransform = new Vector3(startingPoint.transform.position.x,
                                            startingPoint.transform.position.y, 
                                            startingPoint.transform.position.z);
    }

    void Update()

    {
        /* 
        if (Input.GetKeyDown(KeyCode.C)) {
            Debug.Log(targetLight);
            Debug.Log(currentLight);
        }
        */
        if (!closeToExit){

            if (targetLight != null && targetLight != currentLight) {
                //Debug.Log("running first");
                //check to see if it turned off
                lightSourceController lScript = targetLight.GetComponent<lightSourceController>();
                // the targte light was turned off before we got there
                if (lScript.getCurrentLightType() == 0) {
                    //go back to the current light -> have not run find current yet
                    if (currentLight == null)
                        MoveToTarget(startingPointTransform);
                    else
                        MoveToTarget(currentLight);
                    return;
                }

                
            }
            FindCurrent();

            //what if the current light we are at is turned off
            if (currentLight != null) {
                 
                lightSourceController currentScript = currentLight.GetComponent<lightSourceController>();
                if(currentScript.getCurrentLightType() == 0) {
                    //Debug.Log("running second");
                    //go back to any adjacent ones
                    MoveBack();
                    //if not just stay there.
                    return;
                }
            }


            //FindJustVisited();
            MoveToTarget();
            //Animating();
        }
        if (Vector3.Distance(exitPoint.position, transform.position) < 0.3)
        {
            beatLevel = true;
            loadNextLevel();
        }




        float distRemaining = nav.remainingDistance; 

        

      // if (distRemaining!= Mathf.Infinity && nav.pathStatus == NavMeshPathStatus.PathComplete && nav.remainingDistance == 0)
       // if (nav.pathStatus == NavMeshPathStatus.PathComplete)
       if (targetLight != null && Vector3.Distance(transform.position, targetLight.transform.position) < lampDistance && anim.GetBool("isMoving"))
        {
            anim.SetBool("isMoving", false);
        }
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exit"))
        {
            closeToExit = true;
            nav.SetDestination(exitPoint.position);
            anim.SetTrigger("isExit");

        }
        /* 
        else if (other.gameObject.CompareTag("Monster")){
            if (other.GetType() == typeof(CapsuleCollider)){
                travellerHealth.TakeBasicDamage(10);
            }     
        }
        */
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Monster")){
            if (other.GetType() == typeof(CapsuleCollider)){
                EnemyMovement monScript = other.gameObject.GetComponent<EnemyMovement>();
                if (monScript == null)
                    Debug.Log("Could not find the monScript");
                //if (monScript.startAttack())
                  //  travellerHealth.TakeBasicDamage(10);
                monScript.startAttack();
            }     
        }
    }

    public void loadNextLevel(){
        //SceneManager.LoadScene(0);
        endlvl.levelComplete();
    }

    private void MoveToTarget(GameObject g) {
        targetLight = g;
        nav.SetDestination(targetLight.transform.position - offset);
        anim.SetBool("isMoving", true);
    }

     private void MoveToTarget(Vector3 t) {
        
        nav.SetDestination(t - offset);
        anim.SetBool("isMoving", true);
    }

    private void MoveToTarget(){
       // Debug.Log("move to target normal");
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
        // remove any past nodes from possible move list
        foreach (GameObject g in history) {
            if (possibleTargets.Contains(g)) {
               // Debug.Log("removing " + g);
                possibleTargets.Remove(g);
            }
        }


     
        if (possibleTargets.Count > 0){
           
           // foreach(GameObject g in possibleTargets)
           //      Debug.Log(g);
          
            //always go to the latest light, if possible
            if (latestLight != null && (possibleTargets.Contains(latestLight) && !history.Contains(latestLight))){ //not one we have visited
                targetLight = latestLight;
            }

            /* 
            //if the justVisited in the targetLamps array, ignore it
            else if (possibleTargets.Contains(justVisited)){
                possibleTargets.Remove(justVisited);
            }
            */

           
            //else go to the default one
            else{
                // after pruning if we still have a light it can go to
               // if (possibleTargets.Count > 0) {
                    targetLight = possibleTargets[0];
               // }
            }

            nav.SetDestination(targetLight.transform.position - offset);
            anim.SetBool("isMoving", true);

            if (startingPoint != null)
               Destroy(startingPoint);
        }
    }

     private void MoveBack(){
        GameObject[] adjacent;
        List<GameObject> possibleTargets = new List<GameObject>();
        adjacent = currentLight.GetComponentInParent<lightSourceController>().adjacentSources;
        

        foreach (GameObject lamp in adjacent){
            int lightType = lamp.GetComponentInParent<lightSourceController>().getCurrentLightType();
            if (lightType == 1 || lightType == 2){
                possibleTargets.Add(lamp);
            }
        }

     
        if (possibleTargets.Count > 0){
          
            //always go to the latest light, if possible
            if (latestLight != null && (possibleTargets.Contains(latestLight) && !history.Contains(latestLight))){ //not one we have visited
                targetLight = latestLight;
            }

 
            else{

                targetLight = possibleTargets[0];

            }

            nav.SetDestination(targetLight.transform.position - offset);
            anim.SetBool("isMoving", true);

            if (startingPoint != null)
               Destroy(startingPoint);
        }
    }

    private void FindCurrent(){
        foreach (GameObject lamp in lamps){
            if (Vector3.Distance(transform.position, lamp.transform.position) < lampDistance){
                currentLight = lamp;

                if (!history.Contains(currentLight))  {
                    history.Add(currentLight);
                }

                /* 
                if (history.Count == 0){
                    history.Add(currentLight);
                } else if (history[history.Count - 1] != currentLight){
                    history.Add(currentLight);
                }
                */
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

    public void removeFromHistory(GameObject g) {
        if (history.Contains(g)) {
            history.Remove(g);
        }
    }



}
