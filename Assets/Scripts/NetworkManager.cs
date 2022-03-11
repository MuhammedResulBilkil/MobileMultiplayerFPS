using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")] [SerializeField]
    private TextMeshProUGUI _connectionStatusText;

    [Header("Login UI Panel")] [SerializeField]
    private TMP_InputField _playerNameInput;

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _connectionStatusText.text = $"Connection Status: {PhotonNetwork.NetworkClientState}";
    }

    #endregion

    #region UI CallBacks

    public void OnLoginButtonClicked()
    {
        string playerName = _playerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogErrorFormat($"Player Name is Invalid!!!");
        }
    }

    #endregion

    #region Photon CallBacks

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet!");
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogFormat($"{PhotonNetwork.LocalPlayer.NickName} is connected to Photon Server!");
    }

    #endregion
    
}
