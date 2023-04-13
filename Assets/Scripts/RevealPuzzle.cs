using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject oldPuzzle;

    [SerializeField] private GameObject newPuzzle;

    [SerializeField] private Vector3 moveLocation;

    [SerializeField] private Vector3 newPuzzleMoveLocation;

    [SerializeField] private float moveDelay;

    [SerializeField] private float moveTime;

    public void HideOldPuzzle()
    {
        //_newPuzzleMoveLocation = oldPuzzle.transform.position;
        LeanTween.move(oldPuzzle, moveLocation, moveTime).setDelay(moveDelay).setOnComplete(ShowNewPuzzle);
    }

    private void ShowNewPuzzle()
    {
        LeanTween.moveLocal(newPuzzle, newPuzzleMoveLocation, moveTime);
    }
}
