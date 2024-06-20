// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data.Nodes;
using TMPro;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.UI
{
    public class UIDialogueContainer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_SpeakerText;
        
        [SerializeField]
        private TextMeshProUGUI m_DialogueText;
        
        [SerializeField]
        private TextMeshProUGUI m_ContinueText;
        
        public void OnDialogueNext(DialogueNode dialogueNode)
        {
            m_SpeakerText.text = dialogueNode.DialogueLine.Speaker.CharacterName;
            m_DialogueText.text = dialogueNode.DialogueLine.Text;
            m_ContinueText.text = "Press 'E' key to continue.";
        }   
        
    }
}