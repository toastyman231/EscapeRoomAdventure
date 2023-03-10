using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionUIController : MonoBehaviour
{
    private static event EventHandler<UIEventArgs> ShowInteractionUIEvent;

    public static InteractionUIController Instance;

    [SerializeField] private TextMeshProUGUI interactText;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        interactText.enabled = false;
        ShowInteractionUIEvent += SetInteractText;
    }

    void OnDestroy()
    {
        ShowInteractionUIEvent -= SetInteractText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetInteractText(object sender, UIEventArgs args)
    {
        interactText.text = "E - " + args.Text;
        interactText.enabled = true;
    }

    public static void HideInteractionUi()
    {
        Instance.interactText.enabled = false;
    }

    public static void ShowInteractionUi(string text)
    {
        ShowInteractionUIEvent?.Invoke(null, new UIEventArgs{Text = text});
    }
}

public class UIEventArgs : EventArgs
{
    public string Text;
}