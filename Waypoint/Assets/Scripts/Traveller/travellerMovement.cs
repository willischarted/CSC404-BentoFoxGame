using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class travellerMovement : MonoBehaviour
{

    public float speed = 200f;
    Animator anim;
    NavMeshAgent nav;
    Transform startPoint;
    Transform target;

    travellerHealth travellerHealth;


    // Use this for initialization
    void Awake()
    {
        startPoint = GameObject.FindGameObjectWithTag("StartArea").transform;
        target = startPoint;
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        travellerHealth = GetComponent<travellerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        FindTarget();
        nav.SetDestination(target.position);
        Animating();
    }
      
    void FindTarget(){
        
    }

    void Animating(){
        
    }
}
