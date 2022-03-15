using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerMovementController : MonoBehaviour
{
    public FixedTouchField fixedTouchField;
    public Joystick joystick;
    
    private RigidbodyFirstPersonController _rigidbodyFirstPersonController;

    private void Awake()
    {
        _rigidbodyFirstPersonController = GetComponent<RigidbodyFirstPersonController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbodyFirstPersonController.joystickInputAxis.x = joystick.Horizontal;
        _rigidbodyFirstPersonController.joystickInputAxis.y = joystick.Vertical;

        _rigidbodyFirstPersonController.mouseLook.lookInputAxis = fixedTouchField.TouchDist;
    }
}
