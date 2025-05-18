using System.Collections;
using UnityEngine;

public class TrashButton : MonoBehaviour
{
    private bool isTrashEnabled = true;
    [SerializeField] private GameObject opening;

    private void OnMouseDown()
    {
        if (isTrashEnabled) {
            StartCoroutine(OrderLogic.Instance.MarkForDeletion(GameManager.Instance.activeBurger, 2f));
            StartCoroutine(TrashBurger());
        }
    }

    IEnumerator TrashBurger()
    {
        isTrashEnabled = false;
        GameManager.Instance.buttonsEnabled = false;

        opening.GetComponent<OpenTrash>().Open();
        yield return new WaitForSeconds(0.9f);

        opening.GetComponent<OpenTrash>().Close();
        yield return new WaitForSeconds(0.7f);

        GameManager.Instance.CreateBurger();

        isTrashEnabled = true;
        GameManager.Instance.buttonsEnabled = true;
    }
}
