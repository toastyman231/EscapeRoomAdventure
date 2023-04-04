using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public bool Paused { get; private set; }

    [SerializeField] private Canvas pauseCanvas;

    [SerializeField] private MouseLook mouseLook;

    [SerializeField] private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        Paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        Paused = !Paused;
        pauseCanvas.enabled = Paused;
        mouseLook.SetCanLook(!Paused);
        controller.SetCanMove(!Paused);
    }

    public void HidePause()
    {
        TogglePause();
    }
}
