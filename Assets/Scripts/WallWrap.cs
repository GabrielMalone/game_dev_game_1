using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Vector3 position = other.transform.position;

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
    }
}