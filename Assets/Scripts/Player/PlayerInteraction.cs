using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactLayer;

    private IInteractable _currentInteractable;

    private GameObject _previousInteractable;

    private PlayerController _controller;

    private bool _canInteract;

    private bool _usingItem;

    // Start is called before the first frame update
    void Start()
    {
        _canInteract = true;
        _usingItem = false;
        _controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        IInteractable temp = CheckForInteractable();

        if (temp == null || temp != _currentInteractable)
        {
            _currentInteractable?.MouseExit();
        }

        _currentInteractable = temp;

        if (_currentInteractable == null && !_usingItem)
        {
            InteractionUIController.HideInteractionUi();
            return;
        }

        if (_canInteract)
        {
            _currentInteractable?.MouseOver();
        }

        if (_canInteract && Input.GetButtonDown("Interact"))
        {
            _currentInteractable?.Interact();
        }
    }

    private IInteractable CheckForInteractable()
    {
        if (Physics.Raycast(Camera.main.transform.position, _controller.GetDirection().forward, 
                out var hit, interactDistance, interactLayer))
        {
            _previousInteractable = hit.transform.gameObject;
            return _previousInteractable.GetComponent<IInteractable>();
        }

        return null;
    }

    private IEnumerator InteractCoroutine(bool canInteract)
    {
        yield return null;
        _canInteract = canInteract;
    }

    public void SetInteraction(bool canInteract)
    {
        //_canInteract = canInteract;
        StartCoroutine(InteractCoroutine(canInteract));
    }

    public void SetUsingItem(bool usingItem)
    {
        _usingItem = usingItem;
    }

    public Vector3 HeldObjectTarget()
    {
        Vector3 target = Camera.main.transform.position;
        target += GetComponent<PlayerController>().GetDirection().forward
                  * (interactDistance - 0.5f);
        return target;
    }
}
