using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject oldPuzzle;

    [SerializeField] private GameObject newPuzzle;

    [SerializeField] private Vector3 oldPuzzleMoveToLocation;

    [SerializeField] private Vector3 newPuzzleMoveLocation;

    [SerializeField] private float moveDelay;

    [SerializeField] private float moveTime;

    private Vector3 _oldPuzzleStartLocation;

    private Vector3 _newPuzzleStartLocation;

    private void Start()
    {
        _oldPuzzleStartLocation = oldPuzzle.transform.position;
        _newPuzzleStartLocation = newPuzzle.transform.position;
    }

    public void HideOldPuzzle()
    {
        LeanTween.move(oldPuzzle, oldPuzzleMoveToLocation, moveTime).setDelay(moveDelay).setOnComplete(ShowNewPuzzle);
    }

    public void HideNewPuzzle()
    {
        LeanTween.move(newPuzzle, _newPuzzleStartLocation, moveTime).setDelay(moveDelay).setOnComplete(ShowOldPuzzle);
    }

    private void ShowOldPuzzle()
    {
        LeanTween.move(oldPuzzle, _oldPuzzleStartLocation, moveTime);
    }

    private void ShowNewPuzzle()
    {
        LeanTween.moveLocal(newPuzzle, newPuzzleMoveLocation, moveTime);
    }
}
