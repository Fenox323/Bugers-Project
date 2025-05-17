using Unity.VisualScripting;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private float randomPosRange;
    [SerializeField] private float randomRotRange;

    public virtual void Spawn()
    {
        transform.Translate(RandomPosition());
        transform.Rotate(RandomRotation());
    }

    protected Vector3 RandomPosition()
    {
        float x = Random.Range(-randomPosRange, randomPosRange);
        float y = 0;
        float z = Random.Range(-randomPosRange, randomPosRange);
        return new Vector3 (x, y, z);
    }

    protected Vector3 RandomRotation()
    {
        float x = Random.Range(-randomRotRange, randomRotRange);
        float y = Random.Range(-180, 180);
        float z = Random.Range(-randomRotRange, randomRotRange);
        return new Vector3(x, y, z);
    }
}
