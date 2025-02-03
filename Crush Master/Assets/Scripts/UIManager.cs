using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// UIManager.cs
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalAttemptsText;

    private void Awake() => Instance = this;

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        GameManager.Instance.ResetGame();
    }

    public void ShowGameOver(int score, int attempts)
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {score}";
        finalAttemptsText.text = $"Total Attempts: {attempts}";
    }

    public void ReturnToMainMenu()
    {
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
