using UnityEngine;


public class EnemyShieldSript : MonoBehaviour
{

    public LineRenderer shieldLine;
    public int shieldSegments = 10;
    public float shieldRadius = 1f;
    Rigidbody2D rb;
    EnemyStats enemyStats;
    SpriteRenderer parentRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        parentRenderer = GetComponentInParent<SpriteRenderer>();
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        // RenderShield();   
    }

    void RenderShield()
    {
        shieldLine.enabled = true;
        shieldLine.useWorldSpace = true;
        shieldLine.loop = false;
        shieldLine.positionCount = (int)enemyStats.hitPoints;
        int cnt = (int)enemyStats.hitPoints;

        Vector3 center = rb.transform.position;
        Color curColor = parentRenderer.material.color;

        Vector3 parentScale = rb.transform.localScale;

        for (int i = 0; i < cnt; i++)
        {   
            // 2pi / seg == how much angle between each point
            // i tells us which point currently calculating
            float distanceBetweenPoints = 2f * Mathf.PI / shieldSegments;
            float angle = i * distanceBetweenPoints;

            float x = Mathf.Cos(angle) * shieldRadius * parentScale.x;
            float y = Mathf.Sin(angle) * shieldRadius * parentScale.y;

            shieldLine.SetPosition(
                i,
                center + new Vector3(x, y, 0)
            );

            // shieldLine.material.SetColor(
            //     "_Color",
            //     curColor
            // );
        }
    }
    
}
