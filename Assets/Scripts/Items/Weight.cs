using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour, IInteractable, IInventoryItem
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite icon;

    [SerializeField] private List<GameObject> outlineObjects;

    [SerializeField] private float weight;

    private bool _onScale = false;

    private ScalePlate _currentScalePlate;

    private PlayerInteraction _interaction;

    private void Start()
    {
        _interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
    }

    public IInventoryItem GetItem()
    {
        return this;
    }

    public string GetItemDescription()
    {
        return itemDescription;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public Sprite GetItemSprite()
    {
        return icon;
    }

    public float GetWeight()
    {
        return weight;
    }

    public void Interact()
    {
        if (PlayerInventory.Instance.AddItem(this))
            gameObject.SetActive(false);
    }

    public void MouseExit()
    {
        foreach (GameObject outline in outlineObjects)
        {
            outline.layer = LayerMask.NameToLayer("Default");
        }
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public void MouseOver()
    {
        foreach (GameObject outline in outlineObjects)
        {
            outline.layer = LayerMask.NameToLayer("Outline");
        }
        gameObject.layer = LayerMask.NameToLayer("Outline");

        InteractionUIController.ShowInteractionUi("Pick Up " + itemName);
    }

    public void OnAddToInventory()
    {
        _interaction.PlayPickupSound();
        if (_onScale)
        {
            _onScale = false;
            transform.parent = null;
            _currentScalePlate.RemoveWeight(gameObject);
            _currentScalePlate = null;
        }
    }

    public void OnRemoveFromInventory()
    {
        gameObject.SetActive(true);
    }

    public void Use()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, 10f))
        {
            ScalePlate hitPlate = hit.collider.gameObject.GetComponent<ScalePlate>();

            if (hitPlate != null)
            {
                if (!hitPlate.AddWeight(gameObject))
                {
                    DialogueController.InvokeShowDialogueEvent("This side of the scale is full...", 3f);
                    return;
                }

                PlayerInventory.Instance.RemoveItem(this);
                _onScale = true;
                _currentScalePlate = hitPlate;
                return;
            }
        }

        DialogueController.InvokeShowDialogueEvent("I don't think this item can be used here...", 5f);
    }
}
