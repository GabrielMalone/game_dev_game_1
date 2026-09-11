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

    private GameObject currentTarget;


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

    public void Shoot()
    {
        if (lasers.Length == 0)
            return;
        LineRenderer curLaser = lasers[0];
        curLaser.enabled = true;

        Vector2 startPosition = firePoint.position;
        Vector2 direction = firePoint.up;
        RaycastHit2D hit = Physics2D.Raycast(
            startPosition,
            direction,
            1000f,
            enemyLayer
        );

        curLaser.SetPosition(0, firePoint.position);

        if (hit.collider != null)
        {
            // all good, render the laser
            curLaser.SetPosition(1, hit.point);
            Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(
                    direction * laserPower,
                    ForceMode2D.Impulse
                );
            }
            doDamage(hit.collider.gameObject);
        } else
        {
            curLaser.SetPosition(1, startPosition + direction * 1000);
        }
    }


    public void AutoShoot()
    {
        // 1. Check whether our current target is still valid
        if (currentTarget != null)
        {
            float distance = Vector2.Distance(
                firePoint.position,
                currentTarget.transform.position
            );

            // Target left our range
            if (distance > autoShootRadius)
            {
                currentTarget = null;
            }
        }

        // 2. If we don't have a target, find one
        if (currentTarget == null)
        {
            Collider2D[] objectsInRange =
                Physics2D.OverlapCircleAll(
                    firePoint.position,
                    autoShootRadius,
                    enemyLayer
                );

            foreach (Collider2D obj in objectsInRange)
            {
                if (!obj.CompareTag("Enemy"))
                    continue;

                currentTarget = obj.gameObject;
                break;
            }
        }

        // 3. Still no target? Turn laser off
        if (currentTarget == null)
        {
            DisableLasers();
            return;
        }

        // 4. Shoot the current target
        Vector2 startPosition = firePoint.position;

        Vector2 direction =
            ((Vector2)currentTarget.transform.position - startPosition).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            startPosition,
            direction,
            autoShootRadius,
            enemyLayer
        );

        // Something went wrong / target isn't hittable anymore
        if (hit.collider == null)
        {
            currentTarget = null;
            DisableLasers();
            return;
        }

        // all good, render the laser
        LineRenderer curLaser = lasers[0];
        curLaser.enabled = true;
        curLaser.SetPosition(0, firePoint.position);
        curLaser.SetPosition(1, hit.point);

        // some physical consequence of the lasser
        Rigidbody2D rb = currentTarget.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(
                direction * laserPower,
                ForceMode2D.Impulse
            );
        }

        doDamage(currentTarget);

    }

    public void doDamage(GameObject enemyObj)
    {
        // turn on hit fx here
        EnemyBehavior eb = enemyObj.GetComponent<EnemyBehavior>();
        eb.hitSpark.Play();
        if (damageTimer > 0f)
            return;
        
        damageTimer = damageInterval;
        EnemyStats stats = enemyObj.GetComponent<EnemyStats>();
        stats.hitPoints -= 50;

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

        if (lasersEnabled && !PlayerController.playerTurning)
        {
            Shoot();
        }
        else
        {
            DisableLasers();
        }
    }

    


}
