using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable, IUnlockable
{
    [SerializeField] private GameObject[] outlineObjects;

    [SerializeField] private GameObject lid;

    [SerializeField] private Material unlockedMaterial;

    [SerializeField] private float openAngle;

    [SerializeField] private float openTime;

    public void MouseOver()
    {
        foreach (GameObject obj in outlineObjects)
        {
            obj.layer = LayerMask.NameToLayer("Outline");
        }

        InteractionUIController.ShowInteractionUi("Open Box");
    }

    public void MouseExit()
    {
        foreach (GameObject obj in outlineObjects)
        {
            obj.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void Interact()
    {
        DialogueController.InvokeShowDialogueEvent("I need some kind of <color=\"red\">key<color=\"white\"> for this box...", 5f);
    }

    public void Unlock()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        LeanTween.rotateX(lid, openAngle, openTime)
            .setOnComplete(() =>
            {
                lid.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = unlockedMaterial;
            });
    }
}
