using System;
using System.Collections;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Flow;
using TMPro;
using UnityEngine;

public class PlayerInteractionUI : MonoBehaviour
{
    private PlayerInteraction playerInteraction;
    
    [SerializeField] private GameObject containerGO;
    [SerializeField] private TextMeshProUGUI interactText;

    private void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        Hide();
    }

    private void Update()
    {
        if (DialogueFlowManager.Instance == null)
        {
            Debug.LogError("DialogueFlowManager is null");
            return;
        }
        bool isInDialogue = DialogueFlowManager.Instance.CurrentDialogueState == DialogueState.InDialogue;
        if (playerInteraction.GetInteractableObject() != null && !isInDialogue)
        {
            Show(playerInteraction.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }
    
    private void Show(IInteractable interactable)
    {
        if (interactable is NPCInteractable)
        {
            ((NPCInteractable)interactable).ShowOutline();
        }
        containerGO.SetActive(true);
        interactText.text = interactable.GetInteractText();
    }

    private void Hide()
    {
        containerGO.SetActive(false);
    }
}
