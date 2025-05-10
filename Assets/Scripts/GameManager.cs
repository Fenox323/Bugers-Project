using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public static Queue queue;

    [SerializeField] private List<PrefabPool> prefabs = new List<PrefabPool>();
    public List<GameObject> availableOrders = new List<GameObject>();

    private Vector3 startPos, exitPos;

    [Serializable]
    public class PrefabPool
    {
        public GameObject obj;
        public int amount;
    }

    private void Awake()
    {
        Instance = this;
        queue = GameObject.Find("Ordering Zone").GetComponent<Queue>();

        startPos = GameObject.Find("Start Zone").transform.position;
        exitPos = GameObject.Find("Exit Zone").transform.position;
    }

    private void Start()
    {
        // Pool objects at the start of the game
        foreach (PrefabPool prefab in prefabs)
        {
            ObjectPooling.Instance.PoolObject(prefab.obj, prefab.amount);
        }
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

    void OnJump()
    {
        GameObject obj = CreateObject("Client");
        if (obj != null)
        {
            obj.GetComponent<Client>().Initialize();
        }
    }
}
