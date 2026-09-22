using UnityEngine;

public class EnemyDropScript : MonoBehaviour
{
    
    
    [SerializeField] private GameObject energyBonus;
    private EnemyStats es;
    private bool energyDropped = false;


    // Instantiate(myPrefab, Vector3.zero, Quaternion.identity);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        es = GetComponentInParent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        DropEnergy();
    }

    void DropEnergy()
    {
        if (es.hitPoints <= 20 && !energyDropped)
        {
            Instantiate(energyBonus, transform.position, Quaternion.identity);
            energyDropped = true;
        }
    }
}
