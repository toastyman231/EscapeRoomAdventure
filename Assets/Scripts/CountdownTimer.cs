using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float timeLimit;

    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private float uiShowTime;

    private bool _shouldCount = true;

    private float _timeAtStart;

    // Start is called before the first frame update
    void Start()
    {
        _timeAtStart = Time.time;
        countdownText.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_shouldCount) return;

        UpdateCountdownText();
    }

    private void UpdateCountdownText()
    {
        TimeSpan gameTime = TimeSpan.FromSeconds(timeLimit - (Time.time - _timeAtStart));

        string timeLeft = $"{gameTime.Minutes:D2}m:\n{gameTime.Seconds:D2}s";

        countdownText.text = timeLeft;

        if (gameTime.TotalSeconds <= 0) OnCountdownEnd();
    }

    private void OnCountdownEnd()
    {
        _shouldCount = false;

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

        LeanTween.scaleY(gameOverUI, 1f, uiShowTime).setOnComplete(() =>
        {
            Camera.main.GetComponent<MouseLook>().SetCanLook(false);
        });
    }

    public float GetTimeLimit()
    {
        return timeLimit;
    }

    public TimeSpan GetGameTime()
    {
        return TimeSpan.FromSeconds(timeLimit - (Time.time - _timeAtStart));
    }

    public void StopTimer()
    {
        _shouldCount = false;
    }
}
