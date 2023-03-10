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
            InteractionUIController.HideInteractionUi();
            return;
        }

        if (_canInteract)
        {
            _currentInteractable.MouseOver();
        }

        if (_canInteract && Input.GetButtonDown("Interact"))
        {
            _currentInteractable.Interact();
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

    public void SetInteraction(bool canInteract)
    {
        _canInteract = canInteract;
    }

    public Vector3 HeldObjectTarget()
    {
        Vector3 target = Camera.main.transform.position;
        target += GetComponent<PlayerController>().GetDirection().forward
                  * (interactDistance - 0.5f);
        return target;
    }
}
