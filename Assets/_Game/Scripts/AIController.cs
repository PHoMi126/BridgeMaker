using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    public enum CurrentState
    {
        Idle, Running, Dance
    }

    [SerializeField] Transform navMesh;
    [SerializeField] private Animator _animator;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] Transform brickTransform;
    [SerializeField] CurrentState _currentState;

    private List<GameObject> listBrickHave = new List<GameObject>();
    public BrickController.BrickType brickType = BrickController.BrickType.RED;
    GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<NavMeshAgent>().SetDestination(navMesh.position);
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
            obj = Instantiate(brickPrefab, brickTransform);
            obj.GetComponent<BrickController>().enabled = false;

            obj.transform.localPosition = new Vector3(0f, listBrickHave.Count * 0.15f, 0f);
            listBrickHave.Add(obj);
        }
        else if (other.gameObject.tag == "BridgeTile")
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (other.gameObject.tag == "Target")
        {
            _currentState = CurrentState.Dance;
        }
    }
}
