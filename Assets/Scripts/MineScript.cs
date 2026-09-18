using UnityEngine;
using System.Collections.Generic;

public class MineScript : MonoBehaviour
{
    [Header("Mine Settings")]
    public Transform minePosition;
    public float mineRadius = 1f;
    public LayerMask enemyLayer;
    public LineRenderer mineLine;
    public static int maxNumEnemiesBeforeExplosion = 10;
    public static float timeBeforeExplosion = 3f;
    [Header("Mine Particle Effect")]
    public ParticleSystem lineParticles;
    public float particleTravelSpeed = 5f;
    private float particleDistance = 0f;

    [Header("Enemies Tagged By Mine")]
    public static List<GameObject> allEnemiesTaggedByMine = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mineLine.useWorldSpace = true;
        allEnemiesTaggedByMine.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        getEnemiesInRange();
        Debug.Log("Enemies tagged count: " + allEnemiesTaggedByMine.Count);
        drawMineLine();
        moveParticlesAlongLine();
    }

    void getEnemiesInRange()
    {
        
        Collider2D[] objectsInRange =
            Physics2D.OverlapCircleAll(
                minePosition.position,
                mineRadius,
                enemyLayer
            );

        foreach (Collider2D obj in objectsInRange)
        {
            if (!obj.CompareTag("Enemy"))
                continue;
            if (obj.gameObject == gameObject)
                continue;

            if (!allEnemiesTaggedByMine.Contains(obj.gameObject))
            {
                allEnemiesTaggedByMine.Add(obj.gameObject);
                EnemyBehavior enemyBehavior =
                obj.GetComponent<EnemyBehavior>();

                if (enemyBehavior != null)
                {
                    enemyBehavior.selectedByMine = true;
                    ParticleSystem newParticles = Instantiate(
                        lineParticles,
                        obj.transform.position,
                        Quaternion.identity,
                        obj.transform
                    );
                                    }
            }
        }
    }
    void drawMineLine()
    {
        if (allEnemiesTaggedByMine.Count == 0)
        {
            mineLine.enabled = false;
            return;
        }
        mineLine.enabled = true;
        mineLine.useWorldSpace = true;
        // +1 because the mine itself is the first point
        mineLine.positionCount = allEnemiesTaggedByMine.Count + 1;

        // First point = mine
        mineLine.SetPosition(0, minePosition.position);

        // Remaining points = tagged enemies
        for (int i = 0; i < allEnemiesTaggedByMine.Count; i++)
        {
            GameObject enemy = allEnemiesTaggedByMine[i];

            if (enemy != null)
            {
                mineLine.SetPosition(i + 1, enemy.transform.position);
            }
        }
    }

    void moveParticlesAlongLine()
    {
        if (allEnemiesTaggedByMine.Count == 0)
            return;

        // Build our path:
        // mine -> enemy 0 -> enemy 1 -> enemy 2...
        List<Vector3> points = new List<Vector3>();

        points.Add(minePosition.position);

        foreach (GameObject enemy in allEnemiesTaggedByMine)
        {
            if (enemy != null)
            {
                points.Add(enemy.transform.position);
            }
        }

        if (points.Count < 2)
            return;

        particleDistance += particleTravelSpeed * Time.deltaTime;

        float remainingDistance = particleDistance;

        // Walk through each segment of the line
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 start = points[i];
            Vector3 end = points[i + 1];

            float segmentLength = Vector3.Distance(start, end);

            if (remainingDistance <= segmentLength)
            {
                float t = remainingDistance / segmentLength;

                Vector3 particlePosition =
                    Vector3.Lerp(start, end, t);

                lineParticles.transform.position = particlePosition;

                return;
            }

            remainingDistance -= segmentLength;
        }

        // Reached the end — start again
        particleDistance = 0f;
    }
}
