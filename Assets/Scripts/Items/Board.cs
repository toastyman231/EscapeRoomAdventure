using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IInteractable
{
    public void BreakBoard()
    {
        gameObject.SetActive(false);
    }

    public void MouseOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline");

        InteractionUIController.ShowInteractionUi("Remove Board");
    }

    public void MouseExit()
    {
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public void Interact()
    {
        DialogueController.InvokeShowDialogueEvent("This board feels loose... Maybe I could remove it with a <color=\"red\">tool<color=\"white\">...", 3f);
    }
}
