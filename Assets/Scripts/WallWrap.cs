using UnityEngine;
using System.Collections;

public class WallWrap : MonoBehaviour
{
    public enum WallSide
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public WallSide wallSide;
    public Transform oppositeWall;
    public float offset = 1f;
    public ParticleSystem shieldParticles;
    public GameObject player;
    private TrailRenderer trail;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        trail = player.GetComponent<TrailRenderer>();

        Vector3 position = other.transform.position;

        trail.emitting = false;
        trail.Clear();
        
        shieldParticles.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear
        );

        switch (wallSide)
        {
            case WallSide.Left:
                position.x = oppositeWall.position.x - offset;
                break;

            case WallSide.Right:
                position.x = oppositeWall.position.x + offset;
                break;

            case WallSide.Top:
                position.y = oppositeWall.position.y - offset;
                break;

            case WallSide.Bottom:
                position.y = oppositeWall.position.y + offset;
                break;
        }

        other.transform.position = position;

        StartCoroutine(RestartParticles());
    }

    IEnumerator RestartParticles()
    {
        // yiled = stop executing code here, after 5 seconds come back and start running again
        yield return new WaitForSeconds(0.5f);
        trail.emitting = true;
    }
}