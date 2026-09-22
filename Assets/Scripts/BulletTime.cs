using UnityEngine;
using System.Collections;


public class BulletTime : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource music;
    [Header("Slow Down Effects")]
    public float slowdownFactor = 0.5f;
    public float targetPitch = 1f;
    public AudioAnalyzer analyzer;
    public float energyDrain = 10f;
    public int regenBoost = 3;
    public GameObject[] enemyDrops;

    public bool slowDownEnabled = false;
    MineCoolDown mc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mc = GetComponentInParent<MineCoolDown>();  
    }

    // Update is called once per frame
    void Update()
    {
        music.pitch = Mathf.MoveTowards(
            music.pitch,
            targetPitch,
            slowdownFactor * Time.unscaledDeltaTime
        );    
    }

    public void SlowMo()
    {
        slowDownEnabled = true;
        targetPitch = slowdownFactor;
        analyzer.beatSpeedMultiplier *= 0.5f;
        analyzer.maxSpeed *= 0.5f;
        mc.regenSpeed = regenBoost;
        PlayerController.playerHealth -= energyDrain;
        if (PlayerController.playerHealth < 0)
        {
            PlayerController.playerHealth = 0;
        }
        slowDownDrops();
    }


    public void SlowMo(float slowFactor)
    {
        slowDownEnabled = true;
        slowFactor = 1f - slowFactor;
        targetPitch = slowFactor;
        analyzer.beatSpeedMultiplier *= (slowFactor * slowFactor);
        analyzer.maxSpeed *= (slowFactor * slowFactor); 
        mc.regenSpeed = regenBoost;
        PlayerController.playerHealth -= energyDrain;
        if (PlayerController.playerHealth < 0)
        {
            PlayerController.playerHealth = 0;
        }     
        slowDownDrops();
    }

    void slowDownDrops()
    {
        enemyDrops = GameObject.FindGameObjectsWithTag("EnemyDrop");
        foreach (GameObject drop in enemyDrops)
        {
            Rigidbody2D rb = drop.GetComponent<Rigidbody2D>();
            // If it's falling down, cap or reduce its fall speed
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                return;
                // that worked!?
            }
            if (rb.linearVelocity.y < 0) 
            {
                // Dampen the downward velocity by a percentage each frame
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            } 
        }
    }

}
