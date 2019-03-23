using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{

    private Queue<GameObject> lampQueue = new Queue<GameObject>();
    public static int roamingLampsNum;
    public GameObject[] setRoaminglamps = new GameObject[roamingLampsNum];

    private bool isDistracted;
    public bool isBaby;
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
    public float lampDistance = 1.3f;
    GameObject currentLamp;
    GameObject[] lamps;
    public GameObject targetLamp;
    public AudioSource roamingSound;
    public AudioSource attackSound;
    private travellerHealth tHealth;
    private travellerMovement tMovement;

    public GameObject popUp;
    public GameObject popUp2;

    public GameObject monsterGeo;
    private Animator bodyAnim;

    public float attackCooldownValue;
    [SerializeField]
    private float currentAttackCooldown;
    //Need local variable to avoid race conditions with update frame
    private bool isStunned;

    private bool attackInterrupt;

    private bool isChaseTrav;


    private void Awake()
    {
        roamingSound = transform.Find("Audio Source").transform.GetComponent<AudioSource>();
        attackSound = transform.Find("Audio Source (1)").transform.GetComponent<AudioSource>();
        attackSound.enabled = false;
        roamingSound.enabled = false;
        moving = false;
        traveller = GameObject.FindGameObjectsWithTag("Traveller")[0].transform;
        tHealth = traveller.GetComponent<travellerHealth>();
        tMovement = traveller.GetComponent<travellerMovement>();
        nav = GetComponent<NavMeshAgent>();
        monsterAnim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
        currentTarget = transform.position;
        timer = 0f;
        soundTimer = 0f;
        upward.Set(0f, 0.2f, 0f);
        lamps = GameObject.FindGameObjectsWithTag("LampLight");
        List<GameObject> validLamps = new List<GameObject>();
        foreach (GameObject lamp in setRoaminglamps)
        {
            lampQueue.Enqueue(lamp);
        }

        bodyAnim = monsterGeo.GetComponent<Animator>();
        if (bodyAnim == null)
            Debug.Log("Could not find the bodyanim");

        currentAttackCooldown = 0;
        isStunned = false;
        isDistracted = false;
        currentLamp = findCurrentLamp();
        //Debug.Log(currentLamp);
        targetLamp = findCurrentLamp();

        attackInterrupt = false;
        isChaseTrav = false;
    }


    void OnTriggerStay(Collider other)
    {
        if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned") || isDistracted)
        {
            return;
        }

        else
        {
            if (other.gameObject.transform == traveller)
            {
                direction = other.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                //Debug.Log(angle);
                if (angle < fieldOfViewAngle * 0.5f)
                {
                    monsterAnim.SetTrigger("isAlerted");
                }
                else
                {
                    monsterAnim.ResetTrigger("isAlerted");
                }
            }
        }
    }



    void Update()
    {
        

        if (bodyAnim.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {   //for now do nothing while animation completes
            //channge nothing, it can go back to doing what it does after
           // var rotation = Quaternion.LookRotation(traveller.position);
            var rotation = Quaternion.LookRotation (traveller.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
            nav.SetDestination(transform.position);
            return;
        }
        /* 
        else { //once animation is done make sure we can move again
            if (nav.isStopped)
                nav.isStopped = false;
        }
        */

        //for testing purposes
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    startAttack();
       // }

        if (currentAttackCooldown != 0)
        {

            currentAttackCooldown = Mathf.Clamp(currentAttackCooldown -= Time.deltaTime, 0f, attackCooldownValue);
        }



        //monster sounds; possibly temporary, depending if we use 3D audio controller
        //The attackSound is almost definitely temporary, assuming we will someday have
        //and attack state 
        soundTimer += Time.deltaTime;
        float distanceFromTraveler = Vector3.Distance(transform.position, traveller.transform.position);
        if (distanceFromTraveler < 1 && !isDistracted)
        {
            if (startAttack())
            {
                attackSound.enabled = true;
                attackSound.volume = 0.5f;
                if (!attackSound.isPlaying)
                {
                    if (roamingSound.isPlaying)
                    {
                        roamingSound.Pause();
                    }
                    attackSound.Play();
                }
            }
        }
        if (distanceFromTraveler < 3 && !tHealth.isDead && !tMovement.closeToExit)// && distanceFromTraveler > 1)
        {
            if (soundTimer < 2)
            {
                roamingSound.enabled = true;
                roamingSound.volume = (1 / distanceFromTraveler) / 2;
                if (!roamingSound.isPlaying && !attackSound.isPlaying)
                {
                    roamingSound.Play();
                }
            }
            else
            {
                roamingSound.Stop();
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
        if (soundTimer > 4)
        {
            roamingSound.volume = 0;
            soundTimer = 0f;
        }
        //end of monster sound control


        if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {
            if (isChaseTrav)
                isChaseTrav = false;
            nav.SetDestination(transform.position);
            nav.velocity = Vector3.zero;
            nav.isStopped = true;

            //Monster sounds
            roamingSound.enabled = false;
            attackSound.enabled = false;

            timer += Time.deltaTime;
            int countDown = 5 - (int)timer;
            popUp2.GetComponent<WorldSpaceObjectController>().setPopUpText(countDown.ToString());
            if (timer > 5)
            {
                popUp2.SetActive(false);
                monsterAnim.SetTrigger("recovered");
                //Monster sounds
                roamingSound.enabled = true;

                timer = 0f;
                movingToLamp = false;
                isStunned = false;
            }
            return;
        }

        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Chase"))
        {
            currentTarget = traveller.position;
            nav.SetDestination(currentTarget);
            nav.isStopped = false;
            isChaseTrav = true;
            if (Vector3.Distance(roamCenterPoint, traveller.position) >= maxRoamDistance)
            {
                monsterAnim.SetTrigger("travellerLost");
                //monster sounds
                roamingSound.enabled = true;
                attackSound.enabled = false;
                movingToLamp = false;
                isChaseTrav = false;

            }
            if (distanceFromTraveler < 1)
            {

                isChaseTrav = false;
                nav.isStopped = true;
               
            }
        }

        else if (monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Alerted"))
        {
            //Debug.Log("Alerted");
            isChaseTrav = false;
            isDistracted = false;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + upward, direction.normalized, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.transform == traveller)
                {
                    monsterAnim.SetTrigger("travellerSpotted");
                }
                else
                {
                    monsterAnim.SetTrigger("isLooking");
                }
            }
                
        }

        else
        {
            isChaseTrav = false;
            if (!movingToLamp)
            { // not currently moving, find new place to move to
                nextLamp(lampQueue);
            }
            else if (Vector3.Distance(transform.position, currentTarget) < lampDistance)
            {
                movingToLamp = false;
                isDistracted = false;
                nav.SetDestination(transform.position);
                nav.velocity = Vector3.zero;
                nav.isStopped = true;
                currentLamp = findCurrentLamp();
            }
            else
            {
                if (!isLit(targetLamp))
                {
                    float lightDuration = 0f;
                    int lampPriority = 0;
                    GameObject[] adjLamps = targetLamp.GetComponentInParent<lightSourceController>().getAdjacentSources();
                    foreach (GameObject lamp in adjLamps)
                    {
                        if (isLit(lamp))
                        {
                            if ((lightDuration < getLightDuration(lamp))&&(lampPriority<=getLampPriority(lamp)))
                            {
                                targetLamp = lamp;
                                lightDuration = getLightDuration(lamp);
                                movingToLamp = true;
                                bodyAnim.SetBool("isMoving", true);
                            }
                        }
                    }
                }
                currentTarget = targetLamp.transform.position;
                nav.SetDestination(currentTarget);
                nav.isStopped = false;
            }
        }

        //Debug.Log(isChaseTrav);
        if (((Vector3.Distance(transform.position, currentTarget) < lampDistance) && !isChaseTrav) ||isStunned)
        {
           
            bodyAnim.SetBool("isMoving", false);
        }
        else
        {
            bodyAnim.SetBool("isMoving", true);
        }
       
            
        /*
        if (!isStunned)
        {
            if (movingToLamp != bodyAnim.GetBool("isMoving"))
            {
                //Debug.Log("called switch");
                bodyAnim.SetBool("isMoving", movingToLamp);
            }
        }
        */
    }

    public GameObject findCurrentLamp()
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
        return newCurrentLamp;

    }

    public void monsterLampLit(GameObject litLamp)
    {

        if (isStunned)
            return;
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
       // Debug.Log("MonsterLampLit");
        if ((newCurrentLamp == litLamp)&&(getLampPriority(targetLamp)<=getLampPriority(litLamp)))
        {
            monsterAnim.SetTrigger("nearbyLitLamp");
            //Debug.Log("inhere1");
            targetLamp = litLamp;
            currentTarget = litLamp.transform.position;
            nav.SetDestination(currentTarget);
            nav.isStopped = false;
            movingToLamp = true;
            bodyAnim.SetBool("isMoving", true);

            if (!isBaby)
            {
                isDistracted = true;
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
                if ((litLamp == adjacentlamp)&& (getLampPriority(targetLamp) <= getLampPriority(litLamp)))
                {
                    Debug.Log("target:"+ getLampPriority(targetLamp)+ " litlamp:"+getLampPriority(litLamp));
                    monsterAnim.SetTrigger("nearbyLitLamp");
                   // Debug.Log("inhere2");
                    targetLamp = litLamp;
                    currentTarget = litLamp.transform.position;
                    nav.SetDestination(currentTarget);
                    nav.isStopped = false;
                    movingToLamp = true;
                    bodyAnim.SetBool("isMoving", true);
                    if (!isBaby)
                    {
                        isDistracted = true;
                    }
                }
            }
        }
    }

    public bool startAttack()
    {
        //important -> must stop movement before animation
        // or you will get slide effect                             //optimize -> change  to local var 
        int lightType = currentLamp.GetComponentInParent<lightSourceController>().getCurrentLightType();
        Debug.Log("is distracted " + isDistracted);
        if (currentAttackCooldown == 0 && !monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Stunned") && !isStunned && lightType!=3)
        { //only attack on a cooldown 
            nav.SetDestination(transform.position);
            nav.velocity = Vector3.zero;
            nav.isStopped = true;

           

            bodyAnim.SetTrigger("isAttack");
           // Invoke("doneAttacking", 1f);
            StartCoroutine(doneAttacking());
            currentAttackCooldown = attackCooldownValue;
            return true;
        }
        return false;
    }


    //Must use this since the animation is so short it cause the nav mesh
    // to continue moving almost immeditely
    IEnumerator doneAttacking()
    {

        //have to wait for next frame to cheack   
       // yield return new WaitForSeconds(0.4f);
        //wait until the apex of the animation's swing
        yield return new WaitForSeconds(1.0672f);
       
        Debug.Log("done");
        GameObject trav = GameObject.FindGameObjectWithTag("Traveller");
        if (trav == null)
            Debug.Log("Could not find trav");
        travellerHealth travHealth = trav.GetComponent<travellerHealth>();
        if (travHealth == null)
            Debug.Log("Could not find health sciprt");
        Animator travAnim = travHealth.getTravellerAnimator();
        if (travAnim == null)
            Debug.Log("Could not find the traveller animator");
        if (!attackInterrupt && !isStunned) { //may have been stunned a few frames before, not interrupted but code raced past, still stunned
            travAnim.SetTrigger("isAttacked");
            travHealth.TakeBasicDamage(20);
            attackSound.enabled = false;
        }
        if (nav.isStopped)
            nav.isStopped = false;
        
        //    return;
        
        attackInterrupt = false;
        //yield return null;
    }


    public void setStunned()
    {
       
          isStunned = true;
        if (bodyAnim.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) {
            float playBackTime  = ((bodyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime % 1) * 2) * 0.8f;
            
            Debug.Log(playBackTime);
            if (playBackTime <= 1.0672f)
                attackInterrupt = true;
        }
        isChaseTrav = false;
        nav.SetDestination(transform.position);
        nav.velocity = Vector3.zero;
        nav.isStopped = true;

      //  nav.SetDestination(transform.position);
       // nav.velocity = Vector3.zero;
       // nav.isStopped = true;
        bodyAnim.SetTrigger("isStun");

        bodyAnim.SetBool("isMoving", false);
        monsterAnim.SetTrigger("isStunned");
       // isStunned = true;

        
    }
    public bool getIsStunned()
    {
        return isStunned;
    }

    void nextLamp(Queue<GameObject> lampQueue)
    {
        float lightDuration = 0f;
        int lampPriority = 0;
        if (isLit(currentLamp))
        {
            lightDuration = getLightDuration(currentLamp);
            lampPriority = getLampPriority(currentLamp);
            movingToLamp = false;
        }
        GameObject[] adjLamps = currentLamp.GetComponentInParent<lightSourceController>().getAdjacentSources();
        foreach (GameObject lamp in adjLamps)
        {
            if (isLit(lamp))
            {
                if ((lightDuration < getLightDuration(lamp))&&(lampPriority<=getLampPriority(lamp)))
                {
                    targetLamp = lamp;
                    lightDuration = getLightDuration(lamp);
                    movingToLamp = true;
                    bodyAnim.SetBool("isMoving", true);
                }
            }
        }
        if (!isLit(currentLamp) || !isLit(targetLamp))
        {
            targetLamp = lampQueue.Dequeue();
            lampQueue.Enqueue(targetLamp);
            movingToLamp = true;
            bodyAnim.SetBool("isMoving", true);
        }
        if (currentLamp != targetLamp)
        {
            currentTarget = targetLamp.transform.position;
            nav.SetDestination(currentTarget);
            nav.isStopped = false;
        }
    }

    bool isLit(GameObject lamp)
    {
        int lightType = lamp.GetComponentInParent<lightSourceController>().getCurrentLightType();
        return lightType == 1 || lightType == 3;
    }

    float getLightDuration(GameObject lamp)
    {
        return lamp.GetComponentInParent<lightSourceController>().lightDuration;
    }

    int getLampPriority(GameObject lamp)
    {
        return lamp.GetComponentInParent<lightSourceController>().getCurrentLightType();
    }

    public bool getAttackInterrupt() {
        return attackInterrupt;
    }
}