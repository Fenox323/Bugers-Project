using UnityEngine;

public class OrderZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Client>().Order();
    }
}
