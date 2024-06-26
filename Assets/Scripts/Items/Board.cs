using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip breakSound;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void BreakBoard()
    {
        _audioSource.clip = breakSound;
        _audioSource.Play();
        //gameObject.SetActive(false);
        transform.position = new Vector3(0, 10000, 0);
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
