///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///
///       Role: Manages Players Functionalities                                   ///
///       Authors: Willy Lellouch                                                 ///
///                                                                               ///
///:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region IPunObservable implementation

    public AudioClip spitSnd;

    private AudioSource playerSrc;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(isFiring);
            stream.SendNext(health);
        }
        else
        {
            // Network player, receive data
            this.isFiring = (bool)stream.ReceiveNext();
            this.health = (int)stream.ReceiveNext();
        }
    }

    #endregion


    #region Private Fields

    [SerializeField]
    public GameObject projectile;

    //True, when the user is firing
    bool isFiring;
    bool dead;
    float fireRate = 0.33f;
    float nextFire;

    #endregion


    #region Public fields

    [Tooltip("The current health of our player")]
    public int health = 10;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public Slider healthSlider;
    public GameObject DeadText;

    #endregion

    #region MonoBehaviour CallBacks

    void Start()
    {
        playerSrc = GetComponent<AudioSource>();
        playerSrc.clip = spitSnd;

      nextFire = Time.time;
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();



        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }

        if (photonView.IsMine)
        {
            if (GameObject.FindGameObjectWithTag("health Slider"))
            {
                healthSlider = (Slider)FindObjectOfType(typeof(Slider));
            }

            DeadText.SetActive(false);
        }

    }

            //shoots a bullet in the direction passed in
    protected void Shoot(Vector2 direction = default(Vector2))
    {
              
        //if shot delay is over
        if (Time.time > nextFire)
        {
            //set next shot timestamp
            nextFire = Time.time + fireRate;

            // Generates shot
            Vector3 playerPos = transform.position + new Vector3(0,1,0);
            Vector3 playerDirection = transform.forward;
            Quaternion playerRotation = transform.rotation;
            float spawnDistance = 2;

            Vector3 spawnPos = playerPos + playerDirection*spawnDistance;
                    
            //Instanciates projectile
            GameObject bullet = PhotonNetwork.Instantiate(projectile.name, spawnPos, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
        }
    }

    void Awake()
    {

        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            ProcessInputs();
        }

        if (health <= 0)
        {
            // TODO : replace with "ghost mode ?"
            //GameManager.Instance.LeaveRoom();
            dead = true;
        }
        else if (health > 0)
        {
            dead = false;
            DeadText.SetActive(false);
        }
        if (isFiring){
            playerSrc.clip = spitSnd;
            playerSrc.Play();
            Shoot();

        }
        if (dead == true)
        {
            DeadText.SetActive(true);
        }
    }

    /// MonoBehaviour method called when the Collider 'other' enters the trigger.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (!photonView.IsMine)
        {
            return;
        }
        // Picking up health
        if (other.gameObject.CompareTag("health Pickup")) {
            health += 1;
            other.gameObject.SetActive(false);

            healthSlider.value = health;
        }
        // Damage by Ennemies
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Pers 1 de vie");
            health -= 1;
            healthSlider.value = health;
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
            if (!isFiring)
            {
                isFiring = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (isFiring)
            {
                isFiring = false;
            }
        }
    }

    #endregion
}
