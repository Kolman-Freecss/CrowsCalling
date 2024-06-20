using System;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.Utils;
using _999__Working_Space.Tolosa5.Inventory.Model;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInventoryController : MonoBehaviour
{
    [SerializeField] private UIWidgetInventory itemPrefab;
    [SerializeField] private RectTransform itemsBoxTransform;
    [SerializeField] private GameObject subInventory;
    private Image backgroundInventoryWrapper;

    [SerializeField] private float interactRange = 3f;

    private List<UIWidgetInventory> itemsPrefabs = new List<UIWidgetInventory>();

    UIWidgetInventory currentlyPressedWidget = null;

    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private InputActionReference openInventory;
    [SerializeField] private InputActionReference clickAction;

    [SerializeField]
    private List<SerializableDictionaryEntry<ObjectType, List<SerializableDictionaryEntry<bool, Sprite>>>>
        itemHiddenSprites;

    #region Init Data

    private void Awake()
    {
        backgroundInventoryWrapper = GetComponentsInChildren<Image>()[0];
    }

    private void OnEnable()
    {
        openInventory.action.Enable();
        clickAction.action.Enable();
    }

    private void Start()
    {
        // Clean placeHolders from m_ChoicesBoxTransform
        for (int i = 0; i < itemsBoxTransform.childCount; i++)
        {
            Destroy(itemsBoxTransform.GetChild(i).gameObject);
        }

        Hide();
        OnRefreshInventory();
    }

    private void Update()
    {
        InputManagement();
    }

    private void InputManagement()
    {
        if (openInventory.action.WasPerformedThisFrame())
        {
            Show();
        }

        if (clickAction.action.WasPerformedThisFrame())
        {
            OnClickedMouse();
        }
    }

    #endregion

    #region Manage Visibility

    private void ShowSubInventory()
    {
        subInventory.SetActive(true);
        subInventory.transform.position = Input.mousePosition;
        // currentlyPressedWidget.IsInteractable = false;
    }

    public void HideSubInventory()
    {
        subInventory.SetActive(false);
        currentlyPressedWidget.IsInteractable = true;
    }

    private void Show()
    {
        ShowMouseCursor(!itemsBoxTransform.gameObject.activeSelf);
        backgroundInventoryWrapper.gameObject.SetActive(!itemsBoxTransform.gameObject.activeSelf);
        itemsBoxTransform.gameObject.SetActive(!itemsBoxTransform.gameObject.activeSelf);
        if (!itemsBoxTransform.gameObject.activeSelf)
        {
            subInventory.SetActive(false);
        }
    }

    private void ShowMouseCursor(in bool show = true)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void Hide(in bool hide = true)
    {
        backgroundInventoryWrapper.gameObject.SetActive(!hide);
        itemsBoxTransform.gameObject.SetActive(!hide);
        subInventory.SetActive(!hide);
        ShowMouseCursor(!hide);
    }

    #endregion

    #region Flow

    public void OnRefreshInventory()
    {
        CleanCurrentChoices();

        List<ItemData> inventoryList = playerInventory.GetInventory();

        for (int i = 0; i < inventoryList.Count; i++)
        {
            UIWidgetInventory inventoryItem = Instantiate(itemPrefab, itemsBoxTransform);

            inventoryItem.gameObject.GetComponent<Image>().sprite = inventoryList[i].requestedSprite;
            inventoryItem.Index = inventoryList[i].Index;
            inventoryItem.InventoryController = this;
            inventoryItem.IsInteractable = true;

            itemsPrefabs.Add(inventoryItem);
        }

        // Show the other items that are not in the inventory but are in the hidden sprites list
        foreach (var item in itemHiddenSprites)
        {
            if (playerInventory.GetInventory().FindIndex(inventoryItem => inventoryItem.itemType == item.Key) == -1)
            {
                UIWidgetInventory inventoryItem = Instantiate(itemPrefab, itemsBoxTransform);
                inventoryItem.gameObject.GetComponent<Image>().sprite = item.Value[0].Value;
                inventoryItem.Index = -1;
                inventoryItem.InventoryController = this;
                inventoryItem.IsInteractable = false;

                itemsPrefabs.Add(inventoryItem);
            }
        }
    }

    public void OnDialogueChoice(UIWidgetInventory currentlyPressedWidget, in int choiceIndex)
    {
        this.currentlyPressedWidget = currentlyPressedWidget;
        ShowSubInventory();
    }

    #endregion

    #region Gift

    //Called in inspector
    public void OnClickedGive()
    {
        GiftInteract();
    }

    private void GiftInteract()
    {
        NPCInteractable npc = GetNPC();
        if (npc == null)
        {
            Debug.Log("No interactable npc found.");
            return;
        }

        ItemData itemData = playerInventory.GetInventory().Find(item => item.Index == currentlyPressedWidget.Index);

        RequestHandler requestHandler = npc.GetComponent<RequestHandler>();
        if (!requestHandler.GetCanBeGifted())
        {
            Debug.Log("This NPC cannot be gifted.");
            return;
        }

        requestHandler.CompareRequest(itemData, playerInventory, npc.Index);
    }

    public NPCInteractable GetNPC()
    {
        List<NPCInteractable> interactables = new List<NPCInteractable>();
        Collider[] colls = Physics.OverlapSphere(playerInventory.gameObject.transform.position +
                                                 playerInventory.gameObject.transform.forward, interactRange);

        if (colls.Length == 0)
        {
            Debug.Log("colls array empty");
            return null;
        }

        foreach (var coll in colls)
        {
            if (coll.TryGetComponent(out NPCInteractable npcInteractable))
            {
                interactables.Add(npcInteractable);
            }
        }

        NPCInteractable closestInteractable = null;
        foreach (var interactable in interactables)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

    #endregion

    private void OnClickedMouse()
    {
        //If mouse is clicked outside of the inventory, close the inventory
        if (!subInventory.activeSelf)
            return;

        RectTransform currentRect = subInventory.GetComponent<RectTransform>();
        if (!RectTransformUtility.RectangleContainsScreenPoint(currentRect, Input.mousePosition))
        {
            HideSubInventory();
            currentlyPressedWidget = null;
        }
    }

    private void CleanCurrentChoices()
    {
        if (itemsPrefabs.Count > 0)
        {
            itemsPrefabs.ForEach(widget => Destroy(widget.gameObject));
            itemsPrefabs.Clear();
        }
    }

    private void OnDisable()
    {
        openInventory.action.Disable();
        clickAction.action.Disable();
    }

    #region Getters & Setters

    public bool IsInventoryActive()
    {
        return itemsBoxTransform.gameObject.activeSelf || subInventory.activeSelf;
    }

    #endregion
}