using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class KeypadInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent OnCompletedEvent;

    [SerializeField] private string sceneToAdd;

    private bool _keyPadUsed = false;

    void Start()
    {
        OnCompletedEvent.AddListener(OnComplete);
    }

    public void MouseOver()
    {
        if (_keyPadUsed) return;

        gameObject.layer = LayerMask.NameToLayer("Outline");

        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Outline");
        }

        InteractionUIController.ShowInteractionUi("Use Keypad");
    }

    public void MouseExit()
    {
        gameObject.layer = LayerMask.NameToLayer("Interact");

        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void Interact()
    {
        if (_keyPadUsed) return;

        _keyPadUsed = true;
        SceneManager.LoadSceneAsync(sceneToAdd, LoadSceneMode.Additive);

        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MouseLook>().SetCanLook(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetCanMove(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetInventoryAvailable(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>().CanPause = false;
    }

    private void OnComplete()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MouseLook>().SetCanLook(true);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetCanMove(true);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetInventoryAvailable(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>().CanPause = true;

        MouseExit();
    }
}
