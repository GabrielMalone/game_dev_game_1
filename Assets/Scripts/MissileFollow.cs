using UnityEngine;
using UnityEngine.AI;

public class MissileFollow : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // If we already have a movement target, stay on it
        if (target != null)
        {
            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                agent.SetDestination(target.transform.position);
            }
        }
    }
}