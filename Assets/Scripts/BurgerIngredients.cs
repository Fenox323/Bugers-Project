using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BurgerIngredients : MonoBehaviour
{
    public List<int> ingredients  = new List<int>();

    private void OnDisable()
    {
        ingredients.Clear();
    }
}
