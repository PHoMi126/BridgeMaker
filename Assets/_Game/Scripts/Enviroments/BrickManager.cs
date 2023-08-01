using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] GameObject brickPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAllBricks();
    }

    void SpawnAllBricks()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                GameObject obj = Instantiate(brickPrefabs);

                obj.transform.parent = transform;
                obj.transform.localPosition = new Vector3(0.3f - 0.05f * i * 2, 0.55f, 0.35f - 0.05f * j * 2);
            }
        }
    }
    bool isSpawn = false;
}
