using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 1f;
    public float torque = 0.5f;
    public float maxPlayerSpeed = 2f;
    
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Debug.Log("Mouse Pos: " + mousePos);
            // mouse position - player object position
            Vector2 direction = mousePos - transform.position;
            // set the player game object facing that direction
            transform.up = direction.normalized;
        }

        if (Keyboard.current.wKey.isPressed)
        {
            rb.AddForce(transform.up * thrustForce);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            rb.AddTorque(torque);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            rb.AddForce(-transform.up * thrustForce);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            rb.AddTorque(-torque);
        }

        SpeedCheck(rb);

    }



    void SpeedCheck(Rigidbody2D rb)
    {
        if (rb.linearVelocity.magnitude > maxPlayerSpeed)
        {
            // normalized takes away speed and gives only direction. 
            rb.linearVelocity = rb.linearVelocity.normalized * maxPlayerSpeed;
        }
    }

}
