using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    // Get spawnable object prefabs and the amount to pool
    [SerializeField] private List<PrefabPool> prefabs = new List<PrefabPool>();

    [Serializable]
    public class PrefabPool
    {
        public GameObject obj;
        public int amount;
    }

    private void Awake()
    {
        Instance = this;
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

    public void DestroyObject(GameObject obj)
    {
        ObjectPooling.Instance.ReleaseObject(obj);
    }
}
