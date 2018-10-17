using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class EnemyMovement : MonoBehaviour {

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

    public float MAX_LD;
    public GameObject lastVisited;
    private bool moving;
    private bool movingToLamp;
    public Vector3 roamCenterPoint;
    public float maxRoamDistance;

    private void Awake()
    {
        moving = false;
        traveller = GameObject.FindGameObjectsWithTag("Traveller")[0].transform;
        nav = GetComponent<NavMeshAgent>();
        monsterAnim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
        currentTarget = transform.position;
        timer = 0f;
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


    void Update() {


        if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {
            Debug.Log("Stunned");
            nav.SetDestination(transform.position);
            timer += Time.deltaTime;
            if (timer > 5)
            {
                monsterAnim.SetTrigger("recovered");
                timer = 0f;
                moving = false;
            }
        }
        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            Debug.Log("Chase");
            nav.SetDestination(traveller.position);
            if (Vector3.Distance(roamCenterPoint, traveller.position) <= maxRoamDistance)
            {
                monsterAnim.SetTrigger("travellerLost");
            }
        }
        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Alerted"))
        {
            Debug.Log("Alerted");
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, Mathf.Infinity))
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
        }
        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Investigating"))
        {
            Debug.Log("Investigating");
            nav.SetDestination(currentTarget);
            if (Vector3.Distance(transform.position, currentTarget) < 1 || Vector3.Distance(roamCenterPoint, currentTarget) <= maxRoamDistance)
            {
                monsterAnim.SetTrigger("nothingFound");
                currentTarget = transform.position;
                Debug.Log("Reset");
            }
        }
        else
        {

            if (!moving)
                { // not currently moving, find new place to move to
                Debug.Log("not moving");
                moveToLamp();
                }
            else if (Vector3.Distance(transform.position,currentTarget) < 2.5f) { //reached destination
                moving = false;
                Debug.Log("in the elseif");
                nav.SetDestination(transform.position);
                
                if (lastVisited == null){
                    lastVisited = findCurrentLamp();
                }
            }
            else
            {
                if (!movingToLamp)
                {
                    isLampLit();
                }
                
            }
        }
    }
    
    public void moveToLamp() {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        List<GameObject> validLamps = new List<GameObject>();
        foreach (GameObject lamp in lamps)
        {   
            if (Vector3.Distance(transform.position, lamp.transform.position) <= MAX_LD && Vector3.Distance(roamCenterPoint, lamp.transform.position) <= maxRoamDistance)
            {
                lightSourceController lController = lamp.GetComponentInParent<lightSourceController>();
                
                if (lController == null) {
                    Debug.Log("Could not find lightsourcontroller");
                }
                int lightType = lController.getCurrentLightType();
               // lamp.transform.GetChild(0).GetComponentInChildren<Light>().intensity == 3 && 
                if (lightType ==1  || lightType == 3 ) //1 is trav, 3 is monster
                { // lamp is lit

                    if (!lamp.Equals(lastVisited) && lamp.transform.position != currentTarget)
                    {

                        if (lastVisited != null)
                        {

                            lastVisited = findCurrentLamp();
                        }
                        currentTarget = lamp.transform.position;
                        nav.SetDestination(lamp.transform.position);
                        moving = true;
                        movingToLamp = true;
                        return; //should choose a random one
                    }
                }
                else
                {
                    if (!lamp.Equals(lastVisited) && lamp.transform.position != currentTarget)
                    {
                        if (lastVisited != null)
                        {
                            lastVisited = findCurrentLamp();
                        }
                        validLamps.Add(lamp);
                    }
                }
            }
        }
        lamps = validLamps.ToArray();
        if (lamps.Length!=0)
        {
            int ran = Random.Range(0, lamps.Length - 1);
            GameObject lamp = lamps[ran];
            currentTarget = lamp.transform.position;
            nav.SetDestination(lamp.transform.position);
            moving = true;
            movingToLamp = false;
            return;
        }
    }

    public GameObject findCurrentLamp()
    {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) <= 2.5f)
            {
                return lamp;
            }
        }
        return null;
    }

    public void isLampLit()
    {
        Debug.Log("checking if lamp is lit");
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        List<GameObject> validLamps = new List<GameObject>();
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) <= MAX_LD && Vector3.Distance(roamCenterPoint, lamp.transform.position) <= maxRoamDistance)
            {
                lightSourceController lController = lamp.GetComponentInParent<lightSourceController>();

                if (lController == null)
                {
                    Debug.Log("Could not find lightsourcontroller");
                }
                int lightType = lController.getCurrentLightType();
                if (lightType == 1 || lightType == 3)
                {
                    moving = false;
                }
            }
        }
    }
}

