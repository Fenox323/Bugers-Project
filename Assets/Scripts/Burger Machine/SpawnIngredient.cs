using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnIngredient : MonoBehaviour
{
    [SerializeField] private List<GameObject> ingredients;
    private Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition = transform.Find("Spawn Zone").transform.position;
    }

    public void Spawn(int ingredient)
    {
        if (ingredient >= 0 && GameManager.Instance.activeBurger.transform.childCount <= 15)
        {
            GameObject prefab = ingredients[ingredient];

            if (prefab != null)
            {
                GameObject obj = GameManager.Instance.CreateObject(prefab);
                if (obj != null)
                {
                    obj.transform.SetParent(GameManager.Instance.activeBurger.transform);
                    obj.transform.position = spawnPosition;
                    obj.transform.rotation = Quaternion.Euler(0, 0, 0);

                    Rigidbody rb = obj.GetComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.None;
                    rb.useGravity = true;

                    obj.GetComponent<Ingredient>().Spawn();
                }
            }
        }
    }
}
