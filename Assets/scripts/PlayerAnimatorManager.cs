using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : Photon.MonoBehaviour{



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }
	}
}
