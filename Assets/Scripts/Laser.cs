using UnityEngine;
using UnityEngine.InputSystem;




//https://www.youtube.com/watch?v=S6eRVwAtfOM
public class Laser : MonoBehaviour
{

    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;

    [Header("Auto Laser Settings")]
    public float autoShootRadius = 30f;
    public LineRenderer[] lasers;
    public int numLasers = 1;
    public float laserPower = 10f;
    public LayerMask enemyLayer;

    [Header("Laser SFX")]
    public AudioSource laserStartAudio;
    public AudioSource laserBodyAudio;
    public AudioSource laserEndAudio;

    private bool lasersEnabled = false;

    [Header("Enemy Spawner")]
    public EnemySpawn enemySpawner;

    [Header("Damage Cooldown")]
    public float damageInterval = 0.25f;
    private float damageTimer = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laserBodyAudio.loop = true;
    }

    // Update is called once per frame
    void Update()
    {   damageTimer -= Time.deltaTime;
        //autoShoot();
        KeyboardInputs();
    }



    public void AutoShoot()
    {
        Collider2D[] objectsInRange =
            Physics2D.OverlapCircleAll(firePoint.position, autoShootRadius);

        int laserIndex = 0;
        
        foreach (Collider2D obj in objectsInRange)
        {   
            if (laserIndex >= numLasers)
                break;

            if (!obj.CompareTag("Enemy"))
                continue;

            Vector2 startPosition = firePoint.position;

            Rigidbody2D rb = obj.attachedRigidbody;

            // create a vector pointing from me to the enemy
            Vector2 direction =
                    (obj.transform.position - firePoint.position).normalized;
            
            RaycastHit2D hit = Physics2D.Raycast(
                    startPosition,
                    direction,
                    autoShootRadius,
                    enemyLayer
                );
                
            if (hit.collider == null)
                continue;

            LineRenderer curLaser = lasers[laserIndex];

            

            curLaser.enabled = true;

            curLaser.SetPosition(0, firePoint.position);
            curLaser.SetPosition(1, hit.point);

            rb.AddForce(
                direction * laserPower,
                ForceMode2D.Impulse
            );

            laserIndex ++ ;
            doDamage(hit.collider.gameObject);
        }
        
        // turn off any unused lasers
        for (int i = laserIndex; i < numLasers ; i ++)
        {
            lasers[i].enabled = false;
        }

    }

    public void doDamage(GameObject enemyObj)
    {
        if (damageTimer > 0f)
            return;
        
        damageTimer = damageInterval;
        EnemyStats stats = enemyObj.GetComponent<EnemyStats>();
        stats.hitPoints -= 50;

        // turn on hit fx here
        EnemyBehavior eb = enemyObj.GetComponent<EnemyBehavior>();
        eb.hitSpark.Play();

        if (stats.hitPoints <= 0)
        {
            EnemySpawn.allEnemies.Remove(enemyObj);
            Destroy(enemyObj);
            enemySpawner.SpawnEnemyAlongWall();
        }

    }

    public void DisableLasers()
    {
        foreach (LineRenderer laser in lasers)
        {
            laser.enabled = false;
            
        }
    }

    void KeyboardInputs()
    {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame || (Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            lasersEnabled = !lasersEnabled;
        }

        if (lasersEnabled)
        {
            AutoShoot();
        }
        else
        {
            DisableLasers();
        }
    }


}
