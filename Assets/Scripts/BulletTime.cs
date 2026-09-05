using UnityEngine;


public class BulletTime : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource music;
    [Header("Slow Down Effects")]
    public float slowdownFactor = 0.5f;
    public float targetPitch = 1f;
    public AudioAnalyzer analyzer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }


}
