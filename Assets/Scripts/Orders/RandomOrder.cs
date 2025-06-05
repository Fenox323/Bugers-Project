using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomOrder : Order
{
    public override int[] ingredients { get; set; }
    protected override GameObject clientSpeechBox { get; set; }
    protected override GameObject clientSpeechBoxObj { get; set; }
    public override float patience { get; protected set; }
    public override int score { get; protected set; }

    [SerializeField] private List<GameObject> visualIngredients;
    [SerializeField] private int ingredientsInBurger;
    
    private void Awake()
    {
        ingredients = CreateRandomOrder();
        clientSpeechBoxObj = CreateBurgerObject(ingredients);
    }

    int[] CreateRandomOrder()
    {
        List<int> sortedIngredients = new List<int>();
        int[] randomIngredients = GameManager.randomBurgerRandomizer.Randomize(ingredientsInBurger).Select(o => Convert.ToInt32(o)).ToArray();

        // Creating a burger
        sortedIngredients.Add(0);
        if (Random.value < 0.95f) sortedIngredients.Add(4);
        sortedIngredients.AddRange(randomIngredients);
        if (Random.value < 0.90f) sortedIngredients.Add(0);

        // Setting patience and scoring
        patience = sortedIngredients.Count * 2.2f;
        score = sortedIngredients.Count * 20;

        return sortedIngredients.ToArray();
    }

    private GameObject CreateBurgerObject(int[] arr)
    {
        GameObject burgerObject = new GameObject("Random Burger Object");

        float totalUpDistance = 0f;
        int renderOrder = 3001;

        // Add ingredients to the burger with a distance between them
        foreach (int i in arr)
        {
            GameObject ingredient = Instantiate(visualIngredients[i]);

            ingredient.transform.SetParent(burgerObject.transform, true);
            ingredient.transform.localPosition = new Vector3(0, totalUpDistance, 0);
            ingredient.GetComponent<Renderer>().material.renderQueue = renderOrder;

            totalUpDistance += 0.35f;
            renderOrder++;
        }

        return burgerObject;
    }

    public override void ShowBurgerOrder()
    {
        clientSpeechBoxObj.transform.SetParent(transform.parent.Find("Speech Bubble"), true);
        clientSpeechBoxObj.transform.localPosition = new Vector3 (0, 0, 0.1f) - GetBurgerCenter(clientSpeechBoxObj.transform)/5;
        clientSpeechBoxObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public Vector3 GetBurgerCenter(Transform obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("No renderers found!");
            return obj.position;
        }

        Bounds combinedBounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        return combinedBounds.center;
    }
}
