using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // --- Public variables from previous setup ---
    public GameObject gameOverPanel;
    public PlayerController playerController;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI finalScoreText;

    // --- NEW: Audio variable ---
    public AudioClip backgroundMusic;

    private AudioSource audioSource;
    private int score = 0;
    private bool isGameOver = false;

    void Start()
    {
        // --- NEW: Setup and play background music ---
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateCoinText();
    }

    // ... rest of the GameManager code ...

    public void AddCoin()
    {
        if (isGameOver) return;
        score++;
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Score: " + score;
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + score;
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

