using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Stuff")]
    public float thrustForce = 20f;
    public float torque = 5f;
    public float maxPlayerSpeed = 12f;
    public float sidewaysDrag = 0.5f;

    [Header("Defensive Stuff")]
    public float repulseRadius = 5f;
    public float repulseForce = 10f;



    Rigidbody2D rb;

   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // FORWARD
        if (Keyboard.current.wKey.isPressed)
        {
            rb.AddForce(transform.up * thrustForce);
        }

        // LEFT
        if (Keyboard.current.aKey.isPressed)
        {
            rb.AddTorque(torque);
        }

        // BACKWARD
        if (Keyboard.current.sKey.isPressed)
        {
            rb.AddForce(-transform.up * thrustForce);
        }

        // RIGHT
        if (Keyboard.current.dKey.isPressed)
        {
            rb.AddTorque(-torque);
        }
 
        if (Keyboard.current.spaceKey.isPressed)
        {
            Repulse();
        }

        ReduceSidewaysVelocity();
        SpeedCheck();
    }
    // this should help me get rid of the sluggish movment after too many turns or running into walls/obstacles
    void ReduceSidewaysVelocity()
    {
        // rb.linearVelocity is direction and speed the ship is currently moving
        // transform.up is direction ship. is currently facing
        // these could be different directions
        // dot product tells how much of ship velocity is going in the direction the ship is facing
        // or just how fast are we moving forward
        // then we just multiply fowardirect * forward speed to get forward velocity
        Vector2 forwardVelocity =
            transform.up *
            Vector2.Dot(rb.linearVelocity, transform.up);

        // same as above but for sidweways movement
        Vector2 sidewaysVelocity =
            transform.right *
            Vector2.Dot(rb.linearVelocity, transform.right);

        // now we can reduce sidways movment here by cutting it into some fraction
        rb.linearVelocity =
            forwardVelocity +
            sidewaysVelocity * sidewaysDrag;
    }

    void SpeedCheck()
    {
        if (rb.linearVelocity.magnitude > maxPlayerSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * maxPlayerSpeed;
        }
    }
    // called automatically by unity, dont need to put in update
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            WallHit(collision);
        }
    }


    void WallHit(Collision2D collision)
    {
        // contains direction and speed of player
        Vector2 incomingVelocity = rb.linearVelocity;

        // give the contact point from this collision with the wall
        // 0 being the first point of contact (there can be multiple)
        // a normal is a vector pointing perpendicular away from a surface
        // aka which way does the surface I hit point to
        Vector2 wallNormal =
            collision.GetContact(0).normal;

        // take the direction the player was going and reflect if off the wall using the normal vector
        // straight on = straight back , at an angle = at the mirrored angle
        // speed is maintained 
        Vector2 reflectedVelocity =
            Vector2.Reflect(incomingVelocity, wallNormal);

        // Actually bounce the player's movement
        rb.linearVelocity = reflectedVelocity;

        // Face the direction we're now moving
        // convert a vector direction to an angle
        float angle =
            Mathf.Atan2(reflectedVelocity.y, reflectedVelocity.x)
            * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void Repulse()
    {
        Collider2D[] objectsInRange =
            Physics2D.OverlapCircleAll(transform.position, repulseRadius);

        foreach (Collider2D obj in objectsInRange)
        {
            // Don't push ourselves
            if (obj.gameObject == gameObject)
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

}