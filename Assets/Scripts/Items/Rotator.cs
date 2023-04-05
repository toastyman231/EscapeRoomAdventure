using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour, IInteractable
{
    [SerializeField] private CombinationLock comboLock;

    [SerializeField] private GameObject outlineObject;

    [SerializeField] private float rotateDuration;

    [ShowOnly] [SerializeField] private int _currentValue = 0;

    private bool _canRotate = true;

    private float _rotX = 0;

    public void Interact()
    {
        if (!_canRotate) return;

        _canRotate = false;
        _rotX = GetNewRotation(_rotX);
        LeanTween.value(gameObject, RotateCylinderCallback, _rotX + 36f, _rotX, rotateDuration).setOnComplete(OnRotate);
    }

    public void MouseExit()
    {
        outlineObject.layer = LayerMask.NameToLayer("Default");
    }

    public void MouseOver()
    {
        outlineObject.layer = LayerMask.NameToLayer("Outline");
        InteractionUIController.ShowInteractionUi("Rotate");
    }

    public void LockRotator()
    {
        _canRotate = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public int GetValue()
    {
        return _currentValue;
    }

    private void OnRotate()
    {
        _currentValue++;
        if (_currentValue > 9) _currentValue = 0;
        comboLock.CheckCombination();

        _canRotate = true;
    }

    private float GetNewRotation(float current)
    {
        float newRotation = current - 36f;

        if (newRotation < 0f)
        {
            newRotation += 360f;
        }

        if (newRotation > 360f)
        {
            newRotation -= 360f;
        }

        return newRotation;
    }

    private void RotateCylinderCallback(float value)
    {
        transform.eulerAngles = new Vector3(value, comboLock.transform.eulerAngles.y, comboLock.transform.eulerAngles.z);
    }
}
