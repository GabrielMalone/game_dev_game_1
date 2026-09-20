using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    [Header("Health")]
    public float hitPoints = 200f;
    public Material deathMaterial;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        SpriteRenderer curSprite = GetComponent<SpriteRenderer>();
        GameObject deadEnemy = new GameObject("deadEnemySpr");
        deadEnemy.transform.position = transform.position;
        deadEnemy.transform.rotation = transform.rotation;
        deadEnemy.transform.localScale = transform.localScale;
        SpriteRenderer deadEnemyRender = deadEnemy.AddComponent<SpriteRenderer>();
        deadEnemyRender.sprite = curSprite.sprite;
        deadEnemyRender.material = deathMaterial;
        deadEnemy.AddComponent<FadeOutSprite>();
        Destroy(gameObject);
    }


}
