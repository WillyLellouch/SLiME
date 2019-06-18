///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///
///       Role: Manages Enemy behaviour                                           ///
///       Authors:                                                                ///
///                                                                               ///
///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    GameObject[] players;               // Reference to the player's position.
    NavMeshAgent nav;               // Reference to the nav mesh agent.

    Transform target = null;
    private float time = 0.0f;
    public float timer = 0.1f;
    public int pv = 9;

    public AudioClip enemyHit;
    public AudioClip playerHit;

    private AudioSource enemySrc;


    void Awake ()
    {
        // Set up the references.
        players = GameObject.FindGameObjectsWithTag("Player");
        nav = GetComponent <NavMeshAgent> ();
    }

    private void Start()
    {
        enemySrc = GetComponent<AudioSource>();

        Debug.Log("Etat de la source" + enemySrc);

        enemySrc.clip = playerHit;
    }


    void Update ()
    {

        // If the enemy and the player have health left...
       if(pv<= 0)
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
                    if (dist < minDist)
                    {
                        target = t;
                        minDist = dist;
                    }                
                }
            }

        if (target != null)
        { 
            nav.SetDestination(target.position);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
       
        // Check if the class of the game object is spit
        if(other.gameObject.GetComponent<Spit>())
        {
            enemySrc.clip = enemyHit;
            enemySrc.Play();
            pv -= other.gameObject.GetComponent<Spit>().damage;
        }

        if(other.gameObject.GetComponent<PlayerManager>())
        {
            enemySrc.clip = playerHit;
            enemySrc.Play();
        }
    }
}
