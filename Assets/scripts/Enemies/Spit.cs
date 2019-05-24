using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviourPun
{
    public float speed = 20f;

    public int damage = 3;

    public float lifecycle = 1f;

    private float despawnTime;

    private Rigidbody spitRigidBody;

    private SphereCollider spitCollider;

    [HideInInspector]
    public GameObject owner;

    void Awake()
    {
        spitRigidBody = GetComponent<Rigidbody>();
        spitCollider = GetComponent<SphereCollider>();
        OnSpawn();
    }

    void OnSpawn()
    {
        spitRigidBody.velocity = speed * transform.forward;
        if(lifecycle > 0)
        {
            despawnTime = Time.time + lifecycle;
        }
        else
        {
            despawn();
        }
    }

    void Update()
    {
        if(Time.time > despawnTime)
        {
            despawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
      despawn();
    }

    void despawn()
    {
        Destroy(this.gameObject);
    }
}
