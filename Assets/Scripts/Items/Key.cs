using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable, IInventoryItem
{
    [SerializeField] private string keyName;
    [SerializeField] private string keyDescription;
    [SerializeField] private Sprite icon;

    public void MouseOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline");
        InteractionUIController.ShowInteractionUi("Pick Up " + keyName);
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
        return keyName;
    }

    public string GetItemDescription()
    {
        return keyDescription;
    }

    public void OnAddToInventory()
    {
        return;
    }

    public void OnRemoveFromInventory()
    {
        return;
    }

    public void Use()
    {
        Debug.Log("Used " + keyName);
    }

    public Sprite GetItemSprite()
    {
        return icon;
    }
}
