using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class OrderLogic : MonoBehaviour
{
    public static OrderLogic Instance { get; private set; }
    private GameObject player;
    private GameUI gameUI;
    private GameObject ingredientsParent;

    private float throwForce = 10f;

    void Awake()
    {
        Instance = this;
        player = GameObject.Find("Player");
        gameUI = GameObject.Find("Canvas").GetComponent<GameUI>();
        ingredientsParent = GameObject.Find("Ingredients");
    }

    public void OrderDone()
    {
        // Move the burger
        GameManager.Instance.activeBurger.transform.SetParent(player.transform, true);
        // Switch camera
        gameUI.MoveRightPressed();
        // Create a new burger
        GameManager.Instance.CreateBurger();
    }

    public void GiveToCustomer(GameObject burger)
    {
        float time = 2f;

        if (Queue.Instance.clientsInQueue.Count > 0)
        {
            GameObject client = Queue.Instance.clientsInQueue[0];

            if (client.transform.GetComponentInChildren<Order>() != null)
            {
                // Transfer the burger to the client
                burger.transform.SetParent(client.transform, true);
                burger.transform.localPosition = new Vector3(0.166f, 0.368f, -0.737f);

                foreach (Rigidbody rb in burger.transform.GetComponentsInChildren<Rigidbody>())
                {
                    // Freeze physics
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    rb.useGravity = false;
                }

                // Make the client receive an order
                client.GetComponent<Client>().CompleteOrder(CheckOrder(burger, client));


                // Delete the burger
                StartCoroutine(MarkForDeletion(burger, time));
                return;
            }
        }
        // Launch the burger
        foreach (Rigidbody rb in burger.transform.GetComponentsInChildren<Rigidbody>())
        {
            burger.transform.SetParent(null, true);
            rb.AddForce(Vector3.forward * throwForce, ForceMode.Impulse);
        }
        // Lengthen the time for deletion
        time += 3f;
        // Delete the burger
        StartCoroutine(MarkForDeletion(burger, time));
    }

    IEnumerator MarkForDeletion(GameObject burger, float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = burger.transform.childCount - 1; i > 0; i--)
        {
            GameObject burgerChild = burger.transform.GetChild(i).gameObject;

            // Unfreeze physics
            burgerChild.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            burgerChild.GetComponent<Rigidbody>().useGravity = true;

            // Delete the ingredients and return parent
            if (burgerChild.CompareTag("Ingredient"))
            {
                burgerChild.transform.SetParent(ingredientsParent.transform, true);
                GameManager.Instance.DestroyObject(burgerChild);
            }
        }

        burger.transform.parent = null;
        GameManager.Instance.DestroyObject(burger);
    }

    bool CheckOrder(GameObject burger, GameObject client)
    {
        return true;
    }
}
