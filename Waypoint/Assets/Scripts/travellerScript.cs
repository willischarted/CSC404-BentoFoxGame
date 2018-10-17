using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class travellerScript : MonoBehaviour


// Updates:
// Added public function for cape brightness increase; took away user ability to do that from
// travellerScript.
//
//TODO: Implement line of sight 
//TODO: Fully debug the go to nearest light mechanic. Current issue:
//          if you do more than one light at a time and then turn off the 
//          target light, it will go back to the original light.
//        On a related note, the first round messes up if you wait for it to pause.

{


    public Text winText;
    public float speed = 200f;
    bool hasTarget;
    Animator anim;
    Vector3 target;
    GameObject lastVisited;
    GameObject currentLight;


    //Transform lastVisited;
    //Transform currentLight;

    private Rigidbody rb;

    public Transform goal;

    private NavMeshAgent agent;

    public float lightValue;
    private MeshRenderer[] meshRenderers;

    private MeshRenderer meshRendererTraveller;
    private Material cloak;
    private Material hat;
    private bool started = false;
    //private readonly float MAX_LD = 6f; //LD = lamp distance 
    public float MAX_LD;
    //private readonly float MIN_LD = 1f;
    public float MIN_LD;
    private GameObject startArea;

    private float MAX_INTENSITY = 1f;
    private float MIN_INTENSITY = -3f;
    NavMeshAgent nav;
        

    // Use this for initialization
    void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        hasTarget = false;
        winText.text = "";
        startArea = GameObject.FindGameObjectWithTag("StartArea");
        lastVisited = startArea;
        nav = GetComponent<NavMeshAgent>();
        //rb  = GetComponent<Rigidbody>();
        // if (rb == null)
        //Debug.Log("Could not find traveller rb");

        agent = GetComponent<NavMeshAgent>();
        //agent.destination = goal.position; 

        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        if (meshRenderers == null) {
            Debug.Log("could not find meshrenderers");
        }

        foreach (MeshRenderer m in meshRenderers) {
            if (m != GetComponent<MeshRenderer>()) {
                // this is the child
                meshRendererTraveller = m;
            }
        }

        if (meshRendererTraveller == null) {
            Debug.Log("Could not find meshrenderer traveller");

        }
       // Debug.Log(meshRendererTraveller.materials.Length);

        //Debug.Log(meshRendererTraveller.materials.Length);

        foreach (Material m in meshRendererTraveller.materials) {


            //(m.name.Replace("(Instance)","") == ) not working
            if (m.name.Replace("(Instance)", "").CompareTo("TravellerCloak") == 1) {//TravellerCloak
                cloak = m;

            }

            if (m.name.Replace("(Instance)", "").CompareTo("TravellerHat") == 1) {//TravellerCloak
                hat = m;

            }


        }


    }
    void Update() {
        //Testing emmisive cloak material
        //DynamicGI.SetEmissive(cloak, new Color(255f, 255f, 255f, 1.0f) * lightValue);
        if (litLamps() == 0 && started) {
            lightValue -= 0.0001f;
        }
        if (lightValue < MAX_INTENSITY && lightValue > MIN_INTENSITY)
        {
            lightValue = Mathf.Clamp(lightValue, -0.002f, 0.005f);
            cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            hat.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        } else if (lightValue == MIN_INTENSITY)
        {
            //GameOver
        }

    }

    // Update is called once per frame
    void FixedUpdate()
        

    {
        
       // Debug.Log(anim.enabled);
        //Goes towards a lamp
        if (hasTarget && Vector3.Distance(transform.position, target) > MIN_LD) {
            anim.enabled = false;
            
            //Debug.Log(Vector3.Distance(transform.position, target;));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), speed * Time.deltaTime);     
            transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), speed * Time.deltaTime);

            //transform.position += transform.forward * speed * Time.deltaTime;
            //  rb.mov
            //transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

           // Debug.Log("Moving towards" + target);
        }

        //Has reached the lamp
        else {
            //Debug.Log("Reached");
            hasTarget = false;
            anim.enabled = true;

            // These two lines
            currentLight = findCurrentLamp();
            lastVisited = currentLight;
            
            



            if (currentLight != null)
            {
                //Makes it go to the next lamp in sequence
                if (!currentLight.Equals(lastVisited))
                {
                    GameObject nextLamp = checkLamps();
                    if (nextLamp != null)
                    {
                        //Debug.Log(nextLamp.Equals(lastVisited));
                        //Debug.Log("last");
                        anim.enabled = false;
                        hasTarget = true;
                        transform.LookAt(nextLamp.transform);
                        target = nextLamp.transform.position;
                    } 
                }
                //If the light is turned off, it looks for another light
                if (currentLight.transform.GetChild(0).
                    GetComponentInChildren<Light>().intensity == 0)
                {
                    GameObject potential_target = checkLamps();
                    if (potential_target != null)
                    {
                        if (lineOfSight(potential_target.transform.position,
                            (int)MAX_LD, potential_target.transform))
                        {
                            anim.enabled = false;
                            hasTarget = true;
                            transform.LookAt(potential_target.transform);
                            target = potential_target.transform.position;
                        }
                    }
                }
            }
            
        }

    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            winText.text = "WIN!";
        }
        /*
         * 
        if (collision.gameObject.CompareTag("LampLight"))
        {
            lastVisited = currentLight;
            currentLight = findCurrentLamp();

            if (currentLight != null)
            {
                //Makes it go to the next lamp in sequence
                if (!currentLight.Equals(lastVisited))
                {
                    GameObject nextLamp = checkLamps();
                    if (nextLamp != null)
                    {
                        Debug.Log(nextLamp.Equals(lastVisited));
                        Debug.Log("last");
                        hasTarget = true;
                    }
                }
            }
        }*/
    }

    /*
     * Increases the cape's brightness
     * */
    public void increaseCape()
    {
        lightValue += 0.0002f;
    }



    /*
     * Is called by the Player controller when the firefly hits a switch
     * Updates the target accordingly
     * 
     */
    public void setTarget(Transform goal, float light) {
        started = true;
        this.goal = goal;
        if (light > 0)
        {            
            if (Vector3.Distance(transform.position, goal.position) <= MAX_LD &&
                lineOfSight(goal.position, (int)MAX_LD, goal))
            {
               anim.enabled = false;
                Debug.Log("stop!");
        
                Debug.DrawRay(transform.position, goal.position, Color.blue);                
                target = goal.position;
                hasTarget = true;
                lastVisited = currentLight;

                
                //agent.SetDestination(goal.position);
            }
        }
        //if you turn a light off while traveler is travelling, or in general
        else
        {

            if (goal == findCurrentLamp() || goal.position == target)
            {
                GameObject potentialTarget;
                //If you turn off the lamp it is at currently
                //it will see if there is another lit lamp around, not necessarily 
                //the last visited
                if ((potentialTarget = checkLamps()) != null)
                {
                    if (lineOfSight(potentialTarget.transform.position,
                        (int)MAX_LD, potentialTarget.transform))
                    {
                        anim.enabled = false;
                        Debug.Log("find a lamp");
                        target = potentialTarget.transform.position;
                        hasTarget = true;
                    }
                }
            }
            else if (goal.position != target && hasTarget)
            {
                hasTarget = true;
            }
            else
            {
                anim.enabled = true;
                hasTarget = false;
            }
            
        }
    }

    /*
     * Finds the lamp the traveller is currently staying at. 
     */
    public GameObject findCurrentLamp()
    {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) < MIN_LD)
            {
                return lamp;
            }
        }
        return null;
    }

    /**
     * Sees if there is another (but just one) lamp lit after the current one.
     * Should not be the previous lamp.
     * 
     */
    public GameObject checkLamps()
    {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        int litlamps = 0;
        GameObject nextLamp = null;
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) <= MAX_LD)
            {
                if (lamp.transform.GetChild(0).
                    GetComponentInChildren<Light>().intensity == 3) {
                    if (!lamp.Equals(lastVisited) &&
                        !lamp.Equals(currentLight))
                    {
                        nextLamp = lamp;
                        litlamps++;
                    }
                }
            }
        }
        if (litlamps == 1)
        {
            return nextLamp;
        }
        return null;
    }

    public int litLamps()
    {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        int litlamps = 0;
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) <= MAX_LD)
            {
                if (lamp.transform.GetChild(0).
                    GetComponentInChildren<Light>().intensity == 3)
                {
                    litlamps++;
                }
            }
        }
        return litlamps;
    }

    /*
     * The monster uses this to decrease the traveler's health.
     * */
    public void decreaseCape (int damage)
    {
        lightValue -= damage / 1000;
    }


    private bool lineOfSight(Vector3 lampPosition, int lampRange, Transform lamp)
    {
        //Raycast to see if there is an obstructing object
        bool unobstructed = false;
        bool inSightLine = false;
        RaycastHit hitInfo;
        Vector3 currPos = transform.position;
    //    Debug.Log(lamp.tag);

        bool hitSomething = Physics.Raycast(currPos, (lampPosition - currPos), out hitInfo, lampRange);
    //    Debug.Log(hitSomething);
    //    Debug.Log(hitInfo.transform.position + " " + lampPosition);
        Debug.DrawRay(currPos, (lampPosition - currPos).normalized * lampRange, Color.yellow, 5.0f, true);
        if (hitSomething)
        {
           
            Debug.Log(hitInfo.collider.tag);
            if (hitInfo.transform.position == lampPosition)
            {
                Debug.Log("twas THE lamp");
                unobstructed = true;
            }
        }

        //Check if it is within 60 degrees on either side of the direction the traveler is facing
        if (Vector3.Angle((lampPosition - currPos), transform.forward) < 60f)
        {
            inSightLine = true;
        }

        return unobstructed;
        //return (unobstructed && inSightLine);
    }
}
