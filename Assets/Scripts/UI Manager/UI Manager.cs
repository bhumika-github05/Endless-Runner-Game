using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverScreen;

    private int score = 0;
    
    private void OnEnable()
    {
        ZombieController.OnZombieDeath += IncreaseScore;
    }

    private void OnDisable()
    {
        ZombieController.OnZombieDeath -= IncreaseScore;
    }

    private void IncreaseScore()
    {
        score += 1;
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void ShowGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    
}
    