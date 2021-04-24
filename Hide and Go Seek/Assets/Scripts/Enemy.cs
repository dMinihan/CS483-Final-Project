using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 
using UnityEditor.AI; 

/* This class heavily relies on Unity's NavMeshAgent and NavMeshSurface classes.  
 * The NavMeshAgents are set to wander randomly around the navigation mesh until 
 * one of them gets close to the player, then they all go towards the player. 
 * If the player escapes, they resume wandering. 
 */ 

public class Enemy : MonoBehaviour
{
	public static NavMeshSurface surface; 


	public NavMeshAgent agent; 
    float agentSpeed = 6.5f; 


    public bool locked = false; 
	public GameObject player; 

    void Start() {
    	addNavigation();
        addProperties();
        // Sets the initial target destination. 
        navDestination(agent.transform.position, 400, -1);

    }


    // Update is called once per frame
    public void setTarget()
    {
        // If locked == true, the agent is tracking the player's position. 
        if(locked == true) {
            if(player is null) {
                player = GameObject.Find("Main Camera"); 
            }
            agent.SetDestination(player.transform.position);
            startLights();
        }
        
        /* If it is not true and the agent is 2f or closer to its destination, 
         * the agent gets a new random destination. 
         */ 
        else if(Vector3.Distance(agent.transform.position, agent.destination) <= 2) { 
            navDestination(agent.transform.position, 400, -1);
            disableLights();
        }
    }

    // Initializes the NavMeshSurface, NavMeshAgent, player reference, and the agent's speed. 
    public void addNavigation() {
        surface = GameObject.Find("navmesh").GetComponent<NavMeshSurface>();  
        agent = this.gameObject.AddComponent<NavMeshAgent>(); 
        agent.speed = agentSpeed; 
        player = GameObject.Find("Main Camera");
    }


    public void addProperties() {
        this.gameObject.AddComponent<Rigidbody>(); 
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        }

    // Enables the agent's red light. 
    public void startLights() {
        if(gameObject.GetComponent<Light>() == null) {
            this.gameObject.AddComponent<Light>(); 
            this.gameObject.GetComponent<Light>().color = Color.red; 
            this.gameObject.GetComponent<Light>().intensity = 6;
        }
    }

     // Disable the agent's red light. 
     public void disableLights() {
        if(gameObject.GetComponent<Light>() != null) {
            Destroy(this.gameObject.GetComponent<Light>()); 
        }
    }

     // Sets the agent's destination to a random point on the NavMesh.  
     public void navDestination(Vector3 origin, float distance, int layermask) {
                            
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           
        randomDirection += origin;
           
        NavMeshHit navHit;
           
        NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);

        agent.destination = navHit.position; 
    }

}
