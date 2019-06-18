///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///
///       Role: Manages Enemy life                                                ///
///       Authors:                                                                ///
///                                                                               ///
///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemyPv : MonoBehaviour
{
    public int Pv = 10;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touche :" + other.gameObject.name);
       
    }
}
