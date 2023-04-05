using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombinationLock : MonoBehaviour
{
    [SerializeField] private Vector3Int combination;

    [SerializeField] private Rotator[] rotators;

    [SerializeField] private UnityEvent onUnlocked;

    public void CheckCombination()
    {
        if (rotators[0].GetValue() == combination.x &&
            rotators[1].GetValue() == combination.y &&
            rotators[2].GetValue() == combination.z)
        {
            onUnlocked?.Invoke();

            foreach (Rotator rotator in rotators)
            {
                rotator.LockRotator();
            }
        }
    }
}
