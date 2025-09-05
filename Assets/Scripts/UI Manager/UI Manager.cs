using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
        score += 1; // ✅ add 10 per zombie
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

    public void HideGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

}
    