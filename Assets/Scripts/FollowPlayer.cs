using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private GameObject[] targets;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Disregard 3D orientation stuff
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Find every object tagged "Player"
        targets = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        GameObject closestTarget = GetClosestTarget();

        if (closestTarget != null)
        {
            agent.SetDestination(closestTarget.transform.position);
        }
    }

    GameObject GetClosestTarget()
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector2.Distance(
                transform.position,
                target.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }
}