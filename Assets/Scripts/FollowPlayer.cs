using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3f;
    public float stoppingDistance = 1f;
    public GameObject player;
    private Transform playerTransform;

    void Start()
    {        
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Prevent errors if the player doesn't exist in the scene
        if (playerTransform == null)
        {
            Debug.Log("player invalid");
            return;
        }

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Only move toward the player if outside the stopping distance
        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,  // current position
                playerTransform.position, // target position
                speed * Time.deltaTime // maximum distance to move (how far am I allowed to move this frame)
            ); 
        }
    }

}
