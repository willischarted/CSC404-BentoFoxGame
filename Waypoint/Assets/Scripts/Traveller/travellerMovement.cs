using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class travellerMovement : MonoBehaviour
{

    public float speed = 200f;
    Animator anim;
    Vector3 target;
    NavMeshAgent nav;

    Transform player;

    travellerHealth travellerHealth;


    // Use this for initialization
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        travellerHealth = GetComponent<travellerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(player.position);
    }
}
