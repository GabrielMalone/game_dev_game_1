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
    }

    void FixedUpdate()
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
        // turn the line renderer on
        circle.enabled = true;
        circle.useWorldSpace = true;
        // connect the last point back to the first point
        circle.loop = true;
        // construct the line with 100 points (or whatever # segments)
        circle.positionCount = segments;

        Vector3 center = playerCollider.transform.position;

        for (int i = 0; i < segments; i++)
        {
            // divide the entire circle into segments of equal angles, then give me angle number i.
            // full circle is 2pi (2f * Mathf.PI)
            // 2f * Mathf.PI / segments is how many degrees we move over each time
            // i how many steps around the circle we've made it
            float angle = i * 2f * Mathf.PI / segments;
            // convert angle to x, y coordinate
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;


            // line renderer draws straight lines between the points automatically
            circle.SetPosition(
                i,
                center + new Vector3(x, y, 0)
            );
        }
    }
}
