using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private static event EventHandler ToggleCrossHairEvent;

    [SerializeField] private GameObject crosshair;

    private void Start()
    {
        ToggleCrossHairEvent += ToggleCrosshair;
    }

    private void ToggleCrosshair(object sender, EventArgs args)
    {
        crosshair.SetActive(!crosshair.activeSelf);
    }

    public static void InvokeToggleCrossHairEvent()
    {
        ToggleCrossHairEvent?.Invoke(null, EventArgs.Empty);
    }
}
