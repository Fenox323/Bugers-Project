using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Client : MonoBehaviour
{
    [SerializeField] private float patienceTimerInSec;
    [SerializeField] private float speed;
    public Vector3 target { get; set; }
    private GameObject order;
    private bool hasReceivedOrder;
    private List<GameObject> availableOrders = new List<GameObject>();

    void Awake()
    {
        // Setup the object
        availableOrders = GameManager.Instance.availableOrders;
    }

    public void Initialize()
    {
        // Set starting parameters
        order = availableOrders[Random.Range(0, availableOrders.Count)];
        hasReceivedOrder = false;
        transform.position = GameManager.Instance.GetPosition("Start");
        target = transform.position;

        // Add the client to queue
        GameManager.queue.AddToQueue(gameObject);

        // Move
        StartCoroutine(MoveToPosition());
    }

    // Upon entering the order zone
    public void Order()
    {
        // ABSTRACTION
        MakeOrder();
        StartCoroutine(PatienceStartTimer());
    }

    void MakeOrder()
    {
        // Create a new order and assign it to the client
        order = Instantiate(order);
        order.transform.SetParent(gameObject.transform);
    }

    public void Leave()
    {
        // Destroy the client and the order
        Destroy(order);
        GameManager.Instance.DestroyObject(gameObject);
    }

    IEnumerator PatienceStartTimer()
    {
        bool orderTimeout = false;
        float patience = 100;

        float patienceDecrease = 100f / patienceTimerInSec;

        // Waiting for receiving an order
        while (!hasReceivedOrder)
        {
            // Reduce the patience
            patience -= patienceDecrease * Time.deltaTime;
            // Check for timeout
            if (patience <= 0)
            {
                orderTimeout = true;
                break;
            }
            yield return null;
        }

        Debug.Log("Client lost patience.");

        if (orderTimeout)
        {
            // TODO: Fail an order - reference to GameManager
        }
        else
        {
            // TODO: Complete an order - reference to GameManager
        }

        // Remove from queue
        GameManager.queue.RemoveFromQueue(gameObject);

        // Move to exit
        target = GameManager.Instance.GetPosition("Exit");
    }

    IEnumerator MoveToPosition()
    {
        while (true)
        {
            // Check for distance to the target
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                // Move towards the target
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
            // Finish the movement to the target
            transform.position = target;
            Debug.Log("Movement finished.");

            yield return new WaitWhile(() => transform.position == target);
        }
    }
}

// Position in front of the desk: 10.26; 4; 4.32