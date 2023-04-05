using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float gravity;

    private MouseLook _mouseLook;

    private CharacterController _controller;

    private Transform _direction;

    private bool _canMove;

    private bool _inventoryAvailable;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mouseLook = GetComponentInChildren<MouseLook>();
        _direction = transform.GetChild(0);
        _canMove = true;
        _inventoryAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canMove) return;

        _controller.Move(Vector3.down * gravity * Time.deltaTime);
        MovePlayer();

        if (_inventoryAvailable && Input.GetButtonDown("Inventory"))
        {
            if (InventoryUIController.ShowingInventory)
            {
                InventoryUIController.InvokeHideInventory();
                _mouseLook.SetCanLook(true);
            }
            else
            {
                InventoryUIController.InvokeShowInventory();
                _mouseLook.SetCanLook(false);
            }
        }
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        _controller.Move(_direction.right * horizontal + _direction.forward * vertical);
    }

    public Transform GetDirection()
    {
        return _direction;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    public void SetInventoryAvailable(bool inv)
    {
        _inventoryAvailable = inv;
    }
}
