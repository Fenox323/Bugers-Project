using System.Collections.Generic;
using NUnit.Framework;
using TreeEditor;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomOrder : Order
{
    protected override int[] ingredients { get; set; }
    protected override GameObject clientSpeechBox { get; set; }
    protected override GameObject clientSpeechBoxObj { get; set; }

    [SerializeField] private List<GameObject> visualIngredients;
    
    private void Awake()
    {
        ingredients = CreateRandomOrder();
        clientSpeechBoxObj = CreateBurgerObject(ingredients);
    }

    int[] CreateRandomOrder()
    {
        List<int> randomIngredients = new List<int>();
        List<int> sortedIngredients = new List<int>();
        
        bool hasTopBun = Random.value < 0.8f;
        bool hasTomato = Random.value < 0.4f;
        bool hasCheese = Random.value < 0.5f;
        bool hasLettuce = Random.value < 0.4f;
        bool hasMeat = Random.value < 0.95f;

        // Creating a burger
        sortedIngredients.Add(0);
        if (hasTomato)
        {
            for (int i = 0; i < Random.Range(1, 6); i++)
            {
                randomIngredients.Add(1);
            }
        }
        if (hasCheese)
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                randomIngredients.Add(2);
            }
        }
        if (hasLettuce)
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                randomIngredients.Add(3);
            }
        }
        if (hasMeat)
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                randomIngredients.Add(4);
            }
        }
        // Shuffle ingredients in the middle and add them to the burger
        Shuffle<int>(randomIngredients);
        sortedIngredients.AddRange(randomIngredients);
        //
        if (hasTopBun)
        {
            sortedIngredients.Add(0);
        }

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

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // pick a random index between 0 and i (inclusive)

            // Swap list[i] with list[j]
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
