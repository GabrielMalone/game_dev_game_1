using UnityEngine;
using UnityEngine.AI;

public class DropPullScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject movementTarget;
    private GameObject target;
    private BulletTime bulletTime;
    private Rigidbody2D rb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameObject.FindGameObjectWithTag("Player");
        bulletTime = target.GetComponent<BulletTime>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // If we already have a movement target, stay on it
        if (target != null)
        { 
            if (bulletTime.slowDownEnabled)
            {
                agent.enabled = true;    
                rb.gravityScale = 0;
                agent.SetDestination(target.transform.position);
                
            } 
            if (!bulletTime.slowDownEnabled)
            {
                rb.gravityScale = 25;   
                agent.enabled = false;    
            }
        }
    }

}