using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 originalScale;

    [Header("Size and Speed")]
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    public float minSpeed = 5f;
    public float maxSpeed = 25f;
    public float maxSpinSpeed = 5f;


    [Header("NavMesh Agility")]
    public float acceleration = 100f;
    public float turnSpeed = 720f;

    [Header("Movement")]
    public float currentSpeed;
    public float beatSpeedMultiplier = 1.5f;
    public float speedSmoothing = 5f;
    public float sidewaysDrag = 0.5f;

    private Vector2 moveDirection;

    private AudioAnalyzer analyzer;

    [Header("Music Data")]
    public float bass;
    public float lowMid;
    public float mid;
    public float highMid;
    public float treble;
    public float volume;
    public float dominantFrequency;

    private SpriteRenderer spriteRenderer;

    [Header("Frequency")]
    public float minFrequency = 50f;
    public float maxFrequency = 3000f;

    [Header("Treble")]
    public float minTreble = 0.001f;
    public float maxTreble = 0.05f;

    [Header("Volume")]
    public float minVolume = 0.05f;
    public float maxVolume = 1f;

    [Header("Beat Effects")]
    private float sizePulse = 1f;
    public float beatSizeMultiplier = 1.5f;
    public float sizeReturnSpeed = 8f;


    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initEnememy();
    }

    // Update is called once per frame
    void Update()
    {
        agent.speed = analyzer.targetSpeed * 3;
        spriteRenderer.color = analyzer.currentColor;
        pulseOnBeat();
        

    }



    void initEnememy()
    {
        // update the pathfinding agent not the game object directly
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.acceleration = acceleration;
        agent.angularSpeed = turnSpeed;

        // for audio effects
        analyzer = FindAnyObjectByType<AudioAnalyzer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    void pulseOnBeat()
    {

        if (analyzer.beatDetected)
        {
            sizePulse = beatSizeMultiplier;
        }

        sizePulse = Mathf.Lerp(
            sizePulse,
            1f,
            sizeReturnSpeed * Time.deltaTime
        );

        transform.localScale = originalScale * sizePulse;

    }

}
 


