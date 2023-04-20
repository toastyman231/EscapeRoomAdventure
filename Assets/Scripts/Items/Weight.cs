using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour, IInteractable, IInventoryItem
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite icon;

    [SerializeField] private float weight;

    private Rigidbody _rb;

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

    public void Interact()
    {
        if (PlayerInventory.Instance.AddItem(this))
            gameObject.SetActive(false);
    }

    public void MouseExit()
    {
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public void MouseOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline");

        InteractionUIController.ShowInteractionUi("Pick Up " + itemName);
    }

    public void OnAddToInventory()
    {
        return;
    }

    public void OnRemoveFromInventory()
    {
        _rb.isKinematic = true;
        _rb.useGravity = false;
        gameObject.SetActive(true);
    }

    public void Use()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, 10f))
        {
            ScalePlate hitPlate = hit.collider.gameObject.GetComponent<ScalePlate>();

            if (hitPlate != null)
            {
                PlayerInventory.Instance.RemoveItem(this);
                gameObject.transform.position = hitPlate.transform.position;
                return;
            }
        }

        DialogueController.InvokeShowDialogueEvent("I don't think this item can be used here...", 5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
