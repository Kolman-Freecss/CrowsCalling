// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data.Nodes;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Flow;
using _999__Working_Space.Tolosa5.RequestSystem;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Components
{
    public class NPCDialogueController : MonoBehaviour
    {

        [SerializeField] private int Index = 0; // Index of the NPC in the tracker
        public int GetIndex => Index; // From 0 to X
        
        [SerializeField]
        private int narrativeThreads = 1; // Number of narrative threads that the NPC has. By default, 1.
        
        [SerializeField]
        private DialogueNode m_FirstNode; // No request already completed.
        
        [SerializeField]
        private DialogueNode m_SecondNode; // Request already completed. Then another line of dialogue will be thrown.
        
        [SerializeField]
        private DialogueNode m_ThirdNode; // Request already completed. Then another line of dialogue will be thrown.
        
        private DialogueNode m_CurrentNode = null; // By default, the first node is the current node

        private void Awake()
        {
            DialogueFlowManager.Instance.AddNpcToTracker(Index);
            DialogueFlowManager.Instance.AddNpcToChoiceTracker(Index);
            RequestFlowManager.Instance.AddNpcToTrackerRequests(Index);
        }

        public void Next()
        {
            if (m_CurrentNode == null)
            {
                StartDialogue();
            }
            else
            {
                // TODO: Perform criteria m_PreviousChoiceSelected with the time you late with the request completed.
                // By default m_PreviousChoiceSelected will be NONE (because the first thread just have a 1 possible node).
                bool haveChoices = m_CurrentNode.Choices != null && m_CurrentNode.Choices.Length > 0;
                if (haveChoices)
                {
                    DialogueFlowManager.Instance.OnNextDialogueNode(m_CurrentNode);
                }
                m_CurrentNode = m_CurrentNode.NextNode(DialogueFlowManager.Instance.GetNPCChoice(Index));
                if (m_CurrentNode == null)
                    EndDialogue();
                else
                {
                    DialogueFlowManager.Instance.OnNextDialogueNode(m_CurrentNode);
                }
            }
        }

        private void StartDialogue()
        {
            DialogueNode firstNode = GetNextNode();
            if (firstNode == null)
            {
                Debug.LogError("No dialogue to start");
                return;
            }

            m_CurrentNode = firstNode;
            DialogueFlowManager.Instance.OnStartDialogue(m_CurrentNode, this); // Send the first node
        }
        
        private DialogueNode GetNextNode()
        {
            DialogueNode nodeValue = null;
            bool firstNodeThrown = DialogueFlowManager.Instance.IsThreadThrown(Index, 0);
            bool secondNodeThrown = DialogueFlowManager.Instance.IsThreadThrown(Index, 1);
            bool thirdNodeThrown = DialogueFlowManager.Instance.IsThreadThrown(Index, 2);
            if (!firstNodeThrown || narrativeThreads == 1)
            {
                firstNodeThrown = true;
                DialogueFlowManager.Instance.UpdateNPCThread(Index, 0, true);
                nodeValue = m_FirstNode;
            }
            else if (!secondNodeThrown || narrativeThreads == 2)
            {
                secondNodeThrown = true;
                DialogueFlowManager.Instance.UpdateNPCThread(Index, 1, true);
                nodeValue = m_SecondNode;
            }
            else if (!thirdNodeThrown || narrativeThreads == 3)
            {
                thirdNodeThrown = true;
                DialogueFlowManager.Instance.UpdateNPCThread(Index, 2, true);
                nodeValue = m_ThirdNode;
            }

            if (nodeValue != null)
            {
                nodeValue = nodeValue.GetFirstNode(DialogueFlowManager.Instance.GetNPCChoice(Index));
            }
            return nodeValue;
        }
        
        private void EndDialogue()
        {
            DialogueFlowManager.Instance.OnEndDialogue(m_CurrentNode); // Send the last node
            m_CurrentNode = null;
        }
    }
}