using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreboard;
    [SerializeField] private TextMeshProUGUI timeLeft;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject timeLeftUI;
    public string scoreText;

    private void Start()
    {
        transform.GetChild(GameManager.Instance.activeSpot).gameObject.SetActive(false);
    }

    public void MoveLeftPressed()
    {
        if (!GameManager.Instance.gameEnded) {
            GameManager.Instance.SwitchSpot(0);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void MoveRightPressed()
    {
        if (!GameManager.Instance.gameEnded)
        {
            GameManager.Instance.SwitchSpot(1);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false); 
        }
    }

    public void RestartPressed()
    {
        SceneManager.LoadScene("main");
    }

    public void QuitPressed()
    {
        SceneManager.LoadScene("menu");
    }

    public void ChangeScore(int score)
    {
        scoreboard.text = scoreText + ": " + score.ToString();
    }

    public void ChangeTime(int time)
    {
        timeLeft.text = "Time Left: " + time.ToString();
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void EnableTimeLeftUI()
    {
        timeLeftUI.SetActive(true);
    }
}
