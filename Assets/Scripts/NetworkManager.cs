using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_InputField _maxPlayerInputField;
    
    [Header("Inside Room UI Panel")] 
    [SerializeField] private GameObject _insideRoomUIPanel;

    [Header("Room List UI Panel")] 
    [SerializeField] private GameObject _roomListUIPanel;
    
    [Header("Join Random Room UI Panel")] 
    [SerializeField] private GameObject _joinRandomRoomUIPanel;

    private List<GameObject> _panels = new List<GameObject>();
    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();

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

    public void OnCreateRoomButtonClicked()
    {
        string roomName = _roomNameInputField.text;

        if (string.IsNullOrEmpty(roomName))
            roomName = $"Room {Random.Range(0, 100000)}";
        
        byte maxPlayerCount = (byte)int.Parse(_maxPlayerInputField.text);

        // Just In Case
        if (maxPlayerCount == 0)
            maxPlayerCount = 1;
        
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayerCount;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(_gameOptionsUIPanel);
    }

    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        
        ActivatePanel(_roomListUIPanel);
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

    public override void OnCreatedRoom()
    {
        Debug.LogFormat($"{PhotonNetwork.CurrentRoom.Name} is created!");
    }

    public override void OnJoinedRoom()
    {
        Debug.LogFormat($"{PhotonNetwork.LocalPlayer.NickName} joined to {PhotonNetwork.CurrentRoom.Name}!");
        
        ActivatePanel(_insideRoomUIPanel);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (!roomInfo.IsOpen || !roomInfo.IsVisible || roomInfo.RemovedFromList)
                if (_cachedRoomList.ContainsKey(roomInfo.Name))
                    _cachedRoomList.Remove(roomInfo.Name);
            
            //_cachedRoomList[roomInfo.Name] = roomInfo;
            _cachedRoomList.Add(roomInfo.Name, roomInfo);
            
            Debug.LogFormat($"In {PhotonNetwork.CurrentLobby.Name} Lobby - Room Name = {roomInfo.Name}");
        }
           
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
