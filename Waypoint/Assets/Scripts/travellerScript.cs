using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.AI;

public class travellerScript : MonoBehaviour


{


    public Text winText;
    //public Transform lamp;
    public float speed = 200f;
    bool hasTarget;
    Vector3 target;
    GameObject lastVisited;
    private Rigidbody rb;

    public Transform goal;

    private NavMeshAgent agent;

    public float lightValue;
    private MeshRenderer[] meshRenderers;

    private MeshRenderer meshRendererTraveller;
    private Material cloak;
    private Material hat;

   
    // Use this for initialization
    void Start()
    {
        
        hasTarget = false;
        winText.text = "";
        //rb  = GetComponent<Rigidbody>();
       // if (rb == null)
        //Debug.Log("Could not find traveller rb");

        agent = GetComponent<NavMeshAgent>();
        //agent.destination = goal.position; 
        
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        if (meshRenderers == null) {
            Debug.Log("could not find meshrenderers");
        }

        foreach (MeshRenderer m in meshRenderers){
            if (m != GetComponent<MeshRenderer>()){
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
            if (m.name.Replace("(Instance)","").CompareTo("TravellerCloak") == 1) {//TravellerCloak
                cloak = m;
        
            }

            if (m.name.Replace("(Instance)","").CompareTo("TravellerHat") == 1) {//TravellerCloak
                hat = m;
        
            }
            

        }


    }
    void Update() {

        //Testing emmisive cloak material

        //DynamicGI.SetEmissive(cloak, new Color(255f, 255f, 255f, 1.0f) * lightValue);
         if (Input.GetKey(KeyCode.J)) {
            lightValue -= 0.0001f;
        }
        if (Input.GetKey(KeyCode.K)) {
            lightValue += 0.0001f;
        }
        lightValue = Mathf.Clamp(lightValue, -0.002f,  0.005f);
        cloak.SetColor("_EmissionColor", new Color(255f,255f,255f,1.0f) * lightValue);
        hat.SetColor("_EmissionColor", new Color(255f,255f,255f,1.0f) * lightValue);

       
        
    }

    // Update is called once per frame
    void FixedUpdate()


    {

    
        if (hasTarget && Vector3.Distance(transform.position, target) > 1f) {
            //Debug.Log(Vector3.Distance(transform.position, target));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), speed * Time.deltaTime);
            //transform.position += transform.forward * speed * Time.deltaTime;
          //  rb.mov
            //transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }


        else {
            hasTarget = false;
            //Debug.Log("Reached");
            //target = transform.position;
        }
   
    }

     private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            winText.text = "WIN!";
        }
       

    }
    public void setTarget(Transform goal) {
        if (Vector3.Distance(transform.position, goal.position) <= 6f) {
            
            target = goal.position;
            hasTarget = true;
            //Debug.Log(goal.position);
            //agent.SetDestination(goal.position);
        }
    }
}
