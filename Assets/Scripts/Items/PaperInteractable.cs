using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaperInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName;
    [SerializeField] private GameObject paperCanvas;
    [SerializeField] private PaperText paperText;
    [SerializeField] private AudioClip paperSound;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void MouseOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline");

        InteractionUIController.ShowInteractionUi("Read " + itemName);
    }

    public void MouseExit()
    {
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public void Interact()
    {
        _audioSource.clip = paperSound;
        _audioSource.Play();
        InteractionUIController.HideInteractionUi();
        paperCanvas.GetComponentInChildren<TextMeshProUGUI>().text = paperText.text.Replace("\\\\n", "\n");
        paperCanvas.SetActive(true);

        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MouseLook>().SetCanLook(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetCanMove(false);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetInventoryAvailable(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>().CanPause = false;
    }

    public void HidePaper()
    {
        paperCanvas.SetActive(false);

        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<MouseLook>().SetCanLook(true);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetCanMove(true);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>().SetInventoryAvailable(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PauseController>().CanPause = true;
    }
}
