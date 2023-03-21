using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public IInventoryItem SlotItem { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SlotItem == null) return;

        Transform parent = transform.parent;
        foreach (Transform inventorySlot in parent)
        {
            inventorySlot.GetComponentInChildren<Image>().color = Color.white;
        }
        GetComponentInChildren<Image>().color = Color.blue;
        InventoryUIController.InvokeUpdateInfoPanel(SlotItem);
    }
}
