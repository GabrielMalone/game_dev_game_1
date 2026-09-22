using UnityEngine;

public class EnemyDropScript : MonoBehaviour
{
    
    [SerializeField] private GameObject energyBonus;
    [SerializeField] private GameObject energyPenalty;
    private EnemyStats es;
    private bool energyDropped = false;

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

            if (Random.value < 0.5f)
            {
                GameObject dropItemGood = Instantiate(energyBonus, transform.position, Quaternion.identity);
            } 
            else
            {
                 GameObject dropItemBad = Instantiate(energyPenalty, transform.position, Quaternion.identity);
            } 
 
            energyDropped = true;
        }
    }
}
