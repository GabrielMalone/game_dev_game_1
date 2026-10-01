using UnityEngine;

public class BeatAttackScript : MonoBehaviour
{

    [Header("BeatAttack Settings")]
    public GameObject beatAttackObject;
    private GameObject spawnedAttackObject;
    public float timeToLive = 2f;
    private float timeAlive;
    private bool attackpresent = false;

    private BoxCollider2D box;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (! attackpresent ) 
            SpawnBeatAttack();
        explodeAttack();
    }


    void SpawnBeatAttack()
    {
        if (AudioAnalyzer.beatDetected)
        {
            spawnedAttackObject = Instantiate(beatAttackObject, PickRandomSpot(), Quaternion.identity);
            timeAlive = Time.time;
            attackpresent = true;
        }
    }

    Vector2 PickRandomSpot()
    {
        float randomX = Random.Range(box.bounds.min.x, box.bounds.max.x);
        float randomY = Random.Range(box.bounds.min.y, box.bounds.max.y);

        Vector2 v = new Vector2(randomX, randomY);

        Debug.Log($"New location for beat attack: {v}");

        return v;
    }

    void explodeAttack()
    {
        if (Time.time - timeAlive > timeToLive && attackpresent)
        {
            Destroy(spawnedAttackObject);
            attackpresent = false;
        }
    }

}
