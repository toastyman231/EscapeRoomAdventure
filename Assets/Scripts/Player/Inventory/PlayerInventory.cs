using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [SerializeField] private int maxSize;

    private List<IInventoryItem> _inventory;

    private IInventoryItem _activeItem;

    private PlayerInteraction _interaction;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }

        _inventory = new List<IInventoryItem>(maxSize);
        _interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (_activeItem != null && Input.GetButtonDown("Interact"))
        {
            UseActiveItem();
            _activeItem = null;
        }
    }

    public bool AddItem(IInventoryItem item)
    {
        if (_inventory.Count < maxSize)
        {
            _inventory.Add(item);
            item.OnAddToInventory();
            return true;
        }

        return false;
    }

    public void RemoveItem(IInventoryItem item)
    {
        if (_inventory.Count == 0) return;

        _inventory.Remove(item);
        item.OnRemoveFromInventory();
    }

    public IInventoryItem GetItem(int index)
    {
        return _inventory[index].GetItem();
    }

    public int IndexOf(IInventoryItem item)
    {
        return _inventory.IndexOf(item);
    }

    public int CountOf(IInventoryItem item)
    {
        return _inventory.FindAll((i) => { return i.GetType() == item.GetType(); }).Count;
    }

    public bool Empty()
    {
        return _inventory.Count == 0;
    }

    public int Count()
    {
        return _inventory.Count;
    }

    public void PrepareToUseItem(IInventoryItem item)
    {
        _activeItem = item;
        _interaction.SetInteraction(false);
        InteractionUIController.ShowInteractionUi("Use Item");
        _interaction.SetUsingItem(true);
    }

    public IInventoryItem GetActiveItem()
    {
        return _activeItem;
    }

    private void UseActiveItem()
    {
        if (_activeItem != null) _activeItem.Use();
        _interaction.SetInteraction(true);
        _interaction.SetUsingItem(false);
        InteractionUIController.HideInteractionUi();
    }
}
