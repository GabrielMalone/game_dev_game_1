using UnityEngine;

public class EnergyBarScript : MonoBehaviour
{
    private Vector3 originalScale;
    private Rigidbody2D rb;

    [Header("Energy Bar Indicator Settings")]
    public LineRenderer energyIndicatorLine;
    public float radius = 5f;
    // public Color cooldownColor = Color.blue;
    // public Color readyColor = Color.pink;
    // public float hdrIntensity = 3f;

    void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponentInParent<Rigidbody2D>();
        Debug.Log(rb);
    }

    void Update()
    {   
        drawEnergyRemaining();
        // float healthPercent = Mathf.Clamp01(
        //     PlayerController.playerHealth / PlayerController.maxPlayerHealth
        // );

        // transform.localScale = new Vector3(
        //     originalScale.x * healthPercent,
        //     originalScale.y,
        //     originalScale.z
        // );
    }


    void drawEnergyRemaining()
    {
        energyIndicatorLine.enabled = true;
        energyIndicatorLine.useWorldSpace = true;
        energyIndicatorLine.loop = false;

        energyIndicatorLine.positionCount = (int)PlayerController.playerHealth;

        int maxHealth = (int)PlayerController.maxPlayerHealth;
        int playHealthInt = (int)PlayerController.playerHealth;


        Vector3 center = rb.transform.position;

        for (int i = 0 ; i < playHealthInt ; i++)
        {   
            // 2pi / seg == how much angle between each point
            // i tells us which point currently calculating
            float distanceBetweenPoints = 2f * Mathf.PI / PlayerController.maxPlayerHealth;
            float angle = i * distanceBetweenPoints;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            energyIndicatorLine.SetPosition(
                i,
                center + new Vector3(x, y, 0)
            );
        }       
    }



}