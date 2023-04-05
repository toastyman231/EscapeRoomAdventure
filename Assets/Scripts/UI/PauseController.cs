using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public bool Paused { get; private set; }

    public bool CanPause { get; set; }

    [SerializeField] private Canvas pauseCanvas;

    [SerializeField] private MouseLook mouseLook;

    [SerializeField] private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
        CanPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanPause && Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        Paused = !Paused;
        controller.SetInventoryAvailable(!Paused);
        pauseCanvas.enabled = Paused;
        mouseLook.SetCanLook(!Paused);
        controller.SetCanMove(!Paused);
    }

    public void HidePause()
    {
        TogglePause();
    }
}
