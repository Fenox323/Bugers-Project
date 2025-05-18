using UnityEngine;

public class OrderDoneButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (GameManager.Instance.buttonsEnabled) OrderLogic.Instance.OrderDone();
    }
}
