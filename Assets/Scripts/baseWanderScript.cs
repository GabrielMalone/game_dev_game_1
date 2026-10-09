using UnityEngine;

public class baseWanderScript : MonoBehaviour
{
    [Header("Base Movement Settings")]
    public GameObject baseExplorableArea;
    public Rigidbody2D rb;
    private BoxCollider2D box;
    private UnityEngine.AI.NavMeshAgent agent;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private Vector2 nextRndLocation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = baseExplorableArea.GetComponent<BoxCollider2D>();

        minX = box.bounds.min.x;
        maxX = box.bounds.max.x;
        minY = box.bounds.min.y;
        maxY = box.bounds.max.y;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        GetRandomLocation();
    }

    // Update is called once per frame
    void Update()
    {
        GoToRndLocation();
    }

    void GetRandomLocation()
    {
        float rndX = Random.Range(minX, maxX);
        float rndY = Random.Range(minY, maxY);
        nextRndLocation = new Vector2(rndX, rndY);
    }

    void GoToRndLocation()
    {
        agent.SetDestination(nextRndLocation);
    }

}
