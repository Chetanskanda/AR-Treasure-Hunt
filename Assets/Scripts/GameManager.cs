using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject endPanel;

    [Header("Game UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalScoreText;

    [Header("Game Settings")]
    public float gameDuration = 60f;

    private int _score = 0;
    private float _timeRemaining;
    private bool _gameActive = false;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowStartScreen();
    }

    void Update()
    {
        if (!_gameActive) return;

        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0f)
        {
            _timeRemaining = 0f;
            EndGame();
        }

        UpdateTimerUI();
    }

    public void StartGame()
    {
        _score = 0;
        _timeRemaining = gameDuration;
        _gameActive = true;

        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        endPanel.SetActive(false);

        UpdateScoreUI();
        UpdateTimerUI();

        TreasureSpawner spawner = FindObjectOfType<TreasureSpawner>();
        if (spawner != null) spawner.StartSpawning();
    }

    public void EndGame()
    {
        _gameActive = false;

        startPanel.SetActive(false);
        gamePanel.SetActive(false);
        endPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + _score;

        TreasureSpawner spawner = FindObjectOfType<TreasureSpawner>();
        if (spawner != null) spawner.StopSpawning();
    }

    public void RestartGame()
    {
        TreasureSpawner spawner = FindObjectOfType<TreasureSpawner>();
        if (spawner != null) spawner.ClearAllTreasures();
        StartGame();
    }

    public void AddScore(int points)
    {
        if (!_gameActive) return;
        _score += points;
        UpdateScoreUI();
    }

    public bool IsGameActive() => _gameActive;

    void ShowStartScreen()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + _score;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(_timeRemaining);
            timerText.text = "Time: " + seconds + "s";
        }
    }
}