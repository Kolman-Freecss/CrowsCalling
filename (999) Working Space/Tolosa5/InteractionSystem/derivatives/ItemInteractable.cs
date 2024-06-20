using System;
using System.Collections;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Components;
using _999__Working_Space.Tolosa5.InteractionSystem;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(NPCDialogueController))]
public class ItemInteractable : BaseInteractable, IInteractable
{
    public ItemData itemData;
    
    private NPCDialogueController dialogueController;
    
    [SerializeField] private string interactText; //Press E to interact thing

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        dialogueController = GetComponent<NPCDialogueController>();
    }

    public void Interact(Transform interactorTransform)
    {
       if (dialogueController == null)
       {
           Debug.LogError("No dialogue controller found on " + gameObject.name);
           return;
       }
       Debug.Log("Conversation with " + gameObject.name + " started.");
       dialogueController.Next();
       GetPickedUp(interactorTransform);
    }

    private void GetPickedUp(Transform interactorTransform)
    {
        Debug.Log("Item picked up");
        interactorTransform.gameObject.GetComponent<PlayerInventory>().AddItem(this.itemData);
        Destroy(gameObject);
    }
    
    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}