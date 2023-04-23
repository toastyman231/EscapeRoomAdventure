using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable, IInventoryItem
{
    [SerializeField] private string keyName;
    [SerializeField] private string keyDescription;
    [SerializeField] private Sprite icon;

    [SerializeField] private List<GameObject> lockedObjects;

    private PlayerInteraction _interaction;

    private void Start()
    {
        _interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
    }

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
        _interaction.PlayPickupSound();
        return;
    }

    public void OnRemoveFromInventory()
    {
        transform.position = _interaction.HeldObjectTarget();
        GetComponent<Rigidbody>().isKinematic = false;
        gameObject.SetActive(true);
    }

    public void Use()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit, 10f))
        {
            IUnlockable lockedObject = hit.transform.GetComponent<IUnlockable>();

            if (lockedObject != null && lockedObjects.Contains(hit.transform.gameObject))
            {
                lockedObject.Unlock();
                return;
            }
        }

        DialogueController.InvokeShowDialogueEvent("I don't think this item can be used here...", 5f);
    }

    public Sprite GetItemSprite()
    {
        return icon;
    }
}
