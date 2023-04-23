using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, IInteractable, IInventoryItem
{
    [SerializeField] private string itemName;

    [SerializeField] private string itemDescription;

    [SerializeField] private Sprite itemIcon;

    private PlayerInteraction _interaction;

    private void Start()
    {
        _interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
    }

    public void MouseOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline");

        InteractionUIController.ShowInteractionUi("Pick Up " + itemName);
    }

    public void MouseExit()
    {
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public void Interact()
    {
        if (PlayerInventory.Instance.AddItem(this))
            gameObject.SetActive(false);
    }

    public IInventoryItem GetItem()
    {
        return this;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public string GetItemDescription()
    {
        return itemDescription;
    }

    public void OnAddToInventory()
    {
        _interaction.PlayPickupSound();
    }

    public void OnRemoveFromInventory()
    {
        PlayerInteraction interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        transform.position = interaction.HeldObjectTarget();
        gameObject.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Use()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, 10f))
        {
            Board hitBoard = hit.collider.GetComponent<Board>();

            if (hitBoard != null)
            {
                hitBoard.BreakBoard();
                return;
            }
        }

        DialogueController.InvokeShowDialogueEvent("I don't think this item can be used here...", 5f);
    }

    public Sprite GetItemSprite()
    {
        return itemIcon;
    }
}
