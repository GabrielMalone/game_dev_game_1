using UnityEngine;

public class EnemyFireWeapon : MonoBehaviour
{
    [Header("Enemy Missile Weapon")]
    public GameObject enemyMissile;
    public float timeToLive = 3f;
    public float chanceToFire = 0.01f;

    private GameObject currentMissile;
    private float timeShot;
    private bool missileShot = false;

    void Update()
    {
        FireWeapon();
        ExplodeMissile();
    }

    void FireWeapon()
    {
        if (
            AudioAnalyzer.trebleDetected 
            && !missileShot 
            && Random.value < chanceToFire
            )
        {
            missileShot = true;
            timeShot = Time.time;

            currentMissile = Instantiate(
                enemyMissile,
                transform.position,
                Quaternion.identity
            );
        }
    }

    void ExplodeMissile()
    {
        if (missileShot && Time.time - timeShot > timeToLive)
        {
            Destroy(currentMissile);

            currentMissile = null;
            missileShot = false;

            Debug.Log("missile exploded!");
        }
    }
}