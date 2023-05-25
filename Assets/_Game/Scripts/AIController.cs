using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    public enum AnimType
    {
        Idle, Running, Dance
    }

    [SerializeField] Transform navMesh;
    [SerializeField] private Animator _animator;
    [SerializeField] GameObject brickPrefab;
    [SerializeField] Transform brickTransform;

    private AnimType currentAnimName = AnimType.Idle;
    private List<GameObject> listBrickHave = new List<GameObject>();
    public BrickController.BrickType brickType = BrickController.BrickType.RED;
    GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<NavMeshAgent>().SetDestination(navMesh.position);
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
            obj = Instantiate(brickPrefab, brickTransform);
            obj.GetComponent<BrickController>().enabled = false;

            obj.transform.localPosition = new Vector3(0f, listBrickHave.Count * 0.15f, 0f);
            listBrickHave.Add(obj);
        }
        else if (other.gameObject.tag == "BridgeTile")
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else if (other.gameObject.tag == "DeathZone")
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
