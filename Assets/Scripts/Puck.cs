using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Puck : MonoBehaviour
{
    public static Action onMissed;
    
    public void PuckLaunched()
    {
        StartCoroutine(Miss(3f));
    }

    private IEnumerator Miss(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        onMissed?.Invoke();
    }

}