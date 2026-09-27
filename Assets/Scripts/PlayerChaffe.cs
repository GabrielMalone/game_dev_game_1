using UnityEngine;

public class PlayerChaffe : MonoBehaviour
{

    [Header("Chaffe Settings")]
    public float timeToLive = 1f;
    public float chaffeGravity = 2f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (Random.value < 0.5f)
            rb.gravityScale = chaffeGravity * -1;
        else 
            rb.gravityScale = chaffeGravity;

        Destroy(gameObject, timeToLive);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void spreadChaffe()
    {

    }


}
