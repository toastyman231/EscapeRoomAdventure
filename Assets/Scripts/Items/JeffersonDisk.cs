using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JeffersonDisk : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] outlineObjects;

    [SerializeField] private GameObject[] rotatingCylinders;

    [SerializeField] private AudioClip rotateSound;

    [SerializeField] private Transform cameraPosition;

    [SerializeField] private Canvas controlCanvas;

    [SerializeField] private Slider overallSlider;

    [SerializeField] private float transitionTime;

    [SerializeField] private float rotateTime;

    private bool _puzzleActive = false;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnOverallRotated()
    {
        foreach (GameObject cylinder in rotatingCylinders)
        {
            cylinder.transform.localEulerAngles = new Vector3(0, overallSlider.value, 0);
        }
    }

    public void MouseOver()
    {
        foreach (GameObject obj in outlineObjects)
        {
            obj.layer = LayerMask.NameToLayer("Outline");
        }

        InteractionUIController.ShowInteractionUi("Use Code Disk");
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
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteraction>().SetInteraction(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetInventoryAvailable(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MouseLook>().SetCanLook(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetCanMove(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>().CanPause = false;

        LeanTween.move(Camera.main.gameObject, cameraPosition.position, transitionTime).setOnComplete(OnPuzzleEnabled);
        LeanTween.rotate(Camera.main.gameObject, cameraPosition.rotation.eulerAngles, transitionTime);
    }

    public void RotateDisk(int index)
    {
        if (index > rotatingCylinders.Length || !_puzzleActive) return;

        DragRotatingCylinder cylinder = rotatingCylinders[Mathf.Abs(index) - 1].GetComponent<DragRotatingCylinder>();

        if (!cylinder.CanRotate) return;

        cylinder.CanRotate = false;

        _audioSource.clip = rotateSound;
        _audioSource.Play();

        GameObject cylinderObject = cylinder.transform.GetChild(0).gameObject;
        float newY = GetNewRotation(cylinderObject.transform.localEulerAngles.y, index > 0);
        Debug.Log("Rotating to " + newY);
        LeanTween.rotateLocal(cylinder.transform.GetChild(0).gameObject, new Vector3(0, newY, 0), rotateTime).setOnComplete(
            () =>
            {
                cylinder.CanRotate = true;
            });
    }

    public void ExitPuzzle()
    {
        _puzzleActive = false;
        controlCanvas.enabled = false;

        LeanTween.moveLocal(Camera.main.gameObject, new Vector3(0, 0.5f, 0), transitionTime).setOnComplete(
            () =>
            {
                GetComponent<BoxCollider>().enabled = true;
                Crosshair.InvokeToggleCrossHairEvent();
                gameObject.layer = LayerMask.NameToLayer("Interact");
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteraction>().SetInteraction(true);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetInventoryAvailable(true);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MouseLook>().SetCanLook(true);
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetCanMove(true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>().CanPause = true;
            });
    }

    private void OnPuzzleEnabled()
    {
        _puzzleActive = true;
        GetComponent<BoxCollider>().enabled = false;
        controlCanvas.enabled = true;
        Crosshair.InvokeToggleCrossHairEvent();
    }

    private float GetNewRotation(float current, bool down)
    {
        float newRotation = (down) ? current - 13.8f : current + 13.8f;

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
}
