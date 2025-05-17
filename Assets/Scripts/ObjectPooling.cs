using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance { get; private set; }

    private List<GameObject> pooledPrefabs;
    private List<GameObject[]> pooledObjects;

    private void Awake()
    {
        Instance = this;

        pooledPrefabs = new List<GameObject>();
        pooledObjects = new List<GameObject[]>();
    }

    public void PoolObject(GameObject objectToPool, int amount, GameObject parent = null)
    {
        GameObject[] pooledObjectArray = new GameObject[amount];

        for (int i = 0; i < amount; i++)
        {
            // Instantiate an object and disable it in Hierarchy
            GameObject instance = Instantiate(objectToPool);
            instance.SetActive(false);
            // Push an object to an array
            pooledObjectArray[i] = instance;
        }
        // Give objects a parent
        if (parent != null)
        {
            for (int i = 0;i < amount; i++)
            {
                pooledObjectArray[i].transform.SetParent(parent.transform, true);
            }
        }
        // Add the new array with pooled objects to the pooled objects list
        pooledPrefabs.Add(objectToPool);
        pooledObjects.Add(pooledObjectArray);
    }

    public GameObject PullObject(GameObject objectToPull)
    {
        // Look for index of an array with objects to pull
        int poolIndex = GetPoolFromPrefab(objectToPull);
        if (poolIndex == -1) return null;

        // Loop through pooled objects in an array
        foreach (GameObject obj in pooledObjects[poolIndex])
        {
            // Check if object is not active in Hierarchy
            if (!obj.activeInHierarchy)
            {
                // Enable an object and return it
                obj.SetActive(true);
                return obj;
            }
        }

        // ERROR: All pooled objects have been pulled and haven't been returned
        Debug.LogError("Not enough \'" + objectToPull.name + "\' in pool list to pull from");
        return null;
    }

    int GetPoolFromPrefab(GameObject searchedPrefab)
    {
        // Loop through array with pooled prefabs and check if any matches the requested object
        for (int i = 0; i < pooledPrefabs.Count; i++)
        {
            if (pooledPrefabs[i] == searchedPrefab)
            {
                return i;
            }
        }

        // ERROR: There are no arrays with a requested object
        Debug.LogError("Requested object \'" + searchedPrefab.name + "\' has never been pooled");
        return -1;
    }

    public void ReleaseObject(GameObject objectToRelease)
    {
        // Deactivate an object and return it to the pool
        objectToRelease.SetActive(false);
    }
}
