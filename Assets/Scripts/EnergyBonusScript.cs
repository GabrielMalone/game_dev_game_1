using UnityEngine;

public class EnergyBonusScript : MonoBehaviour
{
    [Header("Energy Bonus Settings")]
    public float energyBonus = 500f;
    public float survivalTime = 10f;
    public float colorFXduration = 2f;
    private float colorStartTime;
    private float startTime;
    private float delayedAvailability = 1f;

    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        destroyUncaughtBonus();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (Time.time - startTime < delayedAvailability)
            return;

        if (other.CompareTag("Player"))
        {
            PlayerController.playerHealth += energyBonus;
            Debug.Log("healing you up pal");
            Destroy(gameObject);
        }

    }

    void colorChange()
    {

    }

    void destroyUncaughtBonus()
    {
        if (Time.time - startTime > survivalTime)
        {
            Destroy(gameObject);
        }      
    }

}
