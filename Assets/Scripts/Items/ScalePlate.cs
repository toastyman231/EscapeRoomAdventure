using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlate : MonoBehaviour, IInteractable
{
    public float TotalWeight { get; private set; }

    [SerializeField] private Scale myScale;

    [SerializeField] private float moveDuration;

    [SerializeField] private Transform[] weightLocations;

    private List<GameObject> _weights;

    private PlayerInteraction _playerInteraction;

    private void Start()
    {
        _weights = new List<GameObject>();
        _playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
    }

    public void MovePlate(float amount)
    {
        float moveTo = Mathf.Clamp(gameObject.transform.localPosition.y + (amount / myScale.WeightScale), myScale.YMin, myScale.YMax);
        Debug.Log("Moving to: " + moveTo);
        LeanTween.moveLocalY(gameObject, moveTo, moveDuration).setDelay(2.5f);
    }

    public void MouseOver()
    {
        foreach (GameObject obj in _weights)
        {
            obj.layer = LayerMask.NameToLayer("Outline");
        }

        gameObject.layer = LayerMask.NameToLayer("Outline");

        InteractionUIController.ShowInteractionUi("Add Weight");
    }

    public void MouseExit()
    {
        foreach (GameObject obj in _weights)
        {
            obj.layer = LayerMask.NameToLayer("Default");
        }

        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public void Interact()
    {
        if (!_playerInteraction.UsingItem())
        {
            DialogueController.InvokeShowDialogueEvent("I need a <color=\"red\">weight<color=\"white\"> for this scale...", 5f);
            return;
        }
    }

    public bool AddWeight(GameObject weight)
    {
        if (_weights.Count < weightLocations.Length)
        {
            
            _weights.Add(weight);
            return true;
        }

        return false;
    }
}
