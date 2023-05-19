using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FloatingJoystick _joystick;
    //[SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private string currentAnimName;

    //private bool isGrounded = true;
    //private bool isFall = false;

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);
    }

    protected void ChangeAnimation(string animName)
    {
        if(currentAnimName != animName)
        {
            _animator.ResetTrigger(animName);
            currentAnimName = animName;
            _animator.SetTrigger(currentAnimName);
        }
    }
}
