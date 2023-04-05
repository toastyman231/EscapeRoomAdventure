using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour, IInteractable
{
    [SerializeField] private CombinationLock comboLock;

    [SerializeField] private float rotateDuration;

    private Quaternion _targetRot;

    private bool _canRotate = true;

    private int _currentValue = 0;

    private float _rotX = 0;

    public void Interact()
    {
        if (!_canRotate) return;

        _rotX = GetNewRotation(_rotX);
        Quaternion newRotation = Quaternion.Euler(_rotX, 0f, 0f);
        //LeanTween.value(transform.parent.gameObject, RotateCylinderCallback, transform.parent.rotation, newRotation, rotateDuration);
        //Debug.Log("New rotation: " + _rotX);
        //LeanTween.rotateX(transform.parent.gameObject, _rotX, rotateDuration);
        //transform.parent.eulerAngles = new Vector3(_rotX, 0f, 0f);
        //LeanTween.rotate(transform.parent.gameObject, new Vector3(_rotX, 0f, 0f), rotateDuration);
        //StartCoroutine(RotateCylinder());
        //_targetRot = Quaternion.Euler(_rotX, 0f, 0f);
        //_canRotate = false;
        //Debug.Log("From " + transform.parent.eulerAngles.x + " to " + _rotX);
        //LeanTween.value(gameObject, RotateCylinderCallback, transform.parent.eulerAngles.x, _rotX, rotateDuration)
        //    .setOnComplete(() =>
        //{
        //    _currentValue++;
        //    _canRotate = true;
        //});
    }

    public void MouseExit()
    {
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void MouseOver()
    {
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Outline");
        InteractionUIController.ShowInteractionUi("Rotate");
    }

    private void LateUpdate()
    {
        //transform.parent.eulerAngles = new Vector3(transform.parent.eulerAngles.x, 0f, 0f);
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

    private IEnumerator RotateCylinder()
    {
        while (transform.parent.eulerAngles.x != _rotX)
        {
            transform.parent.Rotate(new Vector3(-36f, 0f, 0f) * rotateDuration * Time.deltaTime);
            yield return null;
        }
    }

    private void RotateCylinderCallback(Quaternion value)
    {
        //Debug.Log(value);
        transform.rotation = value;
    }
}
