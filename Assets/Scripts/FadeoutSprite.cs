using UnityEngine;

public class FadeOutSprite : MonoBehaviour
{
    public float fadeDuration = 2f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color color = spriteRenderer.color;
        // timedelta = amnt of time since prev frame
        // now time passed = what fraction of fade should 
        // since alpha reprented by 0 --> 1 if just sub deltatime
        // alpha would go to zero in 1 second
        color.a -= Time.deltaTime/ 5;
        spriteRenderer.color = color;
        if (color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}