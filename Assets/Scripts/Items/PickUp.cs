using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, IInteractable
{
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
        if (_pickedUp)
        {
            Vector3 target = Camera.main.transform.position;
            target += _playerInteraction.gameObject.GetComponent<PlayerController>().GetDirection().forward 
                      * (_playerInteraction.GetDistance() - 0.5f);

            //Vector3 difference = target - transform.position;
            //_rb.velocity = Vector3.zero;
            //_rb.AddForce(difference.normalized * attractionForce, ForceMode.Impulse);
            transform.position = target;
        } 
        
        if (_pickedUp && Input.GetButtonDown("Interact")) DropObject();
    }

    private void PickUpObject()
    {
        Debug.Log("Picked up");
        _pickedUp = true;
        _playerInteraction.SetInteraction(false);
    }

    private void DropObject()
    {
        Debug.Log("Dropped object!");
        _pickedUp = false;
        _playerInteraction.SetInteraction(true);
    }

    public void MouseOver()
    {
        // TODO: Add mouse over
    }

    public void Interact()
    {
        if (_playerInteraction == null)
            _playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        //if (_pickedUp) DropObject();
        PickUpObject();
    }
}
