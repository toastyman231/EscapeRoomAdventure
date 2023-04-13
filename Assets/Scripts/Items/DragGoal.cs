using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragGoal : MonoBehaviour
{
    [SerializeField] private DragObject dragObject;

    [SerializeField] private float moveTime;

    [SerializeField] private UnityEvent onComplete;

    private void OnTriggerEnter(Collider other)
    {
        dragObject.CanDrag = false;
        LeanTween.move(dragObject.gameObject, transform.position, moveTime).setOnComplete(() =>
        {
            onComplete.Invoke();
        });
    }
}
