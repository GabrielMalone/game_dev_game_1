using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    void EnableLaser()
    {
        lineRenderer.enabled = true;
    }

    void UpdateLaser()
    {
        // start position
        lineRenderer.SetPosition(0, firePoint.position);
        // end position
        lineRenderer.SetPosition(1, firePoint.position + firePoint.up * 1000f);
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }
}
