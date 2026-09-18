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
    public static float timeBeforeExplosion = 1f;

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
        drawMineLine();
    }

    void FixedUpdate()
    {
        getEnemiesInRange();
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

        // Make a copy so we don't change the original list
        List<GameObject> sortedEnemies =
            new List<GameObject>(allEnemiesTaggedByMine);

   
        mineLine.positionCount = sortedEnemies.Count + 1;

        // Mine is first
        mineLine.SetPosition(0, minePosition.position);

        // Then closest -> farthest
        for (int i = 0; i < sortedEnemies.Count; i++)
        {
            GameObject enemy = sortedEnemies[i];

            if (enemy != null)
            {
                mineLine.SetPosition(i + 1, enemy.transform.position);
            }
        }
    }

}
