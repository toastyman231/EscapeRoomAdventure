using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIController : MonoBehaviour
{
    private static event EventHandler ShowInventoryUIEvent;

    private static event EventHandler HideInventoryUIEvent;

    private static event EventHandler<InfoEventArgs> UpdateInfoPanelEvent;

    public static bool ShowingInventory;

    [SerializeField] private Canvas inventoryCanvas;

    [SerializeField] private GameObject infoPanel;

    [SerializeField] private GameObject inventoryParent;

    private IInventoryItem _selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        ShowInventoryUIEvent += ShowInventoryUI;
        HideInventoryUIEvent += HideInventoryUI;
        UpdateInfoPanelEvent += UpdateInfoPanel;
    }

    private void OnDestroy()
    {
        ShowInventoryUIEvent -= ShowInventoryUI;
        HideInventoryUIEvent -= HideInventoryUI;
        UpdateInfoPanelEvent -= UpdateInfoPanel;
    }

    public static void InvokeShowInventory()
    {
        ShowInventoryUIEvent?.Invoke(null, EventArgs.Empty);
    }

    public static void InvokeHideInventory()
    {
        HideInventoryUIEvent?.Invoke(null, EventArgs.Empty);
    }

    public static void InvokeUpdateInfoPanel(IInventoryItem selectedItem)
    {
        UpdateInfoPanelEvent?.Invoke(null, new InfoEventArgs { SelectedItem = selectedItem });
    }

    public void DropItem()
    {
        PlayerInventory.Instance.RemoveItem(_selectedItem);
    }

    private void UpdateInfoPanel(object sender, InfoEventArgs args)
    {
        _selectedItem = args.SelectedItem;
        SetupInfoPanel();
    }

    private void ShowInventoryUI(object sender, EventArgs args)
    {
        inventoryCanvas.enabled = true;
        ShowingInventory = true;
        PopulateInventoryList();
        SetupInfoPanel();
    }

    private void HideInventoryUI(object sender, EventArgs args)
    {
        inventoryCanvas.enabled = false;
        _selectedItem = null;
        ShowingInventory = false;
    }

    private void SetupInfoPanel()
    {
        if (_selectedItem == null)
        {
            infoPanel.SetActive(false);
            return;
        }

        infoPanel.SetActive(true);
        infoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _selectedItem.GetItemName();
        infoPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _selectedItem.GetItemDescription();
    }

    private void PopulateInventoryList()
    {
        if (PlayerInventory.Instance.Empty()) return;

        int index = 0;
        foreach (Transform inventorySlot in inventoryParent.transform)
        {
            if (index >= PlayerInventory.Instance.Count())
            {
                index++;
                continue;
            }

            Image inventoryImage = inventorySlot.GetComponentInChildren<Image>();
            IInventoryItem item = PlayerInventory.Instance.GetItem(index);
            Sprite itemImage = item.GetItemSprite();

            if (itemImage != null)
            {
                inventoryImage.sprite = itemImage;
            }

            InventorySlot slot = inventorySlot.GetComponent<InventorySlot>();
            slot.SlotItem = item;

            index++;
        }
    }
}

public class InfoEventArgs : EventArgs
{
    public IInventoryItem SelectedItem;
}
