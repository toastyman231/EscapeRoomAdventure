using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Material hoveredMaterial;

    [SerializeField] private Material notHoveredMaterial;

    [SerializeField] private float attractionForce;

    [SerializeField] private float waitTime;

    public bool CanDrag 
    {
        get => _canDrag;
        set
        {
            if (!value)
            {
                _rb.isKinematic = true;
                _rb.drag = 1f;
                _rb.useGravity = false;
                GetComponent<MeshRenderer>().material = notHoveredMaterial;
                Debug.Log("Disabled drag!");
            }
            _canDrag = value;
        }
    }

    private bool _canDrag;

    private Rigidbody _rb;

    private bool _pickedUp = false;

    private bool _mousedOver = false;

    private PlayerInteraction _playerInteraction;

    public void Interact()
    {
        return;
    }

    public void MouseExit()
    {
        StartCoroutine(StopObject());
        _mousedOver = false;
    }

    public void MouseOver()
    {
        if (!CanDrag) return;

        _mousedOver = true;
        if (_pickedUp) return;

        GetComponent<MeshRenderer>().material = hoveredMaterial;
        _pickedUp = true;
        _rb.drag = 10f;
    }

    private IEnumerator StopObject()
    {
        yield return new WaitForSeconds(waitTime);

        if (_mousedOver) yield return null;

        _pickedUp = false;
        GetComponent<MeshRenderer>().material = notHoveredMaterial;
        _rb.drag = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        CanDrag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_pickedUp && Vector3.Distance(transform.position, _playerInteraction.HeldObjectTarget()) > 0.1f)
        {
            Vector3 target = _playerInteraction.HeldObjectTarget();

            Vector3 difference = target - transform.position;
            _rb.AddForce(difference * attractionForce);
        }
    }
}
