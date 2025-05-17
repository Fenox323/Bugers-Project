using UnityEngine;

public class OrderDoneButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        OrderLogic.Instance.OrderDone();
    }
}
