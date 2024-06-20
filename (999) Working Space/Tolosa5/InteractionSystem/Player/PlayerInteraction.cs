using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInventory playerInventory;
    
    [SerializeField] private float interactRange = 4f;
    
    [SerializeField] InputActionReference interactAction;

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (interactAction.action.WasPerformedThisFrame()) 
        {
            Interact();
        }
    }

    private void Interact()
    {
        IInteractable interactable = GetInteractableObject();
        if (interactable == null)
        {
            Debug.Log("No interactable object found.");
            return;
        }
        
        interactable.Interact(transform);
    }

    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactables = new List<IInteractable>();
        Collider[] colls = Physics.OverlapSphere(transform.position + transform.forward, interactRange);
        foreach (var coll in colls)
        {
            if (coll.TryGetComponent(out IInteractable interactable))
            {
                interactables.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (var interactable in interactables)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position ,interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }
    
    private void OnDisable()
    {
        interactAction.action.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward, interactRange);
    }
}
