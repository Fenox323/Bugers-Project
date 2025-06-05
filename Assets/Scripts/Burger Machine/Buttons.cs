using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private SpawnIngredient spawnScript;
    [SerializeField] private int ingredientToSpawn = -1;
    private bool pressed = false;
    private Coroutine moveButtonCoroutine;

    private void Start()
    {
        spawnScript = GameObject.Find("Machine").GetComponent<SpawnIngredient>();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.buttonsEnabled) spawnScript.Spawn(ingredientToSpawn);

        if (moveButtonCoroutine != null) StopCoroutine(moveButtonCoroutine);
        moveButtonCoroutine = StartCoroutine(PressButton());
    }

    IEnumerator PressButton()
    {
        if (!pressed) {
            MoveButton(-0.05f);
            pressed = true;
        }
        yield return new WaitForSeconds(0.2f);

        pressed = false;
        MoveButton(0.05f);
        yield return null;
    }

    void MoveButton(float distance)
    {
        transform.Translate(0, 0, distance);
    }
}
