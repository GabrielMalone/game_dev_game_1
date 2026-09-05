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

    public BulletTime bulletTime;
    public AudioAnalyzer analyzer;
    
    private float ogEnemySpeed;
    private float ogEnemyBeatMult;

    Rigidbody2D rb;

   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ogEnemySpeed = analyzer.maxSpeed;
        ogEnemyBeatMult = analyzer.beatSpeedMultiplier;
        
    }

    void FixedUpdate()
    {
        Repulse();
        KeyboardInputs();
        ReduceSidewaysVelocity();
        SpeedCheck();
        if (Gamepad.current != null)
        {
            Debug.Log("Controller connected!");
        }
        GamepadInput();
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

    void KeyboardInputs()
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
            bulletTime.SlowMo();
        } else {
            bulletTime.targetPitch = 1f;
            analyzer.beatSpeedMultiplier = ogEnemyBeatMult;
            analyzer.maxSpeed = ogEnemySpeed;
        }   
    }

    void GamepadInput()
    {
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue();

            // Forward/backward thrust
            float rightTrigger = Gamepad.current.rightTrigger.ReadValue();

            rb.AddForce(transform.up * rightTrigger * thrustForce);

            // Rotation
            rb.AddTorque(-stick.x * torque);
        } 

    }


}