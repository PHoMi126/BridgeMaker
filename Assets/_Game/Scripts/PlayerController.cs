using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private Transform _brickTransform;
    [SerializeField] private Material _material;
    [SerializeField] private Camera _winCamera;
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private float _moveSpeed;

    private float horizontal;
    private float vertical;
    private Vector3 direction;

    public enum AnimType
    {
        Idle, Running, Dance
    }

    public WinLoose winLooseScript;
    private AnimType currentAnimName = AnimType.Idle;
    public List<GameObject> listBrickHave = new List<GameObject>();
    public BrickController.BrickType brickType = BrickController.BrickType.RED;
    GameObject obj;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Moving();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Stopping();
        }
    }

    public void Moving()
    {
        if (_joystick != null)
        {
            horizontal = _joystick.Horizontal;
            vertical = _joystick.Vertical;
            if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            {
                ChangeAnimation(AnimType.Running);
                direction = new Vector3(horizontal, 0f, vertical);
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.deltaTime);
                transform.position += _moveSpeed * Time.deltaTime * direction;
            }
        }
    }

    public void Stopping()
    {
        ChangeAnimation(AnimType.Idle);
        _rigidbody.velocity = Vector3.zero;
    }

    public void ChangeAnimation(AnimType _type)
    {
        if (currentAnimName != _type)
        {
            currentAnimName = _type;
            switch (_type)
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

        if (brick != null && brick.brickType == this.brickType)
        {
            brick.BrickEaten();
            obj = Instantiate(_brickPrefab, _brickTransform);
            obj.GetComponent<BrickController>().enabled = false;

            obj.transform.localPosition = new Vector3(0f, listBrickHave.Count * 0.15f, 0f);
            listBrickHave.Add(obj);
        }
        else if (other.gameObject.CompareTag("BridgeTile"))
        {
            //Change Brick Color
            other.GetComponent<Renderer>().material = _material;

            other.gameObject.GetComponent<MeshRenderer>().enabled = true;

            //Debug.Log(listBrickHave.Count);
        }
        else if (other.gameObject.CompareTag("BridgeWall"))
        {
            if (listBrickHave.Count == 0)
            {
                other.isTrigger = false;
            }
            else
            {
                obj = listBrickHave[listBrickHave.Count - 1];
                Destroy(obj);
                listBrickHave.Remove(obj);
                Destroy(other);
            }
        }
        else if (other.gameObject.CompareTag("DeathZone"))
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            winLooseScript.Win();
        }
        else if (other.gameObject.CompareTag("Target"))
        {
            Time.timeScale = 0;
            ChangeAnimation(AnimType.Dance);
            _winCamera.gameObject.SetActive(true);
            _mainCamera.gameObject.SetActive(false);
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BridgeWall"))
        {
            other.isTrigger = true;
        }
    }
}
