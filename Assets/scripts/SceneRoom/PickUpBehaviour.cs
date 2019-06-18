///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///
///       Role: Manages Bonus Pickup animation                                    ///
///       Authors: Willy Lellouch                                                 ///
///                                                                               ///
///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate (new Vector3 (33, 0, 45) * Time.deltaTime);
    }
}
