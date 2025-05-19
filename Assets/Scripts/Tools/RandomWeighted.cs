using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using UnityEngine;

public class RandomWeighted
{
    Dictionary<object, float> mainList = new Dictionary<object, float>();
    float totalWeight = 0f;

    public void Add(object[] obj, float[] value)
    {
        if (obj.Length != value.Length)
        {
            Debug.LogError("Values count does not match their weight count!");
            return;
        }
        
        for (int i = 0; i < obj.Length; i++)
        {
            Add(obj[i], value[i]);
        }
    }

    public void Add(object obj, float value)
    {
        if (value <= 0f)
        {
            Debug.LogError("Weighed values need to be positive numbers");
            return;
        }
        if (mainList.ContainsKey(obj))
        {
            Debug.LogWarning("Randomizer already contains " + obj.GetType().Name + " " + obj.ToString() + " in its dictionary");
            Change(obj, value);
            return;
        }

        mainList.Add(obj, value);
        totalWeight += value;
    }

    public void Remove(object obj)
    {
        if (mainList.ContainsKey(obj))
        {
            totalWeight -= mainList[obj];
            mainList.Remove(obj);
        }
        else Debug.LogWarning("Randomizer does not have " + obj.GetType().Name + " " + obj.ToString() + " in its dictionary");
    }

    public void Change(object obj, float value)
    {
        if (value <= 0f)
        {
            Debug.LogError("Weighed values need to be positive numbers");
            return;
        }

        if (mainList.ContainsKey(obj))
        {
            totalWeight -= mainList[obj];
            mainList[obj] = value;
            totalWeight += value;
        }
        else Debug.LogWarning("Randomizer does not have " + obj.GetType().Name + " " + obj.ToString() + " in its dictionary");
    }

    public object Randomize()
    {
        if (mainList.Count == 0)
        {
            Debug.LogError("Randomizer has nothing to randomize");
            return null;
        }

        float count = 0f;
        float randomWeight = UnityEngine.Random.Range(0f, totalWeight);

        foreach (KeyValuePair<object, float> pair in mainList)
        {
            count += pair.Value;
            if (randomWeight <= count)
            {
                return pair.Key;
            }
        }

        Debug.LogError("Could not randomize an object");
        return null;
    }

    public object[] Randomize(int amount, bool unique = false)
    {
        if (amount <= 0 || mainList.Count == 0)
        {
            Debug.LogError("Randomizer has nothing to randomize");
            return null;
        }
        if (unique && amount > mainList.Count)
        {
            Debug.LogError("Randomizer does not have enough unique objects to randomize");
            return null;
        }

        object[] output = new object[amount];
        float tempTotalWeight = totalWeight;
        Dictionary<object, float> tempList = mainList;

        for (int i = 0; i < amount; i++)
        {
            float count = 0f;
            float randomWeight = UnityEngine.Random.Range(0f, tempTotalWeight);

            foreach (KeyValuePair<object, float> pair in tempList)
            {
                count += pair.Value;
                if (randomWeight <= count)
                {
                    output[i] = pair.Key;
                    if (unique && i != amount - 1)
                    {
                        tempTotalWeight -= pair.Value;
                        tempList.Remove(pair.Key);
                    }
                    break;
                }
            }
        }

        return output;
    }
}
