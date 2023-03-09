using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public void MouseOver()
    {
        Debug.Log("Moused over!");
    }

    public void Interact()
    {
        Debug.Log("Interacted!");
    }
}
