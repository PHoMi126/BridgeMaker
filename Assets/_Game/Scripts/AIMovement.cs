using UnityEngine;
using UnityEngine.AI; //important

public class AIMovement : MonoBehaviour
{
    public float waitTimer = 0f;
    public NavMeshAgent agent;
    public float range; //radius of sphere
    public Transform centrePoint; //centre of the area the agent wants to move around in
                                  //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    public CharacterController character;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        waitTimer += Time.deltaTime;
        if (character != null)
        {
            if (agent.remainingDistance <= agent.stoppingDistance) //done with path
            {
                character.ChangeAnimation(CharacterController.AnimType.Idle);

                if (RandomPoint(centrePoint.position, range, out Vector3 point) && waitTimer >= 4f) //pass in our centre point and radius of area
                {
                    character.ChangeAnimation(CharacterController.AnimType.Running);
                    waitTimer = 0f;
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    agent.SetDestination(point);
                }
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
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
        if (other.gameObject.CompareTag("Finish"))
        {
            character.winLooseScript.Loose();
        }
    }
}