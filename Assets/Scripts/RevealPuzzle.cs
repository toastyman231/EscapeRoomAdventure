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

    [SerializeField] private AudioClip movingSound;

    private Vector3 _oldPuzzleStartLocation;

    private Vector3 _newPuzzleStartLocation;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _oldPuzzleStartLocation = oldPuzzle.transform.position;
        _newPuzzleStartLocation = newPuzzle.transform.position;
    }

    public void HideOldPuzzle()
    {
        _audioSource.clip = movingSound;
        _audioSource.Play();
        LeanTween.move(oldPuzzle, oldPuzzleMoveToLocation, moveTime).setDelay(moveDelay).setOnComplete(ShowNewPuzzle);
    }

    public void HideNewPuzzle()
    {
        _audioSource.clip = movingSound;
        _audioSource.Play();
        LeanTween.move(newPuzzle, _newPuzzleStartLocation, moveTime).setDelay(moveDelay).setOnComplete(ShowOldPuzzle);
    }

    private void ShowOldPuzzle()
    {
        LeanTween.move(oldPuzzle, _oldPuzzleStartLocation, moveTime).setOnComplete(() =>
        {
            _audioSource.Stop();
        });
    }

    private void ShowNewPuzzle()
    {
        LeanTween.moveLocal(newPuzzle, newPuzzleMoveLocation, moveTime).setOnComplete(() =>
        {
            _audioSource.Stop();
        });
    }
}
