using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    private static event EventHandler<DialogueArgs> ShowDialogueEvent;

    [SerializeField] private float showEffectDuration;

    [SerializeField] private float fadeOutTime;

    private RectTransform _dialogueBox;

    private TextMeshProUGUI _dialogueText;

    private float originalAlpha;

    private bool _showingText;

    // Start is called before the first frame update
    void Start()
    {
        _dialogueBox = transform.GetChild(0).GetComponent<RectTransform>();
        _dialogueText = _dialogueBox.GetComponentInChildren<TextMeshProUGUI>();

        originalAlpha = _dialogueBox.GetComponent<Image>().color.a;

        ShowDialogueEvent += ShowDialogue;
    }

    void OnDestroy()
    {
        ShowDialogueEvent -= ShowDialogue;
    }

    private void ShowDialogue(object sender, DialogueArgs args)
    {
        _dialogueText.text = args.Text;

        if (_showingText)
        {
            StopAllCoroutines();
        }
        else
        {
            _showingText = true;
            LeanTween.scale(_dialogueBox, new Vector3(1f, 0f, 1f), 0f);
            LeanTween.alpha(_dialogueBox, 1f, 0f);
            TweenTextAlpha(originalAlpha, 0f);

            LeanTween.scale(_dialogueBox, new Vector3(1f, 1.5f, 1f), showEffectDuration).setOnComplete(() =>
            {
                LeanTween.scale(_dialogueBox, new Vector3(1f, 1f, 1f), showEffectDuration);
            });
        }

        if (args.Duration > 0f)
        {
            StartCoroutine(ShowTimedDialogue(args.Duration));
        }
    }

    private void TweenTextAlpha(float newAlpha, float time)
    {
        Color newColor = _dialogueText.color;
        newColor.a = newAlpha;
        LeanTween.value(_dialogueText.gameObject, UpdateTextAlphaCallback, _dialogueText.color, newColor, time);
    }

    private IEnumerator ShowTimedDialogue(float time)
    {
        yield return new WaitForSeconds(time);

        LeanTween.alpha(_dialogueBox, 0f, fadeOutTime);
        TweenTextAlpha(0f, fadeOutTime);
        _showingText = false;
    }

    private void UpdateTextAlphaCallback(Color val)
    {
        _dialogueText.color = val;
    }

    public static void InvokeShowDialogueEvent(string text, float duration = 1f)
    {
        ShowDialogueEvent?.Invoke(null, new DialogueArgs{Text = text, Duration = duration});
    }
}

public class DialogueArgs : EventArgs
{
    public string Text;
    public float Duration;
}
