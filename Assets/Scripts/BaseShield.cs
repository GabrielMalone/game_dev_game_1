using UnityEngine;

public class BaseShield : MonoBehaviour
{   


    private Rigidbody2D rb;

    [Header("Base Settings")]



    [Header("Base Shield Settings")]
    public ParticleSystem shieldParticles;
    public float repulseRadius = 5f;
    private float ogRepulseRadius;
    public float repulseForce = 10f;
    public float shieldRadius = 20f;
    private bool shieldEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        bool collisionPresent = false;
        
        foreach (Collider2D obj in objectsInRange)
        {
            // Don't push ourselves
            if (obj.gameObject == gameObject)
                continue;


            collisionPresent = true;
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

        // if (collisionPresent && !shieldEnabled)
        // {
        //     CameraShakeManager.instance.CameraShake(impulseSource);
        // }
    }
}
