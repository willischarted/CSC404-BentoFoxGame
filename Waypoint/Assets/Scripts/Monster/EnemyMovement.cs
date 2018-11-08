using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{

    Transform traveller;
    Animator monsterAnim;
    NavMeshAgent nav;
    public SphereCollider col;
    float fieldOfViewAngle = 120f;
    Vector3 currentTarget;
    public float speed = 10f;
    public float maxRotation = 45f;
    float timer = 0f;
    Vector3 direction;
    Vector3 upward;

    public float MAX_LD;
    public GameObject lastVisited;
    private bool moving;
    private bool movingToLamp;
    public Vector3 roamCenterPoint;
    public float maxRoamDistance;
    public float lampDistance = 5f;
    GameObject currentLamp;
    GameObject[] lamps;
    public GameObject targetLamp;

    private void Awake()
    {
        moving = false;
        traveller = GameObject.FindGameObjectsWithTag("Traveller")[0].transform;
        nav = GetComponent<NavMeshAgent>();
        monsterAnim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
        currentTarget = transform.position;
        timer = 0f;
        upward.Set(0f, 0.2f, 0f);
        currentLamp = null;
        lamps = GameObject.FindGameObjectsWithTag("LampLight");
        List<GameObject> validLamps = new List<GameObject>();
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) <= MAX_LD && Vector3.Distance(roamCenterPoint, lamp.transform.position) <= maxRoamDistance)
            {
                validLamps.Add(lamp);
            }
        }
        lamps = validLamps.ToArray();
    }

    void OnTriggerStay(Collider other)
    {
        if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {
            Debug.Log("inhere");
            return;
        }
        else
        {
            if (other.gameObject.transform == traveller)
            {
                direction = other.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < fieldOfViewAngle * 0.5f)
                {
                    monsterAnim.SetTrigger("isAlerted");
                }
            }


        }

    }


    void Update()
    {
        if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {
            Debug.Log("Stunned");
            nav.SetDestination(transform.position);

           

            timer += Time.deltaTime;
            if (timer > 5)
            {
                monsterAnim.SetTrigger("recovered");
                timer = 0f;
                movingToLamp = false;
                targetLamp = null;
            }
        }
        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            Debug.Log("Chase");
            currentTarget = traveller.position;
            nav.SetDestination(currentTarget);
            if (Vector3.Distance(roamCenterPoint, traveller.position) >= maxRoamDistance)
            {
                monsterAnim.SetTrigger("travellerLost");

                currentLamp = null;
                movingToLamp = false;
                targetLamp = null;
                
                
             

               
               
            }
        }
        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Alerted"))
        {
            Debug.Log("Alerted");
            RaycastHit hit;
            if (Physics.Raycast(transform.position + upward, direction.normalized, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.transform == traveller)
                {
                    monsterAnim.SetTrigger("travellerSpotted");
                   
                }

                else
                {
                    if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Alerted"))
                    {
                        monsterAnim.SetTrigger("isLooking");
                        currentTarget = traveller.position;
                    }
                }
            }
            targetLamp = null;
        }
        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Investigating"))
        {
            Debug.Log("Investigating");
            nav.SetDestination(currentTarget);
            if (Vector3.Distance(transform.position, currentTarget) < 1 || Vector3.Distance(roamCenterPoint, currentTarget) <= maxRoamDistance)
            {
                monsterAnim.SetTrigger("nothingFound");
                movingToLamp = false;
                currentTarget = transform.position;
                Debug.Log("Reset");
            }
            targetLamp = null;
        }
        else
        {
            if (!movingToLamp)
            { // not currently moving, find new place to move to
                Debug.Log("not moving");
                moveToLamp();
            }
            else if (Vector3.Distance(transform.position, currentTarget) < lampDistance)
            { //reached destination, should make this variable public for testing
                movingToLamp = false;
                Debug.Log("in the elseif");
                nav.SetDestination(transform.position);
                currentLamp = findCurrentLamp();
            }
            else
            {
                Debug.Log("in the else");
            }
        }
    }

    public void moveToLamp()
    {
        Debug.Log("in the move to lamp");
        if (currentLamp == null) // initial gamestate when monster is first placed
        {
            float distance = Mathf.Infinity;
            foreach (GameObject lamp in lamps) //this part is also not quite working
            {
                if (Vector3.Distance(transform.position, lamp.transform.position) < distance) // edge case, does not take into account lit lamp
                {
                    distance = Vector3.Distance(transform.position, lamp.transform.position);
                    currentLamp = lamp;
                }
            }
        }
        else //general case of finding lamps
        {
            lightSourceController lController = currentLamp.GetComponentInParent<lightSourceController>();
            if (lController == null)
            {
                Debug.Log("Could not find lightsourcontroller");
            }
            GameObject[] adjacentLamps = lController.getAdjacentSources();
            List<GameObject> possibleTargets = new List<GameObject>();
            foreach (GameObject adjacentlamp in adjacentLamps)
            {
                lightSourceController adjLController = adjacentlamp.GetComponentInParent<lightSourceController>();
                int lightType = adjLController.getCurrentLightType();
                if (lightType == 1 || lightType == 3)
                {
                    possibleTargets.Add(adjacentlamp);
                }
            }
            if (lController.getCurrentLightType() == 1 || lController.getCurrentLightType() == 3) //possible bug need to fix when lamps are implemented
            {
                currentTarget = transform.position;
                nav.SetDestination(currentTarget);
                targetLamp = null;
                movingToLamp = false;
                return;
            }
            GameObject[] targetLamps = possibleTargets.ToArray();
            if (targetLamps.Length == 0)
            {
                targetLamps = adjacentLamps; //WILL NEED TO DEBUGGGGGGG
                Debug.Log("PLEASE PLACE BREAKPOINT HERE unsure if this will behave correctly");
            }
            int ran = Random.Range(0, targetLamps.Length); //unsure doesnt work with length - 1, tried the remove 1 now it works???!!!
            targetLamp = targetLamps[ran];
            currentTarget = targetLamp.transform.position;
            nav.SetDestination(currentTarget);
            movingToLamp = true;
        }
    }

    public GameObject findCurrentLamp()
    {
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) < lampDistance)
            {
                return lamp;
            }
        }
        return null;
    }   


   
    

    public void monsterLampLit(GameObject litLamp)
    {
        Debug.Log("MonsterLampLit");
        GameObject lamp;
        if (targetLamp == null)
        {
            lamp = currentLamp;
        }
        else
        {
            lamp = targetLamp;
        }
        lightSourceController lController = lamp.GetComponentInParent<lightSourceController>();
        if (lController == null)
        {
            Debug.Log("Could not find lightsourcontroller");
        }
        else
        {
            GameObject[] adjacentLamps = lController.getAdjacentSources();
            foreach (GameObject adjacentlamp in adjacentLamps)
            {
                if (litLamp == adjacentlamp)
                {
                    targetLamp = litLamp;
                    currentTarget = litLamp.transform.position;
                    nav.SetDestination(currentTarget);
                    movingToLamp = true;
                }
            }
        }
    }
}

