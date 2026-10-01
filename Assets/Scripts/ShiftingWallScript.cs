using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShiftingWallScript : MonoBehaviour
{
    [Header("Wall Settings")]
    public float pointSpacing = 2f;
    public float scrollSpeed = 0.5f;
    public float openingSize = 10f;
    public float beatEffectTime = 2f;
    public float minOpenningSize = 70f;
    public float maxOpeningSize = 175f;
    public float openinSizeRndVar = 10f;
    public AudioAnalyzer audioAnalyzer;
    public EdgeCollider2D topWallCollider;
    public EdgeCollider2D bottomWallCollider;
    public GameObject sparkPrefab;
    private float volume;
    private float beatTimeStart = 0f;
    private bool wallBeat = false;

    private float ogScrollSpeed;
    
    public LineRenderer topWallLine;
    public LineRenderer bottomWallLine;

    private float openingCenterY = 0;
    private BoxCollider2D box;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float maxWallWidth;
    private List<Vector3> topWallPoints = new List<Vector3>();
    private List<Vector3> bottomWallPoints = new List<Vector3>();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();

        minX = box.bounds.min.x;
        maxX = box.bounds.max.x;
        minY = box.bounds.min.y;
        maxY = box.bounds.max.y;

        maxWallWidth = maxY;

        // load initial points
        for (float x = minX; x <= maxX; x += pointSpacing)
        {
            AddWallPoint(x);
        }

        ogScrollSpeed = scrollSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        ScrollWall();
        RemoveOldPoints();
        AddNewPointIfNeeded();
        DrawWall();

        if (AudioAnalyzer.beatDetected && !wallBeat)
        {
            beatTimeStart = Time.time;
            wallBeat = true;
        }

        if (wallBeat && Time.time - beatTimeStart > beatEffectTime)
        {
            wallBeat = false;
        }

        if (Keyboard.current.spaceKey.isPressed)
        {
            scrollSpeed = Mathf.Lerp(
                scrollSpeed, 
                0,
                2f * Time.deltaTime
            );
        }
        else if (! Keyboard.current.spaceKey.isPressed && scrollSpeed != ogScrollSpeed)
        {
            scrollSpeed = Mathf.Lerp(
                scrollSpeed,
                ogScrollSpeed,
                2f * Time.deltaTime
            );
        }

    }

    void AddWallPoint(float x)
    {
        if (wallBeat)
        {
            openingSize += Random.Range(-openinSizeRndVar, openinSizeRndVar);
            openingSize = Mathf.Clamp(openingSize, minOpenningSize, maxOpeningSize);

            openingCenterY += Random.Range(-1f, 1f);
            openingCenterY = Mathf.Clamp(
                openingCenterY,
                minY + openingSize / 2f,
                maxY - openingSize / 2f
            );
        }

        float topY = openingCenterY + openingSize / 2f;
        float bottomY = openingCenterY - openingSize / 2f;

        topWallPoints.Add(new Vector3(x, topY, 0));
        bottomWallPoints.Add(new Vector3(x, bottomY, 0));
    }


    void DrawWall()
    {
        // Draw visual walls
        topWallLine.positionCount = topWallPoints.Count;
        topWallLine.SetPositions(topWallPoints.ToArray());

        bottomWallLine.positionCount = bottomWallPoints.Count;
        bottomWallLine.SetPositions(bottomWallPoints.ToArray());

        // Update physical walls
        Vector2[] topColliderPoints = new Vector2[topWallPoints.Count];
        Vector2[] bottomColliderPoints = new Vector2[bottomWallPoints.Count];

        for (int i = 0; i < topWallPoints.Count; i++)
        {
            topColliderPoints[i] =
                topWallCollider.transform.InverseTransformPoint(topWallPoints[i]);

            bottomColliderPoints[i] =
                bottomWallCollider.transform.InverseTransformPoint(bottomWallPoints[i]);
        }

        topWallCollider.points = topColliderPoints;
        bottomWallCollider.points = bottomColliderPoints;
    }

    void ScrollWall()
    {
        for (int i = 0; i < topWallPoints.Count; i++)
        {
            Vector3 topPoint = topWallPoints[i];
            Vector3 bottomPoint = bottomWallPoints[i];

            topPoint.x -= scrollSpeed * Time.deltaTime * (1/audioAnalyzer.volume);
            bottomPoint.x -= scrollSpeed * Time.deltaTime * (1/audioAnalyzer.volume);

            topWallPoints[i] = topPoint;
            bottomWallPoints[i] = bottomPoint;
        }
    }

    void AddNewPointIfNeeded()
    {
        Vector3 lastPoint = topWallPoints[topWallPoints.Count - 1];

        if (maxX - lastPoint.x >= pointSpacing)
        {
            AddWallPoint(maxX);
        }
    }

    void RemoveOldPoints()
    {
        if (topWallPoints[0].x < minX)
        {
            topWallPoints.RemoveAt(0);
            bottomWallPoints.RemoveAt(0);
        }
    }

}
