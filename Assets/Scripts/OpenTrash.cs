using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OpenTrash : MonoBehaviour
{
    public void Open()
    {
        StartCoroutine(RotateHatch(true, 0.3f));
    }

    public void Close()
    {
        StartCoroutine(RotateHatch(false, 0.6f));
    }

    IEnumerator RotateHatch(bool open, float time)
    {
        float elapsed = 0;
        float step = 0;
        float prevStep = 0;
        float direction = 75;
        if (open) direction *= -1;

        while (elapsed < time)
        {
            float t = elapsed / time;

            // Move towards the target
            step = Mathf.SmoothStep(0, direction, t);
            transform.RotateAround(new Vector3(20.185f, 4.621f, -0.047f), Vector3.forward, (step-prevStep));

            prevStep = step;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
