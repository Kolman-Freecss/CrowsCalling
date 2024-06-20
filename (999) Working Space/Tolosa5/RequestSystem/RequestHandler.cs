using _999__Working_Space.Kolman_Freecss.Modules.AudioModule;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Components;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Flow;
using _999__Working_Space.Kolman_Freecss.Modules.ReputationModule;
using _999__Working_Space.Tolosa5.Inventory.Model;
using _999__Working_Space.Tolosa5.RequestSystem;
using UnityEngine;
using UnityEngine.UI;

public class RequestHandler : MonoBehaviour
{
    [SerializeField] private ItemData requestItemData, giftItemData;

    [SerializeField] private GameObject requestedVisualGO; // Requested item (Canvas)
    private Image requestedVisual; // Requested item image

    NPCDialogueController dialogueController;
    NPCInteractable npcInteractable;

    [HideInInspector] public bool isRequestCompleted; // Status of the request
    private bool canBeGifted = false;
    
    #region Variables stored too in RequestFlowManager
    
    private ObjectType itemType; // Requested item type
    private Sprite requestedSprite; // Requested item sprite
    
    #endregion

    private void Awake()
    {
        dialogueController = GetComponent<NPCDialogueController>();
        npcInteractable = GetComponent<NPCInteractable>();
    }

    private void Start()
    {
        requestedVisual = requestedVisualGO.GetComponent<Image>();
        // This means that the request is thrown (if its false) and the request is completed (if its true)
        isRequestCompleted = DialogueFlowManager.Instance.IsThreadThrown(dialogueController.GetIndex, 1);
        
        RequestPayload requestPayload = RequestFlowManager.Instance.GetRequestPayload(dialogueController.GetIndex);
        itemType = requestPayload.ItemType;
        requestedSprite = requestPayload.RequestedSprite;
        canBeGifted = requestPayload.IsCanBeGifted;
        if (requestPayload.IsRequestMade && !isRequestCompleted)
        {
            ShowRequest();
        }
        
        bool requestThrownButNotCompleted = DialogueFlowManager.Instance.IsThreadThrown(dialogueController.GetIndex, 0) && !isRequestCompleted;
        requestedVisualGO.SetActive(requestThrownButNotCompleted);
    }

    public void MakeRequest()
    {
        itemType = requestItemData.itemType;
        requestedSprite = requestItemData.requestedSprite;
        SetCanBeGifted(true);

        RequestPayload requestPayload = new RequestPayload(itemType, requestedSprite, true, GetCanBeGifted(), false);
        RequestFlowManager.Instance.UpdateRequestPayload(dialogueController.GetIndex, requestPayload);
        
        ShowRequest();
    }
    
    //Called when giving the item to the NPC
    public void CompareRequest(ItemData itemData, PlayerInventory playerInventory, in int NpcIndex)
    {
        if (itemData.itemType == this.itemType)
        {
            Debug.Log("Request is correct");
            CompleteRequest(itemData, playerInventory, NpcIndex);
        }
        else
        {
            Debug.Log("Request is incorrect");
            float requestFailedReputation = ReputationManager.Instance.m_ReputationAmountForRequestFailed;
            ReputationManager.Instance.AddReputation(requestFailedReputation, NpcIndex);
        }
    }
    
    public void CompleteRequest(ItemData itemData, PlayerInventory playerInventory, in int NpcIndex)
    {
        float requestCompletedReputation = ReputationManager.Instance.m_ReputationAmountForRequestCompleted;
        ReputationManager.Instance.AddReputation(requestCompletedReputation, NpcIndex);
        isRequestCompleted = true;
        
        GameState.Instance.CheckSatisfiedNPCs(); // Manage EndGame
        
        // TODO: Replace item with the hidden item
        // playerInventory.RemoveItem(itemData);
        playerInventory.AddItem(giftItemData);
        playerInventory.Hide();
        HideRequest();
        SetCanBeGifted(false);
        SoundTrackCrowsManager.Instance.StartNPCTrackReferencedByIndex(dialogueController.GetIndex);
        RequestFlowManager.Instance.UpdateRequestPayload(NpcIndex, new RequestPayload(itemType, requestedSprite, true, GetCanBeGifted(), true));
        npcInteractable.Interact(null);
    }
    
    //Called when talking to the NPC
    public void ShowRequest()
    {
        requestedVisual.sprite = requestedSprite;
        requestedVisualGO.SetActive(true);
    }
    
    //Called when talking to the NPC
    public void HideRequest()
    {
        requestedVisualGO.SetActive(false);
    }

    public bool GetCanBeGifted()
    {
        return canBeGifted;
    }
    
    //TODO: Call in dialogue
    public void SetCanBeGifted(bool value)
    {
        canBeGifted = value;
    }
}
