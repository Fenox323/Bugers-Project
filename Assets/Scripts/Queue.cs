using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Queue : MonoBehaviour
{
    [SerializeField] private float distanceBetweenClients;

    private List<GameObject> clientsInQueue = new List<GameObject>();

    public void AddToQueue(GameObject client)
    {
        clientsInQueue.Add(client);

        if (clientsInQueue.Count <= 7)
        {
            client.GetComponent<Client>().target = transform.position + (new Vector3(-distanceBetweenClients, 0, 0) * (clientsInQueue.Count - 1));
        }
    }

    public void RemoveFromQueue(GameObject client = null)
    {
        // Remove the client from queue
        if (client == null)
        {
            clientsInQueue.RemoveAt(0);
        } else
        {
            clientsInQueue.Remove(client);
        }

        // Move other clients up in queue
        MoveInQueue();
    }

    private void MoveInQueue()
    {
        for (int i = 0; i < clientsInQueue.Count && i < 7; i++)
        {
            clientsInQueue[i].GetComponent<Client>().target = transform.position + (new Vector3(-distanceBetweenClients, 0, 0) * i);
        }
    }
}
