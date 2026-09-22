using UnityEngine;

public class OrientationScript : MonoBehaviour
{
    public GameObject player;
    public LineRenderer orientationLine;
    public float orientationLength = 8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        drawOrientation();
    }


    void drawOrientation()
    {
        Vector3 start = player.transform.position; 
        Vector3 oritentation = player.transform.up;
        orientationLine.enabled = true;
        orientationLine.useWorldSpace = true;
        orientationLine.loop = false;

        orientationLine.SetPosition(0, start);
        orientationLine.SetPosition(1, start + (oritentation * orientationLength));
    }

}
