using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float moveTime;

    public void Move(Vector3 pos)
    {
        StartCoroutine(SmoothChangePosition(pos, moveTime));
    }

    IEnumerator SmoothChangePosition(Vector3 target, float time)
    {
        Transform obj = transform;
        Vector3 start = obj.position;
        float elapsed = 0;

        while (elapsed < time)
        {
            float t = elapsed / time;
            float easedT = easingCurve.Evaluate(t);

            // Move towards the target
            obj.position = Vector3.Slerp(start, target, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.position = target;

        if (transform.childCount > 0)
        {
            OrderLogic.Instance.GiveToCustomer(transform.GetChild(0).gameObject);
        }
    }
}