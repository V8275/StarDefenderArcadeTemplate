using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner[] enemySpawner;
    [SerializeField]
    private HealthAbility playerHealth;

    [Header("UI Data")]
    [SerializeField]
    private TextMeshProUGUI hPText;
    [SerializeField]
    private string defaultStringHP;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private string defaultStringScore;

    [Header("UI Panels")]
    [SerializeField]
    private UIView pauseMenu;
    [SerializeField]
    private UIView playData;
    [SerializeField]
    private UIView endGame;

    private int currentScore = 0;

    private void Start()
    {
        foreach (var item in enemySpawner)
            item.OnScoreEnemy += UpdateScore;

        playerHealth.OnHealthChanged += UpdateHP;

        UpdateHP(playerHealth.MaxHealth, playerHealth.MaxHealth);
        UpdateScore(currentScore);
    }

    private void UpdateHP(int currentHealth, int maxHealth)
    {
        hPText.text = defaultStringHP + currentHealth + " / " + maxHealth;

        if(currentHealth <= 0) EndGame();
    }

    private void UpdateScore(int score)
    {
        currentScore += score;
        scoreText.text = defaultStringScore + currentScore;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        pauseMenu.Show();
        playData.Hide();
    }

    public void CoutinueGame()
    {
        Time.timeScale = 1;

        pauseMenu.Hide();
        playData.Show();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void EndGame()
    {
        Time.timeScale = 0;

        playData.Hide();
        endGame.Show();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        foreach (var item in enemySpawner)
            item.OnScoreEnemy -= UpdateScore;

        playerHealth.OnHealthChanged -= UpdateHP;
    }
}
