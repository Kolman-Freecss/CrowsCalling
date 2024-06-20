using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Components;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Flow;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.UI;
using _999__Working_Space.Tolosa5.InteractionSystem;
using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(NPCDialogueController))]
public class NPCInteractable : BaseInteractable, IInteractable
{
    private NPCDialogueController dialogueController;
    private RequestHandler requestHandler;
    
    [SerializeField] private string interactText; //Press E to interact thing

    private Animator anim;
    
    public TownType myTown;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        
        dialogueController = GetComponent<NPCDialogueController>();
        requestHandler = GetComponent<RequestHandler>();
        
        //Add this NPC to the game state list
        GameState.Instance.AddToNPCDictionary(myTown, this);
    }

    public void Interact(Transform interactorTransform)
    {
       if (dialogueController == null)
       {
           Debug.LogError("No dialogue controller found on " + gameObject.name);
           return;
       }
       if (DialogueFlowManager.Instance.GetCurrentContainerType() == DialogueContainerType.Choice)
       {
           Debug.Log("Cannot NEXT dialogue with " + gameObject.name + " because a choice is already active.");
           return;
       }

       if (!requestHandler.isRequestCompleted && DialogueFlowManager.Instance.IsThreadThrown(dialogueController.GetIndex, 0))
       {
           return;
       }
       Debug.Log("Conversation with " + gameObject.name + " started.");
       dialogueController.Next();
       // This means that we're in the last thread of the NPC
       if (DialogueFlowManager.Instance.IsThreadThrown(dialogueController.GetIndex, 2))
       {
           return;
       }
       ManageIfItsRequest();
    }
    
    private void ManageIfItsRequest()
    {
        if (requestHandler == null)
        {
            Debug.LogError("No request handler found on " + gameObject.name);
            return;
        }
        // This means that the request is thrown (if its false) and the request is completed (if its true)
        if (!DialogueFlowManager.Instance.IsThreadThrown(dialogueController.GetIndex, 1))
        {
            requestHandler.MakeRequest();
        }
            
        if (requestHandler.isRequestCompleted)
        {
            anim.SetBool("RequestCompleted", true);
        }
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
