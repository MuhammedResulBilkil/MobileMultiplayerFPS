using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerMovementController : MonoBehaviour
{
    public FixedTouchField fixedTouchField;
    public Joystick joystick;
    
    [SerializeField] private RigidbodyFirstPersonController _rigidbodyFirstPersonController;
    
    // Update is called once per frame
    void Update()
    {
        _rigidbodyFirstPersonController.joystickInputAxis.x = joystick.Horizontal;
        _rigidbodyFirstPersonController.joystickInputAxis.y = joystick.Vertical;

        _rigidbodyFirstPersonController.mouseLook.lookInputAxis = fixedTouchField.TouchDist;
    }
}
