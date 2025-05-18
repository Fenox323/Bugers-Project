using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static Queue queue { get; private set; }
    private CameraManagement mainCamera;
    [SerializeField] private Player player;
    private GameUI gameUI;
    

    [SerializeField] private List<PrefabPool> prefabs = new List<PrefabPool>();
    public List<GameObject> availableOrders = new List<GameObject>();

    public int activeSpot { get; private set; }
    private Vector3 startPos, exitPos, playerClientPos, playerBurgerPos;
    public GameObject activeBurger { get; set; }
    private int score = 0;
    public bool buttonsEnabled { get; set; } = true;

    [SerializeField] private float clientSpawnRate;

    [Serializable]
    public class PrefabPool
    {
        public GameObject obj;
        public int amount;
        public GameObject parent;
    }

    private void Awake()
    {
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
        // Pool objects at the start of the game
        foreach (PrefabPool prefab in prefabs)
        {
            ObjectPooling.Instance.PoolObject(prefab.obj, prefab.amount, prefab.parent);
        }
        // Create a new burger
        CreateBurger();

        // Start gameplay loop
        InvokeRepeating("CreateClient", 3f, clientSpawnRate);
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

    public GameObject CreateBurger()
    {
        activeBurger.transform.GetChild(0).localPosition = Vector3.zero;
        activeBurger.transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);
        activeBurger = CreateObject("Burger");
        activeBurger.transform.position = new Vector3(22.055f, 4.67f, 0.004f);
        activeBurger.transform.rotation = Quaternion.Euler(Vector3.zero);
        return activeBurger;
    }

    public void GoodBurger()
    {
        score += 10;
        gameUI.ChangeScore(score);
    }

    public void BadBurger()
    {
        score -= 20;
        gameUI.ChangeScore(score);
    }

    void CreateClient()
    {
        GameObject obj = CreateObject("Client");
        if (obj != null)
        {
            obj.GetComponent<Client>().Initialize();
        }
    }

    void OnSend()
    {
        if (activeSpot == 0) OrderLogic.Instance.OrderDone();
    }

    void OnMoveLeft()
    {
        gameUI.MoveLeftPressed();
    }

    void OnMoveRight()
    {
        gameUI.MoveRightPressed();
    }
}
