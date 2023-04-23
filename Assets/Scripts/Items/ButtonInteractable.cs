using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool toggleButton;

    [SerializeField] private Material offMat;

    [SerializeField] private Material onMat;

    [SerializeField] private UnityEvent onPressEvent;

    [SerializeField] private MeshRenderer _renderer;

    private bool _pressed;

    public void MouseOver()
    {
        transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Outline");
        InteractionUIController.ShowInteractionUi("Press Button");
    }

    public void MouseExit()
    {
        transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void Interact()
    {
        if (toggleButton)
        {
            if (!_pressed)
            {
                _pressed = true;
                _renderer.material = onMat;
                _renderer.transform.localPosition =
                    new Vector3(_renderer.transform.localPosition.x, 0.009f, _renderer.transform.localPosition.z);
                onPressEvent?.Invoke();
            }
            else
            {
                _pressed = false;
                _renderer.material = offMat;
                _renderer.transform.localPosition =
                    new Vector3(_renderer.transform.localPosition.x, 0.043f, _renderer.transform.localPosition.z);
            }
        }
        else
        {
            if (!_pressed)
            {
                _pressed = true;
                LeanTween.moveLocalY(_renderer.gameObject, 0.009f, 0.2f).setOnComplete(() =>
                {
                    //_renderer.material = onMat;
                    onPressEvent?.Invoke();
                    LeanTween.moveLocalY(_renderer.gameObject, 0.043f, 0.2f).setOnComplete(() =>
                    {
                        _pressed = false;
                    });
                });
            }
        }
    }
}
