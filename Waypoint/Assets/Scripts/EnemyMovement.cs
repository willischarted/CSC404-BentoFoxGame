 using System.Collections;
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
    GameObject lastVisited;
    private bool moving;

    private void Awake()
    {
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
            }
        }
        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            Debug.Log("Chase");
            nav.SetDestination(traveller.position);
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
            if (Vector3.Distance(transform.position, currentTarget) < 1)
            {
                monsterAnim.SetTrigger("nothingFound");
                currentTarget = transform.position;
                Debug.Log("Reset");
            }
        }
        else
        {
            //nav.SetDestination(currentTarget);

            //
            if (!moving)
                moveToLamp();
            else if (Vector3.Distance(transform.position,currentTarget) < 1.0f) {
                moving = false;
            }
            
        }
    

    }
    
    public void moveToLamp() {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        foreach (GameObject lamp in lamps)
        {   
            // within lamp distance
            if (Vector3.Distance(transform.position, lamp.transform.position) <= MAX_LD)
            {
                if (lamp.transform.GetChild(0).
                    GetComponentInChildren<Light>().intensity == 3) { // lamp is lit
                    if (!lamp.Equals(lastVisited))
                    {
                        lastVisited = lamp;
                        currentTarget = lamp.transform.position;
                        nav.SetDestination(lamp.transform.position);
                        moving = true;
                        
                    }
                }
            }
        }

    }
}
