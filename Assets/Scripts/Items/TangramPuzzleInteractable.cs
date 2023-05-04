using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TangramPuzzleInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] outlineObjects;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private Vector3 normalTangramsScale;

    [SerializeField] private Vector3 bigTangramsScale;

    [SerializeField] private Image tangramImage;

    [SerializeField] private UnityEvent onCompleted;

    public TangramSolution defaultPositions;

    public int requiredSolves;

    [SerializeField] private float cameraMoveTime;

    private GameObject _tangramsParent;

    private bool _inPuzzle = false;

    private void Update()
    {
        if (_inPuzzle)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                ExitPuzzle(true);
            }
        }
    }

    public void MouseOver()
    {
        foreach (GameObject obj in outlineObjects)
        {
            obj.layer = LayerMask.NameToLayer("Outline");
        }

        InteractionUIController.ShowInteractionUi("Use Blocks");
    }

    public void MouseExit()
    {
        foreach (GameObject obj in outlineObjects)
        {
            obj.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void Interact()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        GetComponent<BoxCollider>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteraction>().SetInteraction(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetInventoryAvailable(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MouseLook>().SetCanLook(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetCanMove(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>().CanPause = false;
        Crosshair.InvokeToggleCrossHairEvent();

        //_originalCamPos = Camera.main.transform;
        LeanTween.move(Camera.main.gameObject, cameraTransform.position, cameraMoveTime).setOnComplete(() =>
        {
            _inPuzzle = true;
        });
        LeanTween.rotate(Camera.main.gameObject, cameraTransform.rotation.eulerAngles, cameraMoveTime);
    }

    public void ResizeTangrams(bool big)
    {
        if (_tangramsParent == null) _tangramsParent = GameObject.FindGameObjectWithTag("TangramsParent");
        if (big)
        {
            _tangramsParent.transform.localScale = bigTangramsScale;
            defaultPositions = Resources.Load<TangramSolution>("DefaultBig");
        }
        else
        {
            _tangramsParent.transform.localScale = normalTangramsScale;
            defaultPositions = Resources.Load<TangramSolution>("Default");
        }
    }

    public void SetNewPuzzleImage(int index)
    {
        Sprite[] images = Resources.LoadAll<Sprite>("Tangram Images/");

        Sprite chosenImage = images[index];

        Resources.UnloadUnusedAssets();

        tangramImage.sprite = chosenImage;
    }

    public void OnAllSolvesCompleted()
    {
        ExitPuzzle(false);

        onCompleted?.Invoke();
    }

    private  void ExitPuzzle(bool reEnable)
    {
        LeanTween.moveLocal(Camera.main.gameObject, new Vector3(0, 0.5f, 0), cameraMoveTime).setOnComplete(
            () =>
            {
                if (reEnable)
                {
                    gameObject.layer = LayerMask.NameToLayer("Interact");
                    GetComponent<BoxCollider>().enabled = true;
                }
                Crosshair.InvokeToggleCrossHairEvent();
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteraction>().SetInteraction(true);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetInventoryAvailable(true);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MouseLook>().SetCanLook(true);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetCanMove(true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>().CanPause = true;
                _inPuzzle = false;
            });
    }
}
