using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Camera _fpsCamera;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private float _damageAmount;

    [Header("Health Related Stuff")] 
    [SerializeField] private float _maxHealth;
    [SerializeField] private Image _healthBar;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;

        _healthBar.fillAmount = _currentHealth / _maxHealth;
    }

    public void Fire()
    {
        Ray ray = _fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            Debug.Log(raycastHit.collider.name);

            GameObject hitEffectGameObject = Instantiate(_hitEffect, raycastHit.point, Quaternion.identity);
            Destroy(hitEffectGameObject, 0.5f);

            PhotonView enemyPhotonView = raycastHit.collider.GetComponent<PhotonView>();
            if (raycastHit.collider.CompareTag("Player") || !enemyPhotonView.IsMine)
            {
                enemyPhotonView.RPC(nameof(TakeDamage), RpcTarget.AllBuffered, _damageAmount);
            }
            
            
        }
    }

    public void TakeDamage(float damage, PhotonMessageInfo photonMessageInfo)
    {
        _currentHealth -= damage;

        _healthBar.fillAmount = _currentHealth / _maxHealth;
        
        if (_currentHealth <= 0f)
        {
            //Die();
            
            Debug.LogFormat($"{photonMessageInfo.Sender.NickName} killed {photonMessageInfo.photonView.Owner.NickName}");
        }
        
    }
}
