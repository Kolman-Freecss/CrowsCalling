// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Model;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data.Nodes
{
    /// <summary>
    /// This class will be stored by a simple Dialogue Node that will have a list of choices that the player can make 
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptable Objects/Narration/Choice Option")]
    public class ChoiceDialogueOption : ScriptableObject
    {
        public readonly int Index = Guid.NewGuid().GetHashCode();

        [SerializeField]
        private ChoiceOptionType m_ChoiceOptionType;
        
        [SerializeField]
        private NarrationLine m_DialogueLine;
        
        public ChoiceOptionType ChoiceOptionType => m_ChoiceOptionType;
        public NarrationLine DialogueLine => m_DialogueLine;
    }
}