using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")] 
    [SerializeField] private TextMeshProUGUI _connectionStatusText;

    [Header("Login UI Panel")] 
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private GameObject _loginUIPanel;

    [Header("Game Options UI Panel")] 
    [SerializeField] private GameObject _gameOptionsUIPanel;
    
    [Header("Create Room UI Panel")] 
    [SerializeField] private GameObject _createRoomUIPanel;
    
    [Header("Inside Room UI Panel")] 
    [SerializeField] private GameObject _insideRoomUIPanel;

    [Header("Room List UI Panel")] 
    [SerializeField] private GameObject _roomListUIPanel;
    
    [Header("Join Random Room UI Panel")] 
    [SerializeField] private GameObject _joinRandomRoomUIPanel;

    private List<GameObject> _panels = new List<GameObject>();

    private void Awake()
    {
        _panels.Add(_loginUIPanel);
        _panels.Add(_gameOptionsUIPanel);
        _panels.Add(_createRoomUIPanel);
        _panels.Add(_insideRoomUIPanel);
        _panels.Add(_roomListUIPanel);
        _panels.Add(_joinRandomRoomUIPanel);
    }

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(_loginUIPanel);
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
        
        ActivatePanel(_gameOptionsUIPanel);
    }

    #endregion

    #region Public Methods

    public void ActivatePanel(GameObject panelToBeActivated)
    {
        string panelToBeActivatedName = panelToBeActivated.name;

        foreach (GameObject panel in _panels)
            panel.SetActive(panelToBeActivatedName.Equals(panel.name));
        
    }

    #endregion
    
}
