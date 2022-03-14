using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> _fpsHandsChildGameObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> _soldierChildGameObjects = new List<GameObject>();

    private void Start()
    {
        if (photonView.IsMine && PhotonNetwork.IsConnected)
        {
            //Activate FPS Hands, Deactivate Soldier
            foreach (GameObject fpsHandsChildGameObject in _fpsHandsChildGameObjects)
                fpsHandsChildGameObject.SetActive(true);

            foreach (GameObject soldierChildGameObject in _soldierChildGameObjects)
                soldierChildGameObject.SetActive(false);
        }
        else
        {
            //Activate Soldier, Deactivate FPS Hands
            foreach (GameObject fpsHandsChildGameObject in _fpsHandsChildGameObjects)
                fpsHandsChildGameObject.SetActive(false);

            foreach (GameObject soldierChildGameObject in _soldierChildGameObjects)
                soldierChildGameObject.SetActive(true);
        }
    }
}
