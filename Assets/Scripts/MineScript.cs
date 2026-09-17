using UnityEngine;

public class MineScript : MonoBehaviour
{
    [Header("Mine Settings")]
    public Transform minePosition;
    public float mineRadius = 1f;
    public LayerMask enemyLayer;
    public LineRenderer mineLine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mineLine.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        getEnemiesInRange();
    }

void getEnemiesInRange()
{
    
    mineLine.enabled = false;

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
        // dont target self
        if (obj.CompareTag("Player"))
                    continue;

        Vector2 startPosition = minePosition.position;
        Vector2 enemyPosition = obj.transform.position;

        Vector2 direction =
            (enemyPosition - startPosition).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            startPosition,
            direction,
            mineRadius,
            enemyLayer
        );

        mineLine.enabled = true;
        mineLine.SetPosition(0, startPosition);
        mineLine.SetPosition(1, hit.point);

        EnemyBehavior enemyBehavior =
            obj.GetComponent<EnemyBehavior>();

        if (enemyBehavior != null)
        {
            enemyBehavior.selectedByMine = true;
        }
    }
}

}
