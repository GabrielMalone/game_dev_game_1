using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{

    private AudioAnalyzer analyzer;


    [Header("Setup")]
    public BoxCollider2D Wall_left;
    public BoxCollider2D Wall_right;
    public BoxCollider2D Wall_top;
    public BoxCollider2D Wall_bottom;

    public GameObject enemyPrefab;

    [Header("Settings")]
    public int enemiesToSpawn = 500;
    public float spawnOffset = 0f;
    public GameObject enemyThrustParticlesPrefab;

    [Header("Music Data")]
    public float bass;
    public float lowMid;
    public float mid;
    public float highMid;
    public float treble;
    public float volume;
    public float dominantFrequency;

    [Header("Beat Detection")]
    public bool beatDetected;
    public float beatThreshold = 0.01f;
    public float beatCooldown = 0.15f;


    [Header("Enemy Stats")]
    public static List<GameObject> allEnemies = new List<GameObject>();

    void Start()
    {
        analyzer = FindAnyObjectByType<AudioAnalyzer>();
    }

    void Update()
    {
        // let's spawn on beat
        if (AudioAnalyzer.beatDetected)
        {
            SpawnEnemyAlongWall();
        }
    }

    public void SpawnEnemyAlongWall()
    {
        // Randomly choose one of the four walls
        int wallChoice = Random.Range(0, 4);

        BoxCollider2D wallCollider = null;

        // Final spawn position in WORLD SPACE
        Vector3 worldSpawnPosition = Vector3.zero;

        switch (wallChoice)
        {
            case 0: // LEFT WALL
                wallCollider = Wall_left;

                worldSpawnPosition = new Vector3(
                    wallCollider.bounds.center.x + spawnOffset,
                    Random.Range(
                        wallCollider.bounds.min.y,
                        wallCollider.bounds.max.y
                    ),
                    0f
                );
                break;


            case 1: // RIGHT WALL
                wallCollider = Wall_right;

                worldSpawnPosition = new Vector3(
                    wallCollider.bounds.center.x - spawnOffset,
                    Random.Range(
                        wallCollider.bounds.min.y,
                        wallCollider.bounds.max.y
                    ),
                    0f
                );
                break;


            case 2: // TOP WALL
                wallCollider = Wall_top;

                worldSpawnPosition = new Vector3(
                    Random.Range(
                        wallCollider.bounds.min.x,
                        wallCollider.bounds.max.x
                    ),
                    wallCollider.bounds.center.y - spawnOffset,
                    0f
                );
                break;


            case 3: // BOTTOM WALL
                wallCollider = Wall_bottom;

                worldSpawnPosition = new Vector3(
                    Random.Range(
                        wallCollider.bounds.min.x,
                        wallCollider.bounds.max.x
                    ),
                    wallCollider.bounds.center.y + spawnOffset,
                    0f
                );
                break;
        }

        if (allEnemies.Count < enemiesToSpawn)
        {
            GameObject enemy = Instantiate(
                enemyPrefab,
                worldSpawnPosition,
                Quaternion.identity
            );
            allEnemies.Add(enemy);
        }


    }
}