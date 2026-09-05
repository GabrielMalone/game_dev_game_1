using UnityEngine;
using UnityEngine.InputSystem;

//https://www.youtube.com/watch?v=S6eRVwAtfOM
public class Laser : MonoBehaviour
{

    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            EnableLaser();   
        }
        if (Mouse.current.leftButton.isPressed)
        {
            UpdateLaser();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)   
        {
            DisableLaser();
        }
        if (Gamepad.current != null)
        {
            // A button pressed
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                EnableLaser();
            }

            // A button held
            if (Gamepad.current.buttonSouth.isPressed)
            {
                UpdateLaser();
            }

            // A button released
            if (Gamepad.current.buttonSouth.wasReleasedThisFrame)
            {
                DisableLaser();
            }
        }

    }

    void EnableLaser()
    {
        lineRenderer.enabled = true;
    }

    void UpdateLaser()
    {
        Vector2 startPosition = firePoint.position;
        Vector2 direction = firePoint.up;

        RaycastHit2D hit = Physics2D.Raycast(
            startPosition,
            direction,
            1000f
        );

        lineRenderer.SetPosition(0, startPosition);

        if (hit.collider != null)
        {
            // Stop laser at object
            lineRenderer.SetPosition(1, hit.point);

            // Destroy Enemy(Clone)
            if (hit.collider.gameObject.name == "Enemy(Clone)")
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            lineRenderer.SetPosition(
                1,
                startPosition + direction * 1000f
            );
        }
}

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }
}
