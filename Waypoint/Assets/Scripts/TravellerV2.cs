using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TravellerV2 : MonoBehaviour

{

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

    private float MAX_INTENSITY = 1f;
    private float MIN_INTENSITY = -3f;
    NavMeshAgent nav;


    // Use this for initialization
    void Start()
    {

        anim = transform.parent.GetComponent<Animator>();
        hasTarget = false;

        nav = GetComponent<NavMeshAgent>();
        //rb  = GetComponent<Rigidbody>();
        // if (rb == null)
        //Debug.Log("Could not find traveller rb");

        //agent.destination = goal.position; 

        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        if (meshRenderers == null)
        {
            Debug.Log("could not find meshrenderers");
        }

        foreach (MeshRenderer m in meshRenderers)
        {
            if (m != GetComponent<MeshRenderer>())
            {
                // this is the child
                meshRendererTraveller = m;
            }
        }

        if (meshRendererTraveller == null)
        {
            Debug.Log("Could not find meshrenderer traveller");

        }
        // Debug.Log(meshRendererTraveller.materials.Length);

        //Debug.Log(meshRendererTraveller.materials.Length);

        foreach (Material m in meshRendererTraveller.materials)
        {


            //(m.name.Replace("(Instance)","") == ) not working
            if (m.name.Replace("(Instance)", "").CompareTo("TravellerCloak") == 1)
            {//TravellerCloak
                cloak = m;

            }

            if (m.name.Replace("(Instance)", "").CompareTo("TravellerHat") == 1)
            {//TravellerCloak
                hat = m;

            }


        }


    }
    void Update()
    {
        //Testing emmisive cloak material
        //DynamicGI.SetEmissive(cloak, new Color(255f, 255f, 255f, 1.0f) * lightValue);
        if (litLamps() == 0 && started)
        {
            lightValue -= 0.0001f;
        }
        if (lightValue < MAX_INTENSITY && lightValue > MIN_INTENSITY)
        {
            lightValue = Mathf.Clamp(lightValue, -0.002f, 0.005f);
            cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
            hat.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        }
        else if (lightValue == MIN_INTENSITY)
        {
            //GameOver
        }      

    }

    void FixedUpdate()


    {

        // Debug.Log(anim.enabled);
        //Goes towards a lamp
        if (hasTarget && Vector3.Distance(transform.position, target) > MIN_LD)
        {
            anim.enabled = false;

            //Debug.Log(Vector3.Distance(transform.position, target;));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), speed * Time.deltaTime);
            //transform.GetChild(0).transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), speed * Time.deltaTime);

            //transform.position += transform.forward * speed * Time.deltaTime;
            //  rb.mov
            //transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            //transform.GetChild(0).position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Debug.Log("Moving towards" + target);
        }

        //Has reached the lamp
        else
        {
            //Debug.Log("Reached");
            hasTarget = false;
            anim.enabled = true;

            // These two lines
            currentLight = findCurrentLamp();
            lastVisited = currentLight;



/*

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
            }*/

        }

    }

        public void setTarget(GameObject light)
        {

            Debug.Log(currentLight);
        if (currentLight != null)
        {
            lightSourceController lController =
                currentLight.GetComponentInParent<lightSourceController>();

            Debug.Log(lController);
            foreach (GameObject lamp in lController.adjacentSources)
            {

                if (lamp.Equals(light))
                {

                    lightSourceController lampController =
                        lamp.GetComponentInParent<lightSourceController>();
                    if (lampController.getCurrentLightType() == 1 ||
                        lampController.getCurrentLightType() == 2)
                    {
                        goal = light.transform;
                        target = goal.position;
                        hasTarget = true;
                    }
                }
            }
        } else
        {
            if(Vector3.Distance(transform.position, light.transform.position) <= 6f)
            {
                lightSourceController lampController =
                        light.GetComponentInParent<lightSourceController>();
                if (lampController.getCurrentLightType() == 1 ||
                    lampController.getCurrentLightType() == 2)
                {
                    goal = light.transform;
                    target = goal.position;
                    hasTarget = true;
                }
            }
        }
       }



        /*
        * Increases the cape's brightness
        * */
        public void increaseCape()
    {
        lightValue += 0.0002f;
    }

    /*
    * The monster uses this to decrease the traveler's health.
    * */
    public void decreaseCape(int damage)
    {
        lightValue -= damage / 1000;
        cloak.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
        hat.SetColor("_EmissionColor", new Color(255f, 255f, 255f, 1.0f) * lightValue);
    }

    /*
     * Finds the lamp the traveller is currently staying at. 
     */
    public GameObject findCurrentLamp()
    {
        GameObject[] lamps = GameObject.FindGameObjectsWithTag("LampLight");
        foreach (GameObject lamp in lamps)
        {
            if (Vector3.Distance(transform.position, lamp.transform.position) <= MIN_LD)
            {
                return lamp;
            }
        }
        return null;
    }

    /*
     * Returns the number of lamps lit that the traveler could travel to. If it's zero, 
     * traveler's health goes down.
     */
    public int litLamps()
    {
        GameObject currLamp = findCurrentLamp();
        int litlamps = 0;
        if (currLamp != null)
        {
            lightSourceController lController = currLamp.GetComponentInParent<lightSourceController>();
            foreach (GameObject lamp in lController.adjacentSources)
            {
                lightSourceController lampController = lamp.GetComponentInParent<lightSourceController>();
                if (lampController.getCurrentLightType() != 0)
                {
                    litlamps++;
                }
            }
            if (lController.getCurrentLightType() != 0)
            {
                litlamps++;
            }
            return lController.adjacentSources.Length;
        }
        return 0;
    }

    /*
     * Used to get to the next lamp in sequence
     * As of rn, not sure if I want this, so obsolete for now
     */
    public GameObject checkLamps()
    {
        GameObject currLamp = findCurrentLamp();
        int litlamps = litLamps();
        lightSourceController lController = currLamp.GetComponentInParent<lightSourceController>();
        if (litlamps == 1 && lController.getCurrentLightType() != 0)
        {
            return null;
        }
        if (litlamps == 2)
        {
            GameObject nextLamp = null;
            foreach (GameObject lamp in lController.adjacentSources)
            {
                int lightType = lController.getCurrentLightType();
                if (lightType == 1 || lightType == 2)
                {
                    nextLamp = lamp;
                }
            }
            return nextLamp;
        }
        return null;
    }
}



