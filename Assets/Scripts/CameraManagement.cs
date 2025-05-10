using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManagement : MonoBehaviour
{
    private Transform[] cameraPositions;
    private GameObject cameraObject;

    [SerializeField] private float cameraMoveTime, cameraRotateTime, cameraFOVTime;
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Awake()
    {
        cameraObject = transform.Find("Main Camera").gameObject;

        // Get camera positions
        cameraPositions = new Transform[transform.Find("Camera Positions").childCount];
        for (int i = 0; i < cameraPositions.Length; i++)
        {
            cameraPositions[i] = transform.Find("Camera Positions").transform.GetChild(i);
        }
    }

    public void ChangeView(int pos)
    {
        // Change camera position
        Debug.Log("Changing camera position to " + cameraPositions[pos].name);
        StartCoroutine(SmoothChangePosition(cameraPositions[pos].position, cameraMoveTime));
        StartCoroutine(SmoothChangeRotation(cameraPositions[pos].rotation, cameraRotateTime));
        ChangeFOV(pos);
    }

    void ChangeFOV(int pos)
    {
        // Change camera POV
        float FOV;
        switch (pos)
        {
            case 0: FOV = 70; break;
            case 1: FOV = 50; break;
            default: FOV = 50; break;
        }

        StartCoroutine(SmoothChangeFOV(FOV, cameraFOVTime));
    }

    IEnumerator SmoothChangeRotation(Quaternion target, float time)
    {
        Transform obj = cameraObject.transform;
        Quaternion start = obj.rotation;
        float elapsed = 0;

        while (elapsed < time)
        {
            float t = elapsed / time;
            float easedT = easingCurve.Evaluate(t);

            // Move towards the target
            obj.rotation = Quaternion.Slerp(start, target, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.rotation = target;
    }
    IEnumerator SmoothChangePosition(Vector3 target, float time)
    {
        Transform obj = cameraObject.transform;
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
    }

    IEnumerator SmoothChangeFOV(float target, float time)
    {
        Camera obj = cameraObject.GetComponent<Camera>();
        float start = obj.fieldOfView;
        float elapsed = 0;

        while (elapsed < time)
        {
            float t = elapsed / time;
            float easedT = easingCurve.Evaluate(t);

            // Move towards the target
            obj.fieldOfView = Mathf.SmoothStep(start, target, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.fieldOfView = target;
    }
}
