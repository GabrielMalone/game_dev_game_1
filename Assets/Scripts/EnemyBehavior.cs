using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;

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
    private float beatPulse = 1f;

    private AudioAnalyzer analyzer;

    [Header("Music Data")]
    public float bass;
    public float lowMid;
    public float mid;
    public float highMid;
    public float treble;
    public float volume;
    public float dominantFrequency;

    private Color currentColor;
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

    [Header("Smoothing")]
    public float colorSpeed = 5f;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initEnememy();
    }

    // Update is called once per frame
    void Update()
    {
        FrequencyColorChange();
        UpdateMovement();

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
        currentColor = spriteRenderer.color;

        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);
        rb = GetComponent<Rigidbody2D>();
    }




    void FrequencyColorChange()
    {
        bass = analyzer.bass;
        lowMid = analyzer.lowMid;
        mid = analyzer.mid;
        highMid = analyzer.highMid;
        treble = analyzer.treble;
        volume = analyzer.volume;
        dominantFrequency = analyzer.dominantFrequency;

        // -------------------------------
        // FREQUENCY -> COLOR
        // -------------------------------

        float frequency = analyzer.dominantFrequency;
        // how far is the value between a minimum and a maximum
        float hue = Mathf.InverseLerp(
            minFrequency,
            maxFrequency,
            frequency
        );

        // TREBLE -> SATURATION

        float saturation = Mathf.InverseLerp(
            minTreble,
            maxTreble,
            analyzer.treble
        );


        // -------------------------------
        // VOLUME -> INTENSITY
        // -------------------------------
        // how far is the value between a minimum and a maximum
        float intensity = Mathf.InverseLerp(
            minVolume,
            maxVolume,
            analyzer.volume
        );


        // -------------------------------
        // CREATE COLOR
        // -------------------------------
        // hue, saturation, value to RGB
        Color targetColor = Color.HSVToRGB(
            hue,
            1f,
            intensity + 0.2f
        );


        // -------------------------------
        // SMOOTH COLOR CHANGE
        // -------------------------------

        currentColor = Color.Lerp(
            currentColor,
            targetColor,
            colorSpeed * Time.deltaTime
        );

        spriteRenderer.color = currentColor;
        Debug.Log("Frequency: " + frequency + " Hue: " + hue);
    }

    void UpdateMovement()
    {
        float volumeAmount = Mathf.InverseLerp(
            minVolume,
            maxVolume,
            analyzer.volume
        );

        float targetSpeed = Mathf.Lerp(
            minSpeed,
            maxSpeed,
            volumeAmount

        );

        if (analyzer.beatDetected)
        {
            beatPulse = beatSpeedMultiplier;
        }

        beatPulse = Mathf.Lerp(
            beatPulse,
            1f,
            10f * Time.deltaTime

        );

        targetSpeed *= beatPulse;

        agent.speed = targetSpeed;
    }

}
 


