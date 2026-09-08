using UnityEngine;

public class BaseShield : MonoBehaviour
{   

    private Rigidbody2D rb;

    [Header("Base Shield Settings")]
    public ParticleSystem shieldParticles;
    public float repulseRadius = 5f;
    private float ogRepulseRadius;
    public float repulseForce = 10f;
    public float shieldRadius = 20f;

    [Header("Shield Effects")]
    public LineRenderer circle;
    public float radius = 2f;
    public int segments = 100;
    public PolygonCollider2D playerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        RenderShield();
    }

    // Update is called once per frame
    void Update()
    {
        Repulse();   
    }

    void Repulse()
    {
        Collider2D[] objectsInRange =
            Physics2D.OverlapCircleAll(transform.position, repulseRadius);
        
        foreach (Collider2D obj in objectsInRange)
        {
            // Don't push ourselves
            if (obj.CompareTag("Player"))
                continue;

            Rigidbody2D rb = obj.attachedRigidbody;

            if (rb != null)
            {
                // create a vector pointing from me to the other object
                Vector2 direction =
                    (obj.transform.position - transform.position).normalized;

                rb.AddForce(
                    direction * repulseForce,
                    ForceMode2D.Impulse
                );
            }
        }
    }

    void RenderShield()
    {
        circle.enabled = true;
        circle.useWorldSpace = true;
        circle.loop = true;
        circle.positionCount = segments;

        Vector3 center = playerCollider.transform.position;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * 2f * Mathf.PI / segments;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            circle.SetPosition(
                i,
                center + new Vector3(x, y, 0)
            );
        }
    }
}
