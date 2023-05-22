using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] GameObject brickPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAllBricks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnAllBricks()
    {
        for(int i = 0; i < 9;  i++) 
        { 
            for (int j = 0; j < 7; j++)
            {
                GameObject obj = Instantiate(brickPrefabs);
                obj.transform.position = new Vector3(9f - i * 2, 1.1f, 7f - j * 2);
            }
        }
    }
}
