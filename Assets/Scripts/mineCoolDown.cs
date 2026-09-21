using UnityEngine;
using System.Collections;

public class mineCoolDown : MonoBehaviour
{
    [Header("Mine CoolDown Settings")]
    public LineRenderer coolDownIndicator;
    public float radius = 3f;
    public int maxCoolDownTime = 50;
    int cooldownTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(startCoolDown());
        drawCoolDownLine();
    }

    IEnumerator startCoolDown()
    {
        cooldownTime ++;
        if (cooldownTime > maxCoolDownTime){
            cooldownTime = 0;
        }
        yield return new WaitForSeconds(0.3f);
    }

    void drawCoolDownLine()
    {
        coolDownIndicator.enabled = true;
        coolDownIndicator.useWorldSpace = true;
        coolDownIndicator.loop = false;

        coolDownIndicator.positionCount = cooldownTime;

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
