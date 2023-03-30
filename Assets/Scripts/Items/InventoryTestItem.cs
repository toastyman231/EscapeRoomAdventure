using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTestItem : MonoBehaviour, IInventoryItem, IInteractable
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private Sprite image;

    public IInventoryItem GetItem()
    {
        return this;
    }

    public string GetItemDescription()
    {
        return description;
    }

    public string GetItemName()
    {
        return name;
    }

    public Sprite GetItemSprite()
    {
        return image;
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

    public void MouseOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline");
        InteractionUIController.ShowInteractionUi("Take " + name);
    }

    public void OnAddToInventory()
    {
        Debug.Log("Added test item to inventory!");
        Debug.Log("Number of test items in inventory: " + PlayerInventory.Instance.CountOf(this));
    }

    public void OnRemoveFromInventory()
    {
        PlayerInteraction interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        transform.position = interaction.HeldObjectTarget();
        gameObject.SetActive(true);
        Debug.Log("Removed test item from inventory!");
        Debug.Log("Number of test items in inventory: " + PlayerInventory.Instance.CountOf(this));
    }

    public void Use()
    {
        Debug.Log("Using test item!");
    }
}
