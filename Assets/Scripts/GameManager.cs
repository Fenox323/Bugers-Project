using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static Queue queue { get; private set; }
    private CameraManagement mainCamera;
    [SerializeField] private Player player;
    private GameUI gameUI;

    private RandomWeighted orderRandomizer = new();
    public static RandomWeighted randomBurgerRandomizer = new();
    [SerializeField] private List<PrefabPool> prefabs = new();
    [SerializeField] private List<ObjectWeightPair> orders = new();
    [SerializeField] private List<float> burgerRandomizerValues = new();

    public int activeSpot { get; private set; }
    private Vector3 startPos, exitPos, playerClientPos, playerBurgerPos;
    public GameObject activeBurger { get; set; }
    private int score = 0;
    public bool buttonsEnabled { get; set; } = true;
    public int difficulty {  get; private set; }
    private int gamemode;
    public int randomIngredientsNumber { get; private set; } = 2;
    private int clientsServed = 0;
    private int clientsServedDiffThreshold = 7;
    public bool gameEnded { get; private set; }
    public float patienceMultiplier { get; private set; } = 1;

    [SerializeField] private float clientSpawnRate;

    [Serializable]
    public class ObjectWeightPair
    {
        public GameObject obj;
        public float weight;
    }

    [Serializable]
    public class PrefabPool
    {
        public GameObject obj;
        public int amount;
        public GameObject parent;
    }

    private void Awake()
    {
        try
        {
            gamemode = MainManager.Instance.gamemode;
            difficulty = MainManager.Instance.difficulty;
        }
        catch
        {
            gamemode = 0;
            difficulty = 0;
        }

        Instance = this;
        queue = GameObject.Find("Ordering Zone").GetComponent<Queue>();
        mainCamera = GameObject.Find("Camera").GetComponent<CameraManagement>();
        gameUI = GameObject.Find("Canvas").GetComponent<GameUI>();

        startPos = GameObject.Find("Start Zone").transform.position;
        exitPos = GameObject.Find("Exit Zone").transform.position;
        playerClientPos = GameObject.Find("Client Zone").transform.position;
        playerBurgerPos = GameObject.Find("Burger Zone").transform.position;
        activeSpot = 1;
    }

    private void Start()
    {
        InitializeGamemode();

        // Pool objects at the start of the game
        foreach (PrefabPool prefab in prefabs)
        {
            ObjectPooling.Instance.PoolObject(prefab.obj, prefab.amount, prefab.parent);
        }

        // Initialize order randomizer
        foreach (ObjectWeightPair pair in orders)
        {
            orderRandomizer.Add(pair.obj, pair.weight);
        }

        // Initialize burger randomizer
        for (int i = 0; i < burgerRandomizerValues.Count; i++)
        {
            randomBurgerRandomizer.Add(i, burgerRandomizerValues[i]);
        }

        // Create a new burger
        CreateBurger();

        // Start gameplay loop
        StartCoroutine(CreateClient());

        gameUI.ChangeScore(0);
    }

    public GameObject CreateObject(GameObject prefab)
    {
        return ObjectPooling.Instance.PullObject(prefab);
    }
    public GameObject CreateObject(string prefabName)
    {
        foreach (PrefabPool prefab in prefabs)
        {
            if (prefab.obj.name == prefabName) {
                return ObjectPooling.Instance.PullObject(prefab.obj);
            }
        }

        // ERROR: Prefab with a given name could not be found
        Debug.LogError("Prefab \'" + prefabName + "\' could not be found");
        return null;
    }

    public void DestroyObject(GameObject obj)
    {
        ObjectPooling.Instance.ReleaseObject(obj);
    }

    public Vector3 GetPosition(string zone)
    {
        switch (zone)
        {
            case "Start":
                return startPos;
            case "Exit":
                return exitPos;
            default:
                Debug.LogError("Position object not found.");
                return Vector3.zero;
        }
    }

    public Vector3 GetPlayerPosition(string zone)
    {
        switch (zone)
        {
            case "Client":
                return playerClientPos;
            case "Burger":
                return playerBurgerPos;
            default:
                Debug.LogError("Player position not found.");
                return Vector3.zero;
        }
    }

    public void SwitchSpot(int spot)
    {
        if (activeSpot != spot)
        {
            activeSpot = spot;
            mainCamera.ChangeView(spot);

            string zone = "";
            switch (spot)
            {
                case 0:
                    zone = "Burger"; break;
                case 1:
                    zone = "Client"; break;
            }
            Vector3 pos = GetPlayerPosition(zone);
            player.Move(pos);
        }
    }

    public GameObject GetRandomOrder()
    {
        if (gamemode == 2)
        {
            return orders[4].obj;
        }
        else
        {
            return (GameObject)orderRandomizer.Randomize();
        }
    }

    public GameObject CreateBurger()
    {
        activeBurger = CreateObject("Burger");
        activeBurger.transform.GetChild(0).localPosition = Vector3.zero;
        activeBurger.transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);
        activeBurger.transform.position = new Vector3(22.055f, 4.67f, 0.004f);
        activeBurger.transform.rotation = Quaternion.Euler(Vector3.zero);
        return activeBurger;
    }

    public void GoodBurger(GameObject order)
    {
        clientsServed++;
        if (gamemode == 0)
        {
            score += order.GetComponent<Order>().score;
            gameUI.ChangeScore(score);

            if (clientsServed == clientsServedDiffThreshold) {
                IncreaseDifficulty();
            }
        }
        else if (gamemode == 1)
        {
            score += order.GetComponent<Order>().score;
            gameUI.ChangeScore(score);
        }
        else if (gamemode == 2)
        {
            gameUI.ChangeScore(clientsServed);
        }
    }

    public void BadBurger()
    {
        if (gamemode == 0)
        {
            GameOver();
        }
        else if (gamemode == 1)
        {
            score -= 100;
            gameUI.ChangeScore(score);
        }
    }

    IEnumerator CreateClient()
    {
        yield return new WaitForSeconds(2);
        while (!gameEnded)
        {
            GameObject obj = CreateObject("Client");
            if (obj != null)
            {
                obj.GetComponent<Client>().Initialize();
            }
            yield return new WaitForSeconds(clientSpawnRate);
        }
    }

    void InitializeGamemode()
    {
        switch(gamemode)
        {
            case 0:
                clientSpawnRate = 6;
                randomIngredientsNumber = 2;
                gameUI.scoreText = "Score";
                break;

            case 1:
                clientSpawnRate = 1;
                randomIngredientsNumber = 4;
                gameUI.scoreText = "Score";
                patienceMultiplier *= 0.8f;
                gameUI.EnableTimeLeftUI();
                StartCoroutine(TimeAttackCountdown());
                break;

            case 2:
                clientSpawnRate = 2;
                randomIngredientsNumber = 2 + (difficulty * 4);
                gameUI.scoreText = "Clients";
                break;
        }
    }

    IEnumerator TimeAttackCountdown()
    {
        yield return new WaitForSeconds(2);
        int timeLeft = 60;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            gameUI.ChangeTime(timeLeft);
        }
        GameOver();
    }

    void IncreaseDifficulty()
    {
        if (clientSpawnRate > 1.5f) {
            clientSpawnRate -= 0.5f;
        }
        if (randomIngredientsNumber < 10)
        {
            randomIngredientsNumber++;
        }
        patienceMultiplier *= 0.95f;
    }

    void GameOver()
    {
        gameEnded = true;
        StopAllCoroutines();
        MainManager.Instance.CompareHighScore(score);
        gameUI.GameOver();
    }

    void OnSend()
    {
        if (!gameEnded && activeSpot == 0) OrderLogic.Instance.OrderDone();
    }

    void OnMoveLeft()
    {
        if (!gameEnded)
        {
            gameUI.MoveLeftPressed();
        }
    }

    void OnMoveRight()
    {
        if (!gameEnded)
        {
            gameUI.MoveRightPressed();
        }
    }

    void OnQuit()
    {
        GameOver();
    }
}
