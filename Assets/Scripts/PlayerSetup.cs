using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerUIPrefab;
    [SerializeField] private GameObject _cameraGameObject;
    
    [SerializeField] private List<GameObject> _fpsHandsChildGameObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> _soldierChildGameObjects = new List<GameObject>();

    private PlayerMovementController _playerMovementController;
    private RigidbodyFirstPersonController _rigidbodyFirstPersonController;

    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _rigidbodyFirstPersonController = GetComponent<RigidbodyFirstPersonController>();
    }

    private void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            //Activate FPS Hands, Deactivate Soldier
            foreach (GameObject fpsHandsChildGameObject in _fpsHandsChildGameObjects)
                fpsHandsChildGameObject.SetActive(true);

            foreach (GameObject soldierChildGameObject in _soldierChildGameObjects)
                soldierChildGameObject.SetActive(false);
            
            //Instantiate Player UI
            GameObject playerUIGameObject = Instantiate(_playerUIPrefab);

            _playerMovementController.joystick = playerUIGameObject.transform.Find("Fixed Joystick").GetComponent<Joystick>();
            _playerMovementController.fixedTouchField = playerUIGameObject.transform.Find("RotationTouchField").GetComponent<FixedTouchField>();
            _cameraGameObject.SetActive(true);
        }
        else
        {
            //Activate Soldier, Deactivate FPS Hands
            foreach (GameObject fpsHandsChildGameObject in _fpsHandsChildGameObjects)
                fpsHandsChildGameObject.SetActive(false);

            foreach (GameObject soldierChildGameObject in _soldierChildGameObjects)
                soldierChildGameObject.SetActive(true);

            _playerMovementController.enabled = false;
            _rigidbodyFirstPersonController.enabled = false;
            _cameraGameObject.SetActive(false);
        }
    }
}
