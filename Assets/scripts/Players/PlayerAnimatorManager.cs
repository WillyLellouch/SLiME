///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///
///       Role: Manages Players Movements                                         ///
///       Authors:                                                                ///
///                                                                               ///
///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimatorManager : MonoBehaviourPun
{
    #region MonoBehaviour Callbacks

    public float Speed = 4 ;
    Animator animator;
    Vector3 move;
    float z;
    Rigidbody playerRigidbody;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        if (!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!animator)
        {
            return;
        }
        // Get input from default input method (Unity manages this)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
       
        Debug.Log("Avant" + h + "+" + v);

        movement(h,v);

    }

    /// <summary>
    ///  Control Movement for player
    /// </summary>
    /// <param name="horizontal">horizontal speed</param>
    /// <param name="vertical">vertical speed</param>
    void movement(float horizontal, float vertical)
    {
        if ((horizontal != 0f) || (vertical != 0f))
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        horizontal = horizontal /8f;
        vertical = vertical / 8f;

        move.Set(horizontal, 0f, vertical);

        Debug.Log("Après" + horizontal + "+" + vertical);

        playerRigidbody.MovePosition(transform.position + move);
    }


    #endregion
    
}