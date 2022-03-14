using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobileFPSGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    
    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (_playerPrefab != null)
            {
                int randomPoint = Random.Range(-10, 10);
                PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(randomPoint, 0f, randomPoint),
                    Quaternion.identity);
            }
            else
            {
                Debug.LogError("Player Prefab is null!!!");
            }
        }
    }
}
