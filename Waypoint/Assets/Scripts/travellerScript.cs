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
//TODO: If Traveller is beside start area, go back to start area
//TODO: Fully debug the go to nearest light mechanic. Current issue:
//          if you do more than one light at a time and then turn off the 
//          target light, it will go back to the original light.
//        On a related note, the first round messes up if you wait for it to pause.

{


    public Text winText;
    public float speed = 200f;
    bool hasTarget;
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
    private readonly float MAX_LD = 6f; //LD = lamp distance 
    private readonly float MIN_LD = 1f;
    private GameObject startArea;


    // Use this for initialization
    void Start()
    {

        hasTarget = false;
        winText.text = "";
        startArea = GameObject.FindGameObjectWithTag("StartArea");
        lastVisited = startArea;
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
        Debug.Log(meshRendererTraveller.materials.Length);

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
        lightValue = Mathf.Clamp(lightValue, -0.002f, 0.005f);
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        hat.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);

    }

    // Update is called once per frame
    void FixedUpdate()


    {

        //Goes towards a lamp
        if (hasTarget && Vector3.Distance(transform.position, target) > MIN_LD) {
            //Debug.Log(Vector3.Distance(transform.position, target));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), speed * Time.deltaTime);
            //transform.position += transform.forward * speed * Time.deltaTime;
            //  rb.mov
            //transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

        //Has reached the lamp
        else {

            hasTarget = false;
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
                        hasTarget = true;
                        target = potential_target.transform.position;
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
        if (light == 3)
        {
            if (Vector3.Distance(transform.position, goal.position) <= MAX_LD)
            {

                target = goal.position;
                hasTarget = true;
                lastVisited = currentLight;
                //agent.SetDestination(goal.position);
            }
        }
        else
        {
            hasTarget = true;
            target = lastVisited.transform.position;
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
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LampLight"))
        {
            Debug.Log("switch");
            lastVisited = other.gameObject;
        }
    }*/
}
