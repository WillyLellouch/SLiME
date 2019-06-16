using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    GameObject[] players;               // Reference to the player's position.
    //EnemyHealth enemyHealth;        // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.

    public AudioClip playerHited;
    public AudioClip enemyHited;
    public AudioSource musicSource;

    Transform target = null;
    private float time = 0.0f;
    public float timer = 0.1f;
    public int Pv = 9;

    void Awake ()
    {
        // Set up the references.
        players = GameObject.FindGameObjectsWithTag("Player");
        //enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }

    void Start()
    {
        musicSource.clip = playerHited;   
    }


    void Update ()
    {
        // If the enemy and the player have health left...
       if(Pv<= 0)
        {
            Destroy(gameObject);
        }
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
        if (target != null)
        { 
            nav.SetDestination(target.position);
        }

        /*}
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        }*/
    }

    public void OnTriggerEnter(Collider other)
    {
        musicSource.clip = enemyHited;
        // Check if the class of the game object is spit
        if(other.gameObject.GetComponent<Spit>())
        {
            musicSource.Play();
            Pv -= other.gameObject.GetComponent<Spit>().damage;
        }

        if(other.gameObject.GetComponent<PlayerManager>())
        {
            musicSource.Play();
        }
    }
}
