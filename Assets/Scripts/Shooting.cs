using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Camera _fpsCamera;
    [SerializeField] private GameObject _hitEffect;

    public void Fire()
    {
        Ray ray = _fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            Debug.Log(raycastHit.collider.name);

            GameObject hitEffectGameObject = Instantiate(_hitEffect, raycastHit.point, Quaternion.identity);
            Destroy(hitEffectGameObject, 0.5f);
        }
    }
}
