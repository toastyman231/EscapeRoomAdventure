using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryCutscene : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private TextMeshProUGUI victoryText;
 
    [SerializeField] private GameObject door;

    [SerializeField] private GameObject victoryUI;

    [SerializeField] private CountdownTimer timer;

    [SerializeField] private float moveDuration;

    [SerializeField] private float uiDuration;

    [SerializeField] private AudioClip doorSound;

    private void Start()
    {
        GetComponent<AudioSource>().clip = doorSound;
    }

    public void StartCutscene()
    {
        timer.StopTimer();
        TimeSpan gameTime = TimeSpan.FromSeconds(timer.GetTimeLimit() - timer.GetGameTime().TotalSeconds);

        victoryText.text = $"You escaped!\nYour time: {gameTime.Minutes:D2}m:{gameTime.Seconds:D2}s";

        PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        controller.SetCanMove(false);
        controller.SetInventoryAvailable(false);

        if (InventoryUIController.ShowingInventory)
        {
            InventoryUIController.InvokeHideInventory();
        }

        PauseController pauseController = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>();
        if (pauseController.Paused)
        {
            pauseController.HidePause();
        }
        pauseController.CanPause = false;

        Camera.main.GetComponent<MouseLook>().SetCanLook(false, false, false);
        LeanTween.move(Camera.main.gameObject, cameraTarget.position, moveDuration).setOnComplete(Victory);
        LeanTween.rotate(Camera.main.gameObject, 
            new Vector3(cameraTarget.forward.x, cameraTarget.forward.y + 90, cameraTarget.forward.z), moveDuration);
    }

    private void Victory()
    {
        LeanTween.moveY(door, door.transform.position.y + 6f, moveDuration)
            .setOnComplete(() =>
            {
                Camera.main.GetComponent<MouseLook>().SetCanLook(false);
                LeanTween.scaleY(victoryUI, 1f, uiDuration);
                GetComponent<AudioSource>().Stop();
            });
    }
}
