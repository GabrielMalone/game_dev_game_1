using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }

}
