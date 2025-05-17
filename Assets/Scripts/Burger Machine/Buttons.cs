using Unity.VisualScripting;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private SpawnIngredient spawnScript;
    [SerializeField] private int ingredientToSpawn = -1;

    private void Start()
    {
        spawnScript = GameObject.Find("Machine").GetComponent<SpawnIngredient>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Button " + gameObject.name + " clicked.");
        spawnScript.Spawn(ingredientToSpawn);
    }
}
