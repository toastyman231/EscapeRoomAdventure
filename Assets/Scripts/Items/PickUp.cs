using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private string objectName;

    [SerializeField] private float attractionForce;

    private bool _pickedUp;

    private Rigidbody _rb;

    private PlayerInteraction _playerInteraction;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_pickedUp && Vector3.Distance(transform.position, _playerInteraction.HeldObjectTarget()) > 0.1f)
        {
            Vector3 target = _playerInteraction.HeldObjectTarget();

            Vector3 difference = target - transform.position;
            _rb.AddForce(difference * attractionForce);
        } 
        
        if (_pickedUp && Input.GetButtonDown("Interact")) StartCoroutine(DropObject());
    }

    private void PickUpObject()
    {
        Debug.Log("Picked up");
        _pickedUp = true;
        _playerInteraction.SetInteraction(false);
        _rb.useGravity = false;
        _rb.drag = 10f;
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        transform.parent = _playerInteraction.transform;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private IEnumerator DropObject()
    {
        Debug.Log("Dropped object!");
        _pickedUp = false;
        _playerInteraction.SetInteraction(true);
        _rb.useGravity = true;
        _rb.drag = 1f;
        _rb.constraints = RigidbodyConstraints.None;
        transform.parent = null;

        yield return null;
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public void MouseOver()
    {
        // TODO: Add mouse over
        InteractionUIController.ShowInteractionUi("Pick Up " + objectName);
    }

    public void Interact()
    {
        if (_playerInteraction == null)
            _playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();

        PickUpObject();
    }
}
