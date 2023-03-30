using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool interactable = true;
    [SerializeField] private RectTransform background;
    [SerializeField] private Vector3 bgScale;
    [SerializeField] private float scaleAmount;
    [SerializeField] private float clickScaleAmount;
    [SerializeField] private float duration;

    [SerializeField] private UnityEvent onClickEvent;

    private bool clicked;
    private Vector3 oldScale;
    private Vector3 oldBgScale;

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        LeanTween.color(background, Color.white, 0f);
        oldScale = transform.localScale;
        oldBgScale = background.localScale;
        LeanTween.scale(background, bgScale, duration);
        LeanTween.scale(gameObject, new Vector3(scaleAmount, scaleAmount), duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clicked)
        {
            CallButtonClickEvent();
        }

        LeanTween.cancel(gameObject);
        LeanTween.scale(background, oldBgScale, duration);
        LeanTween.scale(gameObject, oldScale, duration);
        LeanTween.color(background, Color.white, 0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) return;

        clicked = true;
        LeanTween.cancel(gameObject);
        LeanTween.color(background, Color.gray, duration);
        LeanTween.scale(gameObject, new Vector3(scaleAmount + clickScaleAmount,
            scaleAmount + clickScaleAmount), duration).setOnComplete(FinishClick(new Vector3(scaleAmount, scaleAmount)));
    }

    private Action FinishClick(Vector3 scale)
    {
        return () =>
        {
            LeanTween.cancel(gameObject);
            LeanTween.color(background, Color.white, duration);
            LeanTween.scale(gameObject, scale, duration).setOnComplete(CallButtonClickEvent);
        };
    }

    private void CallButtonClickEvent()
    {
        onClickEvent?.Invoke();
        clicked = false;
    }
}
