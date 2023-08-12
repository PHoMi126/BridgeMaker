using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] internal GameObject _brickPrefab;
    [SerializeField] internal Transform _brickTransform;
    [SerializeField] internal Material _material;
    [SerializeField] internal Animator _animator;
    [SerializeField] internal WinLoose winLooseScript;
    [SerializeField] internal GameObject character;
    [SerializeField] internal BrickController.BrickType brickType = BrickController.BrickType.Red;
    [SerializeField] internal List<GameObject> listBrickHave = new();

    GameObject obj;
    //private Vector3 targetPos;
    private bool isTriggered = false;

    /* private void FixedUpdate()
    {
        DetectHit(transform.position, 0.5f, transform.forward);
    } */

    public enum AnimType
    {
        Idle, Running, Dance
    }
    AnimType currentAnimName;

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

    public void OnTriggerEnter(Collider other)
    {
        BrickController brick = other.GetComponent<BrickController>();

        if (brick != null && brick.brickType == brickType)
        {
            brick.BrickEaten();
            obj = Instantiate(_brickPrefab, _brickTransform);
            obj.GetComponent<BrickController>().enabled = false;

            obj.transform.localPosition = new Vector3(0f, listBrickHave.Count * 0.15f, 0f);
            listBrickHave.Add(obj);
        }
        else if (other.gameObject.CompareTag("BridgeTile"))
        {
            if (listBrickHave.Count > 0)
            {
                //Change Brick Color
                other.GetComponent<Renderer>().material = _material;
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else if (other.gameObject.CompareTag("BridgeWall"))
        {
            if (listBrickHave.Count == 0)
            {
                isTriggered = false;
                other.isTrigger = false;
            }
            else if (isTriggered == false)
            {
                isTriggered = true;
                obj = listBrickHave[listBrickHave.Count - 1];
                Destroy(obj);
                listBrickHave.Remove(obj);
                //Destroy(other);
                other.isTrigger = true;
                Debug.Log(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Target"))
        {
            Time.timeScale = 0;
            ChangeAnimation(AnimType.Dance);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BridgeWall"))
        {
            isTriggered = false;
            other.isTrigger = true;
        }
    }

    /* RaycastHit DetectHit(Vector3 startPos, float distance, Vector3 direction)
    {
        Ray ray = new Ray(startPos, direction);
        RaycastHit hit;
        Vector3 endPos = startPos + (distance * direction);
        while (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.CompareTag("BridgeWall"))
            {
                if (listBrickHave.Count == 0)
                {
                    hit.collider.isTrigger = false;
                }
                else
                {
                    transform.localPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                    //obj = listBrickHave[listBrickHave.Count - 1];
                    //Destroy(obj);
                    //listBrickHave.Remove(obj);
                    //Destroy(other);
                    hit.collider.isTrigger = true;
                }
            }
            endPos = hit.point;
            break;
        }
        Debug.DrawLine(startPos, endPos, Color.black);
        return hit;
    } */
}
