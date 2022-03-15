using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private RigidbodyFirstPersonController _rigidbodyFirstPersonController;

    public FixedTouchField fixedTouchField;
    public Joystick joystick;

    private Animator _animator;
    
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Running = Animator.StringToHash("Running");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbodyFirstPersonController.joystickInputAxis.x = joystick.Horizontal;
        _rigidbodyFirstPersonController.joystickInputAxis.y = joystick.Vertical;

        _rigidbodyFirstPersonController.mouseLook.lookInputAxis = fixedTouchField.TouchDist;

        _animator.SetFloat(Horizontal, joystick.Horizontal);
        _animator.SetFloat(Vertical, joystick.Vertical);

        if (Mathf.Abs(joystick.Horizontal) > 0.7f || Mathf.Abs(joystick.Vertical) > 0.7f)
        {
            //Running
            _rigidbodyFirstPersonController.movementSettings.ForwardSpeed = 16f;
            _animator.SetBool(Running, true);
        }
        else
        {
            //Walking
            _rigidbodyFirstPersonController.movementSettings.ForwardSpeed = 8f;
            _animator.SetBool(Running, false);
        }
    }
}
