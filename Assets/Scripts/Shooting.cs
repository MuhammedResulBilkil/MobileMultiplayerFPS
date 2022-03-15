using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Camera _fpsCamera;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private float _damageAmount;

    [Header("Health Related Stuff")] 
    [SerializeField] private float _maxHealth;
    [SerializeField] private Image _healthBar;

    private PhotonView _photonView;
    private Animator _animator;
    private PlayerMovementController _playerMovementController;
    
    private float _currentHealth;
    private static readonly int Dead = Animator.StringToHash("Dead");

    private void Awake()
    {
        _playerMovementController = GetComponent<PlayerMovementController>();
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
        
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

            _photonView.RPC(nameof(CreateHitEffect), RpcTarget.All, raycastHit.point);

            PhotonView enemyPhotonView = raycastHit.collider.GetComponent<PhotonView>();
            if (raycastHit.collider.CompareTag("Player") || !enemyPhotonView.IsMine)
            {
                enemyPhotonView.RPC(nameof(TakeDamage), RpcTarget.AllBuffered, _damageAmount);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(float damage, PhotonMessageInfo photonMessageInfo)
    {
        _currentHealth -= damage;

        _healthBar.fillAmount = _currentHealth / _maxHealth;
        
        if (_currentHealth <= 0f)
        {
            Die();
            
            Debug.LogFormat($"{photonMessageInfo.Sender.NickName} killed {photonMessageInfo.photonView.Owner.NickName}");
        }
    }

    [PunRPC]
    public void CreateHitEffect(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(_hitEffect, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.5f);
    }

    private void Die()
    {
        if (_photonView.IsMine)
        {
            _animator.SetBool(Dead, true);

            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        _playerMovementController.enabled = false;
        
        TextMeshProUGUI respawnText = GameObject.Find("Text_Respawn").GetComponent<TextMeshProUGUI>();

        float respawnTime = 8.0f;

        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1f);
            respawnTime -= 1.0f;
            respawnText.text = $"You are Killed! Respawning at: {respawnTime.ToString((".00"))}";
        }
        
        _animator.SetBool(Dead, false);
        respawnText.text = "";

        int randomPoint = Random.Range(-20, 20);
        transform.position = new Vector3(randomPoint, 0f, randomPoint);
        _playerMovementController.enabled = true;
        
        _photonView.RPC(nameof(RegainHealth), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RegainHealth()
    {
        _currentHealth = _maxHealth;

        _healthBar.fillAmount = _currentHealth / _maxHealth;
    }
}
