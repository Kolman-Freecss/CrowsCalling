using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWidgetInventory : MonoBehaviour
{
    private UIInventoryController inventoryController;
    public UIInventoryController InventoryController
    {
        get => inventoryController;
        set => inventoryController = value;
    }

    private int index;
    public int Index
    {
        get => index;
        set => index = value;
    }
    
    private bool isInteractable = true;
    public bool IsInteractable
    {
        get => isInteractable;
        set => isInteractable = value;
    }
    
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (isInteractable)
            inventoryController.OnDialogueChoice(this, index);
    }
}
