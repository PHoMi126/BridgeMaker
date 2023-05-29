using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

//if you use this code you are contractually obligated to like the YT video
public class RandomMovement : MonoBehaviour //don't forget to change the script name if you haven't
{
    public NavMeshAgent agent;
    public float range; //radius of sphere

    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _navMesh;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _brickPrefab;
    [SerializeField] private Transform _brickTransform;
    [SerializeField] private Material _material;
    [SerializeField] private CurrentState _currentState;

    public WinLoose winLooseScript;
    private List<GameObject> listBrickHave = new List<GameObject>();
    public BrickController.BrickType brickType = BrickController.BrickType.RED;
    GameObject obj;

    public enum CurrentState
    {
        Idle, Running, Dance
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(SwitchAnim());
    }

    
    void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }

        AnimCheck();
    }

    public void AnimCheck()
    {
        if (_currentState == CurrentState.Idle)
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

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return true;
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
