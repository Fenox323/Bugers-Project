using UnityEngine;

public class TestBurger : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private float objectGirth;
    [SerializeField] private Quaternion rotation;

    // Update is called once per frame
    void Update()
    {
        float totalDistance = 0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            child.transform.localPosition = new Vector3(0, child.GetComponent<Renderer>().bounds.size.y / 2 + totalDistance, 0);
            child.transform.localRotation = rotation;

            totalDistance += distance + child.GetComponent<Renderer>().bounds.size.y * objectGirth;
        }
    }
}
