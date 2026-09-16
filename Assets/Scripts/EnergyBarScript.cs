using UnityEngine;

public class EnergyBarScript : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {   
        
        float healthPercent = Mathf.Clamp01(
            PlayerController.playerHealth / PlayerController.maxPlayerHealth
        );

        transform.localScale = new Vector3(
            originalScale.x * healthPercent,
            originalScale.y,
            originalScale.z
        );
        

    }
}