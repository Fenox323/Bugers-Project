using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TreeEditor;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomOrder : Order
{
    public override int[] ingredients { get; set; }
    protected override GameObject clientSpeechBox { get; set; }
    protected override GameObject clientSpeechBoxObj { get; set; }
    public override float patience { get; protected set; }
    public override float score { get; protected set; }

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

        return sortedIngredients.ToArray();
    }

    private GameObject CreateBurgerObject(int[] arr)
    {
        GameObject burgerObject = new GameObject("Random Burger Object");

        // Offset the burger based on its size
        float bottomDistance = arr.Length * -0.1f;

        // Add ingredients to the burger with a distance between them
        foreach (int i in arr)
        {
            GameObject ingredient = Instantiate(visualIngredients[i]);

            ingredient.transform.SetParent(burgerObject.transform, true);
            ingredient.transform.localPosition = new Vector3(0, bottomDistance, 0);

            bottomDistance += 0.2f;
        }

        return burgerObject;
    }

    public override void ShowBurgerOrder()
    {
        clientSpeechBoxObj.transform.SetParent(transform.parent.Find("Speech Bubble"), true);
        clientSpeechBoxObj.transform.localPosition = Vector3.zero;
    }
}
