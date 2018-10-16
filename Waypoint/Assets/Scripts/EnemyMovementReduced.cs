using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class EnemyMovementReduced : MonoBehaviour
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

    public float MAX_LD;
    public GameObject lastVisited;
    private bool moving;
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
            }
        }

        else
        {

            if (!moving)
            { // not currently moving, find new place to move to
                Debug.Log("not moving");
                moveToLamp();
            }
            else if (Vector3.Distance(transform.position, currentTarget) < 3.0f)
            { //reached destination
                moving = false;
                Debug.Log("in the elseif");
                nav.SetDestination(transform.position);

                if (lastVisited == null)
                {
                    lastVisited = findCurrentLamp();
                }
            }
        }
    }

    public void moveToLamp()
    {
        Debug.Log("in here");
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        List<GameObject> validLamps = new List<GameObject>();
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) <= MAX_LD && Vector3.Distance(roamCenterPoint, lamp.transform.position) <= maxRoamDistance)
            {
                if (lamp.transform.GetChild(0).GetComponentInChildren<Light>().intensity == 3)
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
        if (lamps.Length != 0)
        {
            int ran = Random.Range(0, lamps.Length - 1);
            GameObject lamp = lamps[ran];
            currentTarget = lamp.transform.position;
            nav.SetDestination(lamp.transform.position);
            moving = true;
            return;
        }
    }

    public GameObject findCurrentLamp()
    {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) <= 3.0f)
            {
                return lamp;
            }
        }
        return null;
    }
}
