using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public enum BrickType
    {
        RED, BLUE, GREEN
    }

    [SerializeField] MeshRenderer brickMeshRenderer;
    [SerializeField] BrickColorScriptable colorScriptable;

    public BrickType brickType = BrickType.RED;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ResetBrick(0f));
    }

    public IEnumerator ResetBrick(float timeDelay)
    {
        yield return new WaitForSecond(timeDelay);
        brickMeshRenderer.gameObject.SetActive(true);
        this.GetComponent<BoxCollider>().enabled = true;
        brickType = (BrickType)Random.Range(0, 3);
        brickMeshRenderer.material = colorScriptable.listMaterial[(int)brickType];
    }


}
