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
        findTarget();
    }


    void findTarget()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
        if (targets.Length == 0)
            return;
        GameObject target = targets[Random.Range(0, targets.Length)];
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();

        if (targetRb != null)
        {
            movementTarget = target;
            agent.SetDestination(movementTarget.transform.position);
        }
    }


}