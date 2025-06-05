using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class Order : MonoBehaviour
{
    public abstract int[] ingredients { get; set; }
    protected abstract GameObject clientSpeechBox { get; set; }
    protected abstract GameObject clientSpeechBoxObj { get; set; }
    public abstract float patience { get; protected set; }
    public abstract int score { get; protected set; }

    public virtual void ShowBurgerOrder()
    {
        clientSpeechBoxObj = Instantiate(clientSpeechBox);
        clientSpeechBoxObj.transform.SetParent(transform.parent.Find("Speech Bubble"), true);
        clientSpeechBoxObj.transform.localPosition = Vector3.zero;
        clientSpeechBoxObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public virtual void HideBurgerOrder()
    {
        Destroy(clientSpeechBoxObj);
    }
}
