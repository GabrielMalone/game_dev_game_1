using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private GameObject[] targets;
    private NavMeshAgent agent;

    private GameObject movementTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        targets = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        // If we already have a movement target, stay on it
        if (movementTarget != null)
        {
            Rigidbody2D rb = movementTarget.GetComponent<Rigidbody2D>();

            if (rb != null && rb.linearVelocity.magnitude > 0.5f)
            {
                agent.SetDestination(movementTarget.transform.position);
                return;
            }

            // It stopped moving, so release it
            movementTarget = null;
        }

        // Look for a new moving target
        foreach (GameObject target in targets)
        {
            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();

            if (targetRb != null &&
                targetRb.linearVelocity.magnitude > 0.5f &&
                Random.value < 0.0005f)
            {
                movementTarget = target;
                agent.SetDestination(movementTarget.transform.position);
                return;
            }
        }

        // Otherwise chase the closest player normally
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