﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;


public class EnemyMovementRefined : MonoBehaviour
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
    float soundTimer = 0f;

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
    public AudioSource roamingSound;
    public AudioSource attackSound;

    public GameObject popUp;

    public GameObject monsterGeo;
    private Animator bodyAnim;

    public float attackCooldownValue;
    [SerializeField]
    private float currentAttackCooldown;
    //Need local variable to avoid race conditions with update frame
    private bool isStunned;
    private bool isdistracted;
    public bool isBaby;

    private void Awake()
    {
        roamingSound = transform.Find("Audio Source").transform.GetComponent<AudioSource>();
        attackSound = transform.Find("Audio Source (1)").transform.GetComponent<AudioSource>();
        attackSound.enabled = false;
        roamingSound.enabled = true;
        moving = false;
        traveller = GameObject.FindGameObjectsWithTag("Traveller")[0].transform;
        nav = GetComponent<NavMeshAgent>();
        monsterAnim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
        currentTarget = transform.position;
        timer = 0f;
        soundTimer = 0f;
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


        bodyAnim = monsterGeo.GetComponent<Animator>();
        if (bodyAnim == null)
            Debug.Log("Could not find the bodyanim");


        currentAttackCooldown = 0;
        isStunned = false;
        isdistracted = false;


    }


    void OnTriggerStay(Collider other)
    {
        if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {
            //Debug.Log("inhere");
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


        if (bodyAnim.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {   //for now do nothing while animation completes
            //channge nothing, it can go back to doing what it does after
            return;
        }
        /* 
        else { //once animation is done make sure we can move again
            if (nav.isStopped)
                nav.isStopped = false;
        }
        */



        //for testing purposes
        if (Input.GetKeyDown(KeyCode.M))
        {
            startAttack();
        }

        if (currentAttackCooldown != 0)
        {

            currentAttackCooldown = Mathf.Clamp(currentAttackCooldown -= Time.deltaTime, 0f, attackCooldownValue);
        }



        //monster sounds; possibly temporary, depending if we use 3D audio controller
        //The attackSound is almost definitely temporary, assuming we will someday have
        //and attack state 
        soundTimer += Time.deltaTime;
        float distanceFromTraveler = Vector3.Distance(transform.position, traveller.transform.position);
        if (distanceFromTraveler < 1 && !isdistracted)
        {
            if (startAttack())
            {
                attackSound.enabled = true;
                attackSound.volume = 0.5f;
                if (!attackSound.isPlaying)
                {
                    attackSound.Play();
                }
            }
        }
        else if (distanceFromTraveler < 3)// && distanceFromTraveler > 1)
        {
            if (soundTimer < 1)
            {
                roamingSound.enabled = true;
                roamingSound.volume = (1 / distanceFromTraveler) / 2;
                if (!roamingSound.isPlaying)
                {
                    roamingSound.Play();
                }
            }
            else
            {
                roamingSound.enabled = false;
            }
        }
        else
        {
            if (!roamingSound.isPlaying)
            {
                roamingSound.Play();
            }
            roamingSound.volume = 0;
        }
        if (soundTimer > 3)
        {
            roamingSound.volume = 0;
            soundTimer = 0f;
        }
        //end of monster sound control
        if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {


            //Debug.Log("Stunned");
            nav.SetDestination(transform.position);
            //Monster sounds
            roamingSound.enabled = false;
            attackSound.enabled = false;


            timer += Time.deltaTime;
            if (timer > 5)
            {
                monsterAnim.SetTrigger("recovered");
                //Monster sounds
                roamingSound.enabled = true;
                //
                timer = 0f;
                movingToLamp = false;

                targetLamp = null;

                isStunned = false;
            }


            return;
        }

        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            //Debug.Log("Chase");
            currentTarget = traveller.position;
            nav.SetDestination(currentTarget);
            if (Vector3.Distance(roamCenterPoint, traveller.position) >= maxRoamDistance)
            {
                monsterAnim.SetTrigger("travellerLost");
                //monster sounds
                roamingSound.enabled = true;
                attackSound.enabled = false;
                currentLamp = null;
                movingToLamp = false;
                targetLamp = null;

            }
        }
        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Alerted"))
        {
            //Debug.Log("Alerted");
            isdistracted = false;
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
            //Debug.Log("Investigating");
            nav.SetDestination(currentTarget);
            if (Vector3.Distance(transform.position, currentTarget) < 1 || Vector3.Distance(roamCenterPoint, currentTarget) <= maxRoamDistance)
            {
                monsterAnim.SetTrigger("nothingFound");
                movingToLamp = false;
                currentTarget = transform.position;

                //Debug.Log("Reset");
            }

            targetLamp = null;
        }
        else
        {
            if (!movingToLamp)
            { // not currently moving, find new place to move to
                //Debug.Log("not moving");
                moveToLamp();
            }
            else if (Vector3.Distance(transform.position, currentTarget) < lampDistance)
            { //reached destination, should make this variable public for testing
                movingToLamp = false;

                //Debug.Log("in the elseif");
                nav.SetDestination(transform.position);
                currentLamp = findCurrentLamp();
            }
            else
            {
                //Debug.Log("in the else");
            }
        }

        if (!isStunned)
        {
            if (movingToLamp != bodyAnim.GetBool("isMoving"))
            {
                //Debug.Log("called switch");
                bodyAnim.SetBool("isMoving", movingToLamp);
            }
        }




    }

    public void moveToLamp()
    {
        //Debug.Log("in the move to lamp");

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
                //Debug.Log("PLEASE PLACE BREAKPOINT HERE unsure if this will behave correctly");
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
        float distance = Mathf.Infinity;
        GameObject newCurrentLamp = null;
        foreach (GameObject newlamp in lamps) //this part is also not quite working
        {
            if (Vector3.Distance(transform.position, newlamp.transform.position) < distance) // edge case, does not take into account lit lamp
            {
                distance = Vector3.Distance(transform.position, newlamp.transform.position);
                newCurrentLamp = newlamp;
            }
        }
        Debug.Log("MonsterLampLit");
        if (newCurrentLamp == litLamp)
        {
            monsterAnim.SetTrigger("nearbyLitLamp");
            Debug.Log("inhere1");
            targetLamp = litLamp;
            currentTarget = litLamp.transform.position;
            nav.SetDestination(currentTarget);
            movingToLamp = true;
            if (!isBaby)
            {
                isdistracted = true;
            }
        }
        lightSourceController lController = newCurrentLamp.GetComponentInParent<lightSourceController>();
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
                    monsterAnim.SetTrigger("nearbyLitLamp");
                    Debug.Log("inhere2");
                    targetLamp = litLamp;
                    currentTarget = litLamp.transform.position;
                    nav.SetDestination(currentTarget);
                    movingToLamp = true;
                    if (!isBaby)
                    {
                        isdistracted = true;
                    }
                }
            }
        }
    }


    public bool startAttack()
    {
        //important -> must stop movement before animation
        // or you will get slide effect                             //optimize -> change  to local var 
        if (currentAttackCooldown == 0 && !monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned") && !isdistracted)
        { //only attack on a cooldown 
            nav.isStopped = true;
            bodyAnim.SetTrigger("isAttack");
            Invoke("doneAttacking", 1f);
            currentAttackCooldown = attackCooldownValue;

            return true;
        }
        return false;

    }
    //Must use this since the animation is so short it cause the nav mesh
    // to continue moving almost immeditely
    public void doneAttacking()
    {
        GameObject trav = GameObject.FindGameObjectWithTag("Traveller");
        travellerHealth travHealth = trav.GetComponent<travellerHealth>();
        travHealth.TakeBasicDamage(10);
        attackSound.enabled = false;
        if (nav.isStopped)
            nav.isStopped = false;
    }

    public void setStunned()
    {



        bodyAnim.SetBool("isMoving", false);

        monsterAnim.SetTrigger("isStunned");
        isStunned = true;
    }


}
