using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Player name Input field. Let the user input his name, will appear above the player in the game.
[RequireComponent(typeof(InputField))]

public class PlayerNameInputField : MonoBehaviour {

    #region Private Variables

    //Store the PlayerPref Key to avoid typos
    static string PlayerNamePrefKey = "PlayerName";

    #endregion

    #region Monobehaviour Callbacks

    // Use this for initialization
    void Start ()
    {
        string defaultName = "";
        InputField _inputField = this.GetComponent<InputField>();
        if(_inputField != null)
        {
            if (PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.playerName = defaultName;
	}

    #endregion

    #region Public Methods

    //Sets the name of the player and save it in the PlayerPrefs for future sessions.
    //<param name = "value"> the name of the player </param>

    public void SetPlayerName(string value)
    {
        //#Important
        PhotonNetwork.playerName = value + " ";
        PlayerPrefs.SetString(PlayerNamePrefKey, value);
    }

    #endregion
}
