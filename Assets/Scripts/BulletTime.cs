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
        targetPitch = slowdownFactor;
        analyzer.beatSpeedMultiplier *= 0.5f;
        analyzer.maxSpeed *= 0.5f;
        mc.regenSpeed = regenBoost;
        PlayerController.playerHealth -= energyDrain;
    }


    public void SlowMo(float slowFactor)
    {
        targetPitch = slowFactor;
        analyzer.beatSpeedMultiplier *= (slowFactor * slowFactor);
        analyzer.maxSpeed *= (slowFactor * slowFactor);      
    }

}
