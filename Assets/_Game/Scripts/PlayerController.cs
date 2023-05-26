using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum AnimType
    {
        Idle, Running, Dance
    }
    
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private Transform _brickTransform;
    [SerializeField] private Material _material;

    [SerializeField] private float _moveSpeed;

    private AnimType currentAnimName = AnimType.Idle;
    private List<GameObject> listBrickHave = new List<GameObject>();
    public BrickController.BrickType brickType = BrickController.BrickType.RED;
    GameObject obj;

    private void FixedUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            ChangeAnimation(AnimType.Running);
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
                case AnimType.Running:
                    _animator.SetTrigger("isRunning");
                    break;
                case AnimType.Dance:
                    _animator.SetTrigger("isDance");
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
            obj = Instantiate(_brickPrefab, _brickTransform);
            obj.GetComponent<BrickController>().enabled = false;

            obj.transform.localPosition = new Vector3(0f, listBrickHave.Count * 0.15f, 0f);
            listBrickHave.Add(obj);
        }
        else if(other.gameObject.tag == "BridgeTile")
        {
            //Change Brick Color
            other.GetComponent<Renderer>().material = _material;

            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            listBrickHave.Remove(obj);
            Destroy(obj);

            Debug.Log(listBrickHave.Count);
        }
        else if (other.gameObject.tag == "BridgeWall")
        {
            if (listBrickHave == null)
            {
                other.isTrigger = false;
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.tag == "DeathZone")
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BridgeWall")
        {
            other.isTrigger = true;
        }
    }
}
