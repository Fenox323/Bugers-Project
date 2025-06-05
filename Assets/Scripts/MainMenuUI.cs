using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject difficultyButton;
    [SerializeField] private TextMeshProUGUI standardHighScore;
    [SerializeField] private TextMeshProUGUI timeAttackHighScore;

    private void Start()
    {
        standardHighScore.text = "High Score: " + MainManager.Instance.LoadHighScore("Standard").ToString();
        timeAttackHighScore.text = "High Score: " + MainManager.Instance.LoadHighScore("Time Attack").ToString();
    }

    public void PressedStandard()
    {
        MainManager.Instance.gamemode = 0;
        MainManager.Instance.StartGame();
    }
    public void PressedTimeAttack()
    {
        MainManager.Instance.gamemode = 1;
        MainManager.Instance.StartGame();
    }
    public void PressedEndless()
    {
        MainManager.Instance.gamemode = 2;
        MainManager.Instance.StartGame();
    }
    public void PressedEndlessDifficulty()
    {
        MainManager.Instance.CycleDifficulty();
        ChangeColor(MainManager.Instance.difficulty);
    }
    public void PressedQuit()
    {
        MainManager.Instance.QuitGame();
    }

    void ChangeColor(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                difficultyButton.GetComponent<Image>().color = Color.green; break;
            case 1:
                difficultyButton.GetComponent<Image>().color = Color.yellow; break;
            case 2:
                difficultyButton.GetComponent<Image>().color = Color.red; break;
        }
    }
}
