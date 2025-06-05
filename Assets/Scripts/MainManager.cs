#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Overlays;

#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }
    public int gamemode;
    /*
     * 0 - Standard
     * 1 - Time Attack
     * 2 - Endless
    */
    public int difficulty = 0;

    [Serializable]
    public class HighScores
    {
        public int standardHighScore;
        public int timeAttackHighScore;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("main");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void CycleDifficulty()
    {
        difficulty = (difficulty + 1) % 3;
    }

    public int LoadHighScore(string mode)
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScores data = JsonUtility.FromJson<HighScores>(json);

            switch(mode)
            {
                case "Standard":
                    return data.standardHighScore;
                case "Time Attack":
                    return data.timeAttackHighScore;
            }
        }
        return 0;
    }

    public void CompareHighScore(int score)
    {
        switch (gamemode)
        {
            case 0:
                if (score > LoadHighScore("Standard"))
                {
                    SaveHighScore(score);
                }
                break;
            case 1:
                if (score > LoadHighScore("Time Attack"))
                {
                    SaveHighScore(score);
                }
                break;
        }
    }

    public void SaveHighScore(int score)
    {
        HighScores scoreboard = new HighScores();

        switch (gamemode)
        {
            case 0:
                if (score > LoadHighScore("Standard"))
                {
                    scoreboard.standardHighScore = score;
                    scoreboard.timeAttackHighScore = LoadHighScore("TimeAttack");
                }
                break;
            case 1:
                if (score > LoadHighScore("Time Attack"))
                {
                    scoreboard.standardHighScore = LoadHighScore("Standard");
                    scoreboard.timeAttackHighScore = score;
                }
                break;
        }

        string json = JsonUtility.ToJson(scoreboard);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
