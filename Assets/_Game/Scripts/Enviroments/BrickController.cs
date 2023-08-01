using System.Collections;
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
        yield return new WaitForSeconds(timeDelay);
        brickMeshRenderer.gameObject.SetActive(true);
        this.GetComponent<BoxCollider>().enabled = true;
        brickType = (BrickType)Random.Range(0, 3);
        brickMeshRenderer.material = colorScriptable.listMaterial[(int)brickType];
    }

    public void BrickEaten()
    {
        brickMeshRenderer.gameObject.SetActive(false);
        this.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(ResetBrick(5f));
    }
}
