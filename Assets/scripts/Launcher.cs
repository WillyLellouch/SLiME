using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Photon.PunBehaviour {

    #region Public variables
    
    // The PUN loglevel.
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    
    // The maximum numbers of players per room.When a room is full, it can't be joined by new players and so new room will be created.
    [Tooltip(" The maximum numbers of players per room.When a room is full, it can't be joined by new players and so new room will be created.")]
    public byte MaxPlayersPerRoom = 4;

    #endregion

    #region Private variables
    
    // This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
    string _gameVersion = "1";

    //<NEW>
    //Keep track of the current process. Since connection is asynchronous and is based on several callback from Photon, 
    //we need to keep track of this to properly adjust the behavior when we receive back by Photon
    //Typically this is used for the OnConnectedToMaster() callback.
    bool isConnected;
    //<\NEW>

    #endregion

    #region Public properties

    [Tooltip("The UI panel to let the user enter Name, connect and play")]
    public GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    public GameObject progressLabel;

    #endregion

    #region MonoBehavior Callbacks
    
    // MonoBehavior method called on GameObject by Unity during early intialization phase.
    void Awake()
    {
        //#Critical
        //we don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = false;

        //#Critical
        //this make sure we can use PhotonNetwork.LoadLevel() on the master client and all clients on the same room sync their level automatically.
        PhotonNetwork.automaticallySyncScene = true;

        //#NotImportant
        //Force Loglevel
        PhotonNetwork.logLevel = Loglevel;
    }
    
    // MonoBehavior method called on GameObject by Unity during early intialization phase.
    void Start ()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
	}

    #endregion

    #region Photon.PunBehavior CallBacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("DemoAnimator/Launcher : OnConnectedToMaster() was called by PUN");

        //<NEW>
        //we don't want to do anything if we are not attempting to join a room.
        //this case chere isConnected is false is typically when you lose or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        //we don't want to do anything
        if (isConnected)
        {
            //#Critical : The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnPhotonRandomJointFailed()
            PhotonNetwork.JoinRandomRoom();
        }
        //<\NEW>
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("DemoAnimator/Launcher : OnDisconnectedFromPhoton() was called by PUN");
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("DemoAnimator/Launcher : OnPhotonRandomJointFailed() was called by PUN. No random room available, so we create one. \nCalling : PhotonNetwork.CreateRoom(null, new RoomOptions(){MaxPlayers=4}, null);");
        //#Critical : we fail to join a random room, maybe none exist or they are all full. No worries we create a room.
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom}, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("DemoAnimator/Launcher : OnJoinedRoom() was called by PUN. Now this client is in a room");

        //#Critical
        //Load the room level
        PhotonNetwork.LoadLevel("EmptyRoom");
    }

    #endregion

    #region Public Methods
    
    // Start the connection process
    // - if already connected we attempt joining a random room
    // - if not yet connected, connect this application instance to Photon Cloud Network

    public void Connect()
    {
        //<NEW>
        //keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        isConnected = true;
        //</NEW>

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.connected)
        {
            //#Critical we need at this point to attempt joining a random room. If it fails we will get notified in OnPhotonRandomJoinFailed() and we will create one
            PhotonNetwork.JoinRandomRoom();
            //TODO : join the first level
        }
        else
        {
            //#Critical we must first and foremost connect to photon online server.
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    #endregion
}
