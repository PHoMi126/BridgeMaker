using UnityEngine;

public class BrickSpawn : MonoBehaviour
{
    [SerializeField] GameObject brickPrefabs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < -7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    GameObject obj = Instantiate(brickPrefabs);
                    obj.transform.position = new Vector3(-7f - i * 2, 6f, 7f - j * 2);
                }
            }
        }
    }
}
