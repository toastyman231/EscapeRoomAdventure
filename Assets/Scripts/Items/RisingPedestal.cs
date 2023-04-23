using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPedestal : MonoBehaviour
{
    [SerializeField] private Vector3 moveToLocation;

    [SerializeField] private float moveTime;

    public void InvokePedestalRise()
    {
        LeanTween.moveLocal(gameObject, moveToLocation, moveTime);
    }
}
