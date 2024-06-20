using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] ItemData debugIntemInteractable;
    [SerializeField] UIInventoryController uiInventoryController;
    public UIInventoryController UIInventoryController => uiInventoryController;

    #region Variables to be reset when player starts a new game

    private List<ItemData> inventory = new List<ItemData>();

    #endregion

    private void Start()
    {
        Reset();
        // TODO: It's ok by default?
        AddItem(debugIntemInteractable);
    }

    public void Reset()
    {
        inventory.Clear();
    }
    
    public void Hide(bool hide = true)
    {
        uiInventoryController.Hide(hide);
    }

    public void AddItem(ItemData item)
    {
        Debug.Log("Added item: " + item.name);
        inventory.Add(item);
        uiInventoryController.OnRefreshInventory();
    }

    public void RemoveItem(ItemData item)
    {
        Debug.Log("Removed item: " + item.name);
        inventory.Remove(item);
        uiInventoryController.OnRefreshInventory();
    }
    
    public List<ItemData> GetInventory()
    {
        return inventory;
    }
    
    public ItemData GetItem(int i)
    {
        return inventory[i];
    }
}