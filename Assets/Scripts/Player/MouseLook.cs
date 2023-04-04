using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Values for fine tuning mouse control
    [SerializeField] private float mouseSensitivity = 100.0f;
    [SerializeField] private float clampAngle = 80.0f;

    private bool _canLook;

    private float _rotY = 0.0f; // Rotation around the up/y axis
    private float _rotX = 0.0f; // Rotation around the right/x axis

    void Start()
    {
        // Set up the rotation values with the camera's current angle on start
        Vector3 rot = transform.localRotation.eulerAngles;
        _rotY = rot.y;
        _rotX = rot.x;

        _canLook = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        mouseSensitivity = Mathf.Round(PlayerPrefs.GetFloat("sensitivity", 1f) * 100f);
    }

    void Update()
    {
        if (!_canLook) return;

        // Get the mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        // Calculate how much to rotate each axis
        _rotY += mouseX * mouseSensitivity * Time.deltaTime;
        _rotX += mouseY * mouseSensitivity * Time.deltaTime;

        // Clamp between the clamp angle so the player can't swivel their head upside down
        _rotX = Mathf.Clamp(_rotX, -clampAngle, clampAngle);

        // Apply the rotation
        Quaternion localRotation = Quaternion.Euler(_rotX, _rotY, 0.0f);
        transform.rotation = localRotation;
    }

    public void SetSensitivity(float amount)
    {
        mouseSensitivity = amount;
    }

    public void SetCanLook(bool canLook)
    {
        _canLook = canLook;
        Cursor.visible = !canLook;
        Cursor.lockState = (canLook) ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
