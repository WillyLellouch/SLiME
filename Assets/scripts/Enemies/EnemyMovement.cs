using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    GameObject[] players;               // Reference to the player's position.
    //EnemyHealth enemyHealth;        // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.

    Transform target = null;
    private float time = 0.0f;
    public float timer = 0.1f;


    void Awake ()
    {
        // Set up the references.
        players = GameObject.FindGameObjectsWithTag("Player");
        //enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }


    void Update ()
    {
        // If the enemy and the player have health left...
        //if(enemyHealth.currentHealth > 0)
        //{
            // ... set the destination of the nav mesh agent to the player.
            time += Time.deltaTime;

            if (time >= timer){
              time = 0.0f;
            Vector3 position = nav.transform.position;
            float minDist = Mathf.Infinity;
            foreach (GameObject g in players){
              Transform t = g.transform;
              float dist = Vector3.Distance(t.position, position);
              if (dist < minDist){
                target = t;
                minDist = dist;
              }
            }
          }
            nav.SetDestination (target.position);
        /*}
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        }*/
    }
}
