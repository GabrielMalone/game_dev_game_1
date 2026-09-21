using UnityEngine;
using System.Collections;

public class MineCoolDown : MonoBehaviour
{
    [Header("Mine CoolDown Settings")]
    public LineRenderer coolDownIndicator;
    public float radius = 3f;
    public int maxCoolDownTime = 50;
    public int cooldownTime = 0;
    public Color cooldownColor = Color.blue;
    public Color readyColor = Color.pink;
    public float hdrIntensity = 3f;
    public int regenSpeed = 1;
    public int ogSpeed;

    Rigidbody2D rb;
    PlayerController pc; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
        StartCoroutine(startCoolDown());
        ogSpeed = regenSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        drawCoolDownLine();
    }

    IEnumerator startCoolDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            cooldownTime += regenSpeed;
            if (cooldownTime >= maxCoolDownTime){
                cooldownTime = maxCoolDownTime;
                pc.mineDropped = false;
            }
        }
    }

    void drawCoolDownLine()
    {
        coolDownIndicator.enabled = true;
        coolDownIndicator.useWorldSpace = true;
        coolDownIndicator.loop = false;

        coolDownIndicator.positionCount = cooldownTime;

        if (cooldownTime >= maxCoolDownTime)
        {
            coolDownIndicator.material.SetColor(
                "_Color",
                readyColor * hdrIntensity
            );

        } else {
            coolDownIndicator.material.SetColor(
                "_Color",
                cooldownColor * hdrIntensity / 2
            );            
        }

        Vector3 center = transform.position;

        for (int i = 0; i < cooldownTime; i++)
        {   
            // 2pi / seg == how much angle between each point
            // i tells us which point currently calculating
            float distanceBetweenPoints = 2f * Mathf.PI / maxCoolDownTime;
            float angle = i * distanceBetweenPoints;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            coolDownIndicator.SetPosition(
                i,
                center + new Vector3(x, y, 0)
            );
        }
    }

}
