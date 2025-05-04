using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;

    public void PressedStart()
    {
        MainManager.Instance.StartGame();
    }

    public void PressedQuit()
    {
        MainManager.Instance.QuitGame();
    }

    string GetPlayerName()
    {
        return playerName.text;
    }
}
