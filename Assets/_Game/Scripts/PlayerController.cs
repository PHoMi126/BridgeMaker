using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum AnimType
    {
        Idle, Run, Falling
    }
    
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] Transform brickTransform;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private AnimType currentAnimName = AnimType.Idle;
    private List<GameObject> listBrickHave = new List<GameObject>();
    public BrickController.BrickType brickType = BrickController.BrickType.RED;

    private void FixedUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            ChangeAnimation(AnimType.Run);
            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            ChangeAnimation(AnimType.Idle);
            _rigidbody.velocity = Vector3.zero;

        }
    }

    public void ChangeAnimation(AnimType _type)
    {
        if(currentAnimName != _type)
        {
            currentAnimName = _type;
            switch(_type)
            {
                case AnimType.Idle:
                    _animator.SetTrigger("isIdle");
                    break;
                case AnimType.Run:
                    _animator.SetTrigger("isRun");
                    break;
                case AnimType.Falling:
                    _animator.SetTrigger("isFalling");
                    break;
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        BrickController brick = other.GetComponent<BrickController>();

        if(brick != null && brick.brickType == this.brickType)
        {
            brick.BrickEaten();
            GameObject obj = Instantiate(brickPrefab, brickTransform);
            obj.GetComponent<BrickController>().enabled = false;

            obj.transform.localPosition = new Vector3(0f, listBrickHave.Count * 0.3f, 0f);
            listBrickHave.Add(obj);
        }
    }
}
