using UnityEngine;
using UnityEngine.InputSystem;

//https://www.youtube.com/watch?v=S6eRVwAtfOM
public class Laser : MonoBehaviour
{

    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;

    [Header("Auto Laser Settings")]
    public GameObject playerShip;
    public float autoShootRadius = 30f;
    public LineRenderer[] lasers;
    public int numLasers = 5;

    [Header("Laser SFX")]
    public AudioSource laserStartAudio;
    public AudioSource laserBodyAudio;
    public AudioSource laserEndAudio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laserBodyAudio.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        autoShoot();
    }


    void createLasers()
    {

    }


    void EnableLaser()
    {
        lineRenderer.enabled = true;
        laserBodyAudio.loop = true;
        laserBodyAudio.Play();
        // laserEndAudio.Stop();
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
        laserBodyAudio.Stop();
        // laserEndAudio.Play();
    }


    void autoShoot()
    {
        Collider2D[] objectsInRange =
            Physics2D.OverlapCircleAll(playerShip.transform.position, autoShootRadius);

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
            
            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, autoShootRadius);

            if (hit.collider == null)
                continue;

            LineRenderer curLaser = lasers[laserIndex];

            curLaser.enabled = true;

            curLaser.SetPosition(0, startPosition);
            curLaser.SetPosition(1, hit.point);

            laserIndex ++ ;
        }

        for (int i = laserIndex; i < numLasers ; i ++)
        {
            lasers[i].enabled = false;
        }

    }


}
