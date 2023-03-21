using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    public IInventoryItem GetItem();
    public string GetItemName();
    public string GetItemDescription();
    public void OnAddToInventory();
    public void OnRemoveFromInventory();
    public void Use();
    public Sprite GetItemSprite();
}
