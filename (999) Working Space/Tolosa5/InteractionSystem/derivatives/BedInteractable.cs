using System;
using System.Collections;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModalsModule;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Components;
using _999__Working_Space.Tolosa5.InteractionSystem;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BedInteractable : BaseInteractable, IInteractable
{
    [SerializeField] private String question = "Do you want to sleep?"; // The question that will be displayed in the modal
    [SerializeField] private String[] options = {"Sleep", "Cancel"}; // By default, the bed will have two options: Sleep and Cancel
    
    [SerializeField] private string interactText = "Press E to sleep"; //Press E to interact thing
    
    private PlayerModalOptionController playerModalOptionController;

    protected override void Start()
    {
        base.Start();
        playerModalOptionController = FindObjectOfType<PlayerModalOptionController>();
        if (playerModalOptionController == null)
        {
            Debug.LogError("No player modal option controller found in scene.");
        }
    }

    public void Interact(Transform interactorTransform)
    {
        if (playerModalOptionController == null)
        {
            Debug.LogError("No player modal option controller found in scene. Name of object: " + gameObject.name);
            return;
        }
        Debug.Log("Interacting with bed");
        playerModalOptionController.OnOptionSelected += HandleOnOptionSelected;
        playerModalOptionController.Invoke(new List<string>(options), question);
    }

    private void HandleOnOptionSelected(int index)
    {
        Debug.Log("Option selected: " + index);
        if (index == 0)
        {
            GameState.Instance.GoToSleep();
        }
        playerModalOptionController.OnOptionSelected -= HandleOnOptionSelected;
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
