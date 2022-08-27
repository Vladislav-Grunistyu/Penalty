using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public static Action onGoal;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Puck>())
        {
            onGoal?.Invoke();
        }
    }
}
