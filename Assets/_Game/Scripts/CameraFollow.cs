using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + new Vector3(0f, 10f, -10f);
        }
    }
}
