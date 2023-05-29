using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _navMesh;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private Transform _brickTransform;
    [SerializeField] private Material _material;
    [SerializeField] private CurrentState _currentState;

    public enum CurrentState
    {
        Idle, Running, Dance
    }

    public WinLoose winLooseScript;
    private List<GameObject> listBrickHave = new List<GameObject>();
    public BrickController.BrickType brickType = BrickController.BrickType.RED;
    GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<NavMeshAgent>().SetDestination(_navMesh.position);
        StartCoroutine(SwitchAnim());
    }
    void Update()
    {
        AnimCheck();
    }

    public void AnimCheck()
    {
        if(_currentState == CurrentState.Idle)
        {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isDance", false);
        }
        else if (_currentState == CurrentState.Running)
        {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isRunning", true);
            _animator.SetBool("isDance", false);
        }
        else if (_currentState == CurrentState.Dance)
        {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isDance", true);
            _rigidbody.velocity = Vector3.zero;
        }
    }

    IEnumerator SwitchAnim()
    {
        yield return new WaitForSeconds(0.5f);
        _currentState = CurrentState.Running;
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
        else if (other.gameObject.tag == "BridgeTile")
        {
            //Change Brick Color
            other.GetComponent<Renderer>().material = _material;

            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (other.gameObject.tag == "BridgeWall")
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
        else if (other.gameObject.tag == "Target")
        {
            Time.timeScale = 0;
            _currentState = CurrentState.Dance;
        }
        else if (other.gameObject.tag == "Finish")
        {
            winLooseScript.Loose();
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
