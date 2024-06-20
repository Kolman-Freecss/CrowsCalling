// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Model;
using _999__Working_Space.Kolman_Freecss.Modules.Utils;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data.Nodes
{
    [CreateAssetMenu(fileName = "DialogueNode", menuName = "Dialogue/DialogueNode")]
    public class DialogueNode : ScriptableObject
    {
        // If there are multiple possible first nodes, then the container node will be an empty node 
        [SerializeField]
        private List<SerializableDictionaryEntry<ChoiceOptionType, DialogueNode>> m_PossibleFirstNodes;
        
        [SerializeField]
        private NarrationLine m_DialogueLine;
        
        [SerializeField]
        private List<SerializableDictionaryEntry<ChoiceOptionType, DialogueNode>> m_NextNode;
        
        [SerializeField]
        private ChoiceDialogueOption[] m_Choices;
        
        public ChoiceDialogueOption[] Choices => m_Choices;
        public NarrationLine DialogueLine => m_DialogueLine;
        
        // If there are multiple possible first nodes will find the first node based on the choice, else will return the only possible first node (itself)
        public DialogueNode GetFirstNode(ChoiceOptionType choice) 
        {
            DialogueNode firstNode = GetPossibleFirstNode(choice);
            if (firstNode != null)
            {
                return firstNode;
            }
            return this;
        }
        
        public DialogueNode GetPossibleFirstNode(ChoiceOptionType choice) 
        {
            if (m_PossibleFirstNodes == null || m_PossibleFirstNodes.Count == 0)
            {
                // Debug.LogWarning("No possible first nodes found.");
                return null;
            }
            if (m_PossibleFirstNodes.Count == 1)
            {
                // Debug.Log("Only one possible first node found.");
                return m_PossibleFirstNodes[0].Value;
            }
            foreach (var entry in m_PossibleFirstNodes)
            {
                if (entry.Key == choice)
                {
                    // Debug.Log("Possible first node found.");
                    return entry.Value;
                }
            }

            return m_PossibleFirstNodes[0].Value; // If no choice is found, return the first one as default
        }
        
        public DialogueNode NextNode(ChoiceOptionType choice) 
        {
            if (m_NextNode == null || m_NextNode.Count == 0)
            {
                return null;
            }
            if (m_NextNode.Count == 1)
            {
                return m_NextNode[0].Value;
            }
            foreach (var entry in m_NextNode)
            {
                if (entry.Key == choice)
                {
                    return entry.Value;
                }
            }

            return null;
        }
        
    }
}