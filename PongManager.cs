using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class PongManager : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private GameObject computerPaddleObject; 
    [SerializeField] private GameObject player2PaddleObject; 
    [SerializeField] private ComputerPaddle computerPaddle;
    [SerializeField] private Player2Paddle player2Paddle;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI computerScoreText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private Slider difficultySlider;
    [SerializeField] private TextMeshProUGUI difficultyText;

    private bool isPlayingAgainstBot = true;
    private int playerScore;
    private int computerScore;
    private bool isPaused = false;
    private int maxScore = 7;
    private const string PREFS_DIFFICULTY = "BotDifficulty";

    private void Start()
    {
        difficultyPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Player 2 ve Computer Paddle başlangıçta devre dışı
        computerPaddleObject.SetActive(false);
        player2PaddleObject.SetActive(false);

        if (!PlayerPrefs.HasKey(PREFS_DIFFICULTY))
        {
            PlayerPrefs.SetInt(PREFS_DIFFICULTY, 1);
            PlayerPrefs.Save();
        }


        int savedDifficulty = PlayerPrefs.GetInt(PREFS_DIFFICULTY);
        difficultySlider.value = savedDifficulty;
        UpdateDifficulty(savedDifficulty);

        difficultySlider.onValueChanged.AddListener(delegate { OnDifficultyChanged(); });

        Time.timeScale = 0; 
    }

    public void StartGameAgainstBot()
    {
        isPlayingAgainstBot = true;
        difficultySlider.gameObject.SetActive(true);

        player2PaddleObject.SetActive(false);
        computerPaddleObject.SetActive(true);

        StartGame();
    }

    public void StartGameAgainstPlayer()
    {
        isPlayingAgainstBot = false;
        difficultySlider.gameObject.SetActive(false);

        computerPaddleObject.SetActive(false);
        player2PaddleObject.SetActive(true);

        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        difficultyPanel.SetActive(false);
        NewGame();
    }

    public void OnDifficultyChanged()
    {
        int difficulty = (int)difficultySlider.value;
        PlayerPrefs.SetInt(PREFS_DIFFICULTY, difficulty);
        PlayerPrefs.Save();
        UpdateDifficulty(difficulty);
    }

    private void UpdateDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                difficultyText.text = "Easy";
                computerPaddle.SetSpeed(6f);
                break;
            case 1:
                difficultyText.text = "Medium";
                computerPaddle.SetSpeed(8f);
                break;
            case 2:
                difficultyText.text = "Hard";
                computerPaddle.SetSpeed(10f);
                break;
        }
    }

    public void NewGame()
    {
        SetPlayerScore(0);
        SetComputerScore(0);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        NewRound();
    }

    public void NewRound()
    {
        if (playerScore >= maxScore || computerScore >= maxScore)
        {
            GameOver();
            return;
        }

        playerPaddle.ResetPosition();
        if (isPlayingAgainstBot)
            computerPaddle.ResetPosition();
        else
            player2Paddle.ResetPosition();

        ball.ResetPosition();
        CancelInvoke();
        Invoke(nameof(StartRound), 1f);
    }

    private void StartRound()
    {
        ball.AddStartingForce();
    }

    public void OnPlayerScored()
    {
        SetPlayerScore(playerScore + 1);
        NewRound();
    }

    public void OnComputerScored()
    {
        SetComputerScore(computerScore + 1);
        NewRound();
    }

    private void SetPlayerScore(int score)
    {
        playerScore = score;
        playerScoreText.text = score.ToString();
    }

    private void SetComputerScore(int score)
    {
        computerScore = score;
        computerScoreText.text = score.ToString();
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);

        if (playerScore >= maxScore)
        {
            winnerText.text = isPlayingAgainstBot ? "Player Win!" : "Player 1 Win!";
        }
        else if (computerScore >= maxScore)
        {
            winnerText.text = isPlayingAgainstBot ? "Bot Win!" : "Player 2 Win!";
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        NewGame();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}