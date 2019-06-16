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
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.Health = (int)stream.ReceiveNext();
        }
    }

    #endregion


    #region Private Fields

    [SerializeField]
    public GameObject projectile;

    //True, when the user is firing
    bool IsFiring;
    bool dead;
    float fireRate = 0.33f;
    float nextFire;

    #endregion


    #region Public fields

    [Tooltip("The current Health of our player")]
    public int Health = 10;

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
            if (GameObject.FindGameObjectWithTag("Health Slider"))
            {
                healthSlider = (Slider)FindObjectOfType(typeof(Slider));
            }

            DeadText.SetActive(false);
        }

    }

    //shoots a bullet in the direction passed in
            //we do not rely on the current turret rotation here, because we send the direction
            //along with the shot request to the server to absolutely ensure a synced shot position
            protected void Shoot(Vector2 direction = default(Vector2))
            {
              /*GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
              bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);*/
                //if shot delay is over
                if (Time.time > nextFire)
                {
                    //set next shot timestamp
                    nextFire = Time.time + fireRate;


                    Vector3 playerPos = transform.position + new Vector3(0,1,0);
                    Vector3 playerDirection = transform.forward;
                    Quaternion playerRotation = transform.rotation;
                    float spawnDistance = 2;

                    Vector3 spawnPos = playerPos + playerDirection*spawnDistance;
            
                    GameObject bullet = Instantiate(projectile, spawnPos, Quaternion.identity) as GameObject;
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

        if (Health <= 0)
        {
            // TODO : replace with "ghost mode ?"
            //GameManager.Instance.LeaveRoom();
            dead = true;
        }
        else if (Health > 0)
        {
            dead = false;
            DeadText.SetActive(false);
        }
        if (IsFiring){
            playerSrc.clip = spitSnd;
            playerSrc.Play();
            Shoot();

        }
        // if (beams != null && IsFiring != beams.activeSelf)
        // {
        //     beams.SetActive(IsFiring);
        // }

        if (dead == true)
        {
            DeadText.SetActive(true);
        }
    }

    /// MonoBehaviour method called when the Collider 'other' enters the trigger.
    /// Affect Health of the Player if the collider is a beam
    /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
    /// One could move the collider further away to prevent this or check if the beam belongs to the player.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (!photonView.IsMine)
        {
            return;
        }
        // Picking up health
        if (other.gameObject.CompareTag("Health Pickup")) {
            Health += 1;
            other.gameObject.SetActive(false);

            healthSlider.value = Health;
        }
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Pers 1 de vie");
            Health -= 1;
            healthSlider.value = Health;
        }

    }
    /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
    /// We're going to affect health while the beams are touching the player
    /// <param name="other">Other.</param>
    /*void OnTriggerStay(Collider other)
    {
        // we dont' do anything if we are not the local player.
        if (!photonView.IsMine)
        {
            return;
        }
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Enemy"))
        {
            return;
        }
        // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
        Health -= 1f * Time.deltaTime;
    }*/

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
