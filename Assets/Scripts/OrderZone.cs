using UnityEngine;

public class OrderZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Client"))
        {
            other.gameObject.GetComponent<Client>().Order();
        }
    }
}
