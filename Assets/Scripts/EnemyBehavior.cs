using UnityEngine;
using System.Collections;
using Unity.Cinemachine;


public class EnemyBehavior : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 originalScale;

    [Header("Size and Speed")]
    public float minSize = 0.5f;
    public float maxSize = 2.0f;



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

    [Header("Treble Teleport")]
    public float teleportRadius = 0.25f;
    public float teleportDuration = 0.01f;

    [Header("Treble Effect")]
    public Material bloomMaterial;
    public Material defaultMaterial;
    private Material enemyBloomMaterial;
    public float glowSpeed = 5f;
    public float maxGlow = 5f;
    private float currentGlow = 0f;

    [Header("laser hit FX")]
    public ParticleSystem hitSpark;

    private ParticleSystem thrustParticles;
    private CinemachineImpulseSource impulseSource;

    [Header("Enemy Attack Power")]
    public float enemyPowerBase = 0.1f;
    public float enemyPower = 0f;

    [Header("Enemy Mine Attack Effects")]
    public bool selectedByMine = false;
    public float mineLineRadius = 1f;
    public LayerMask enemyLayer;

    [Header("Enemy Crowd Control")]
    public int crowded = 10;
    public float repulseForce = 200f;
    private Collider2D[] crowdBuffer = new Collider2D[1000];
    public float crowdCheckInterval = 0.01f;
    float nextCrowdCheck;
    private float crowdRadius = 10f;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initEnememy();
        thrustParticles = GetComponentInChildren<ParticleSystem>();
        impulseSource = GetComponent<CinemachineImpulseSource>(); 
        nextCrowdCheck = Time.time + Random.Range(0f, crowdCheckInterval);       
    }

    // Update is called once per frame
    void Update()
    {
        enemyPower = enemyPowerBase;
        agent.speed = analyzer.targetSpeed * 3;
        spriteRenderer.color = analyzer.currentColor;
        pulseOnBeat();
        glowOnTreble();
    }

    void FixedUpdate()
    {
        if (selectedByMine)
            getEnemiesInRange();
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

        enemyBloomMaterial = new Material(bloomMaterial);

        enemyPower = enemyPowerBase;
    }

    void pulseOnBeat()
    {

        if (AudioAnalyzer.beatDetected)
        {
            sizePulse = beatSizeMultiplier;
        }

        sizePulse = Mathf.Lerp(
            sizePulse,
            1f,
            sizeReturnSpeed * Time.deltaTime
        );

        transform.localScale = originalScale * sizePulse;

        enemyPower = originalScale.x * sizePulse;

    }

    void glowOnTreble()
    {
        spriteRenderer.material = enemyBloomMaterial;

        Color currentColor = spriteRenderer.color;
        // Treble hit gives us a fresh burst
        if (analyzer.trebleDetected)
        {
            currentGlow = maxGlow;
        }

        // Always fade back down afterward
        currentGlow = Mathf.Lerp(
            currentGlow,
            1f,
            glowSpeed * Time.deltaTime
        );

        Color hdrColor = currentColor * currentGlow;


        enemyBloomMaterial.SetColor("_Color", hdrColor);
        enemyBloomMaterial.SetFloat("_BloomIntensity", currentGlow);
    }


    public int getNumEnemiesInRange()
    {
        return Physics2D.OverlapCircleNonAlloc(
            transform.position,
            crowdRadius,
            crowdBuffer,
            enemyLayer
        );
    }


    public void RepulseCrowd()
    {

        crowdRadius = Random.Range(5f, 25f);

        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            crowdRadius,
            crowdBuffer,
            enemyLayer
        );

        if (count <= crowded)
            return;

        int numRepulsed = 0;

        for (int i = 0; i < count; i++)
        {
            Collider2D obj = crowdBuffer[i];

            if (obj == null)
                continue;

            // don't push yourself
            if (obj.gameObject == gameObject)
                continue;

            Rigidbody2D otherRB = obj.attachedRigidbody;

            if (otherRB == null)
                continue;

            Vector2 direction =
                ((Vector2)obj.transform.position - rb.position).normalized;

            otherRB.AddForce(
                direction * repulseForce,
                ForceMode2D.Impulse
            );

            Debug.Log("IT'S TOO CROWDED IN HERE!@!");

            numRepulsed++;

            // if (numRepulsed >= 10)
            //     break;
        }
    }

    public void getEnemiesInRange()
    {
        
        Collider2D[] objectsInRange =
            Physics2D.OverlapCircleAll(
                transform.position,
                mineLineRadius,
                enemyLayer
            );

        foreach (Collider2D obj in objectsInRange)
        {
            // Ignore anything that isn't an enemy
            if (!obj.CompareTag("Enemy"))
                continue;

            // Don't target yourself
            if (obj.gameObject == gameObject)
                continue;

            EnemyBehavior enemyBehavior =
                obj.GetComponent<EnemyBehavior>();

            if (enemyBehavior != null && !enemyBehavior.selectedByMine)
            {
                enemyBehavior.selectedByMine = true;

                if (!MineScript.allEnemiesTaggedByMine.Contains(obj.gameObject))
                {
                    MineScript.allEnemiesTaggedByMine.Add(obj.gameObject);
                }
            }
        }
    }

}
 


