using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Pool;

public class SequenceKeypad : MonoBehaviour
{
    [SerializeField] private int[] correctSequence;

    [SerializeField] private RectTransform overallPanel;

    [SerializeField] private GameObject resultsPanel;

    [SerializeField] private GameObject sequenceRowPrefab;

    [SerializeField] private int numGuesses;

    [SerializeField] private Color correctColor;

    [SerializeField] private Color partialCorrectColor;

    [SerializeField] private Color defaultColor;

    [SerializeField] private float showTime;

    [SerializeField] private float resetTime;

    [SerializeField] private float tweenTime;

    [SerializeField] private LayerMask layers;

    private IObjectPool<GameObject> _results;

    private int[] _currentSequence;

    private int _currentGuessIndex;

    private int _currentGuess;

    private bool _canGuess;

    void Start()
    {
        LeanTween.scaleY(transform.GetChild(0).gameObject, 1, showTime);
        _results = new ObjectPool<GameObject>(CreateSequenceRow, OnTakeFromPool, 
            OnReturnedToPool, OnDestroyPoolObject, true, 4, numGuesses);
        StartCoroutine(ResetPuzzle());
    }

    public IEnumerator ResetPuzzle(bool timedReset = false)
    {
        _canGuess = false;
        correctSequence = new[]{ Random.Range((int)0, (int)10), Random.Range((int)0, (int)10), Random.Range((int)0, (int)10) };
        _currentSequence = new int[3];
        _currentGuessIndex = 0;
        _currentGuess = 0;

        foreach (Transform child in resultsPanel.transform)
        {
            _results.Release(child.gameObject);
            if (timedReset) yield return new WaitForSeconds(resetTime);
        }

        for (int i = 0; i < numGuesses; i++)
        {
            _results.Get();
        }

        SetSelector();
        _canGuess = true;
    }

    public void SubmitPuzzle()
    {
        if (_currentGuessIndex < 3 || !_canGuess) return;

        int index = 0;
        foreach (Transform child in resultsPanel.transform.GetChild(_currentGuess))
        {
            if (index == 0 || index >= resultsPanel.transform.childCount)
            {
                index++;
                continue;
            }

            TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>(true);

            Image background = text.transform.parent.GetComponent<Image>();

            if (_currentSequence[index - 1] == correctSequence[index - 1])
            {
                background.color = correctColor;
            } else if (correctSequence.Contains(_currentSequence[index - 1]))
            {
                background.color = partialCorrectColor;
            }

            index++;
        }

        if (PuzzleSolved())
        {
            _canGuess = false;
            LeanTween.moveX(overallPanel, 2000f, tweenTime)
                .setOnComplete(() =>
                {
                    SceneManager.UnloadSceneAsync("SequenceGame");

                    if (Physics.Raycast(Camera.main.transform.position,
                            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>()
                                .GetDirection().forward,
                            out var hit, 4, layers))
                    {
                        KeypadInteractable keypad = hit.collider.gameObject.GetComponent<KeypadInteractable>();

                        keypad?.OnCompletedEvent.Invoke();
                    }
                });
            return;
        }

        _currentGuess++;
        if (_currentGuess >= numGuesses)
        {
            StartCoroutine(ResetPuzzle(true));
            return;
        }

        SetSelector();

        _currentSequence = new int[3];
        _currentGuessIndex = 0;
    }

    public void AddDigit(int digit)
    {
        if (_currentGuessIndex > 2 || !_canGuess) return;

        _currentSequence[_currentGuessIndex++] = digit;
        UpdateSequence();
    }

    public void ClearGuess()
    {
        if (!_canGuess) return;

        _currentGuessIndex = 0;
        _currentSequence = new int[3];

        int index = 0;
        foreach (Transform child in resultsPanel.transform.GetChild(_currentGuess))
        {
            if (index == 0 || index >= resultsPanel.transform.childCount)
            {
                index++;
                continue;
            }

            TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>(true);

            text.gameObject.SetActive(false);
            index++;
        }
    }

    private void UpdateSequence()
    {
        int index = 0;
        foreach (Transform child in resultsPanel.transform.GetChild(_currentGuess))
        {
            if (index == 0 || index >= resultsPanel.transform.childCount)
            {
                index++;
                continue;
            }
            
            TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>(true);

            text.text = _currentSequence[index - 1].ToString();
            if (index == _currentGuessIndex) text.gameObject.SetActive(true);
            index++;
        }
    }

    private bool PuzzleSolved()
    {
        if (_currentSequence[0] == correctSequence[0] && _currentSequence[1] == correctSequence[1] &&
            _currentSequence[2] == correctSequence[2])
        {
            return true;
        }

        return false;
    }

    private void SetSelector()
    {
        int index = 0;
        foreach (Transform child in resultsPanel.transform)
        {
            if (index == _currentGuess)
            {
                child.GetChild(0).gameObject.SetActive(true);
                child.GetChild(child.childCount - 1).gameObject.SetActive(true);
            }
            else
            {
                child.GetChild(0).gameObject.SetActive(false);
                child.GetChild(child.childCount - 1).gameObject.SetActive(false);
            }

            index++;
        }
    }

    private GameObject CreateSequenceRow()
    {
        return Instantiate(sequenceRowPrefab, resultsPanel.transform);
    }

    private void OnTakeFromPool(GameObject sequenceRow)
    {
        int index = 0;
        foreach (Transform child in sequenceRow.transform)
        {
            if (index == 0 || index >= resultsPanel.transform.childCount)
            {
                index++;
                continue;
            }

            TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>(true);

            Image background = text.transform.parent.GetComponent<Image>();

            background.color = defaultColor;

            text.gameObject.SetActive(false);
            index++;
        }

        sequenceRow.SetActive(true);
    }

    private void OnReturnedToPool(GameObject sequenceRow)
    {
        sequenceRow.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject sequenceRow)
    {
        Destroy(sequenceRow);
    }
}
