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
                other.GetComponent<Renderer>().sharedMaterial = _material;
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
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
                //other.isTrigger = true;
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
            other.isTrigger = true;
        }
    }
}
