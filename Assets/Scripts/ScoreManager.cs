using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject player;
    PlayerController pc;

    int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "INFILDELS VAPORIZED: " + score.ToString();
        pc = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        score = pc.enemiesKilled;
        scoreText.text = "INFILDELS VAPORIZED: " + score.ToString();
    }
}
