using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Photon.MonoBehaviour
{
    #region Public variables

    [Tooltip("The Beams GameObject to control")]
    public GameObject Beams;

    #endregion

    #region Private variables

    bool IsFiring;

    #endregion

    #region MonoBehaviour CallBacks

    private void Awake()
    {
        if (Beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            Beams.SetActive(false);
        }
    }

    // Use this for initialization
    void Start ()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

        if (_cameraWork != null)
        {
            if (photonView.isMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.Log("<Color=Red><a>Missing</a></Color> CameraWork component on playerPrefab.", this);
        }
    }

	// Update is called once per frame
  void Update()
        {
            ProcessInputs ();

            // trigger Beams active state
            if (Beams != null && IsFiring != Beams.GetActive())
            {
                Beams.SetActive(IsFiring);
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {

            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }
        #endregion


}
