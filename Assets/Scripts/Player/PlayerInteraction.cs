using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactLayer;

    private IInteractable _currentInteractable;

    private PlayerController _controller;

    private bool _canInteract;

    // Start is called before the first frame update
    void Start()
    {
        _canInteract = true;
        _controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentInteractable = CheckForInteractable();

        if (_currentInteractable == null)
        {
            //_canInteract = true;
            return;
        }

        if (_canInteract)
        {
            _currentInteractable.MouseOver();
        }

        if (_canInteract && Input.GetButtonUp("Interact"))
        {
            Debug.Log("Interacted!");
            _currentInteractable.Interact();
        }
    }

    private IInteractable CheckForInteractable()
    {
        if (Physics.Raycast(Camera.main.transform.position, _controller.GetDirection().forward, 
                out var hit, interactDistance, interactLayer))
        {
            return hit.transform.gameObject.GetComponent<IInteractable>();
        }

        return null;
    }

    public void SetInteraction(bool canInteract)
    {
        _canInteract = canInteract;
    }

    public float GetDistance()
    {
        return interactDistance;
    }
}
