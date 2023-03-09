using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float gravity;

    private CharacterController _controller;

    private Transform _direction;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _direction = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        _controller.Move(Vector3.down * gravity * Time.deltaTime);
        MovePlayer();
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
}
