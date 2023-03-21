using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public IInventoryItem SlotItem { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked!");
        if (SlotItem == null) return;

        Debug.Log("Showing item: " + SlotItem.GetItemName());
        InventoryUIController.InvokeUpdateInfoPanel(SlotItem);
    }
}
