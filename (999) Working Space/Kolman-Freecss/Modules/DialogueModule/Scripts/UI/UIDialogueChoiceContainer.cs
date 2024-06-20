// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data.Nodes;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Flow;
using TMPro;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.UI
{
    public class UIDialogueChoiceContainer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_SpeakerText;
        
        [SerializeField]
        private TextMeshProUGUI m_DialogueText;
        
        [SerializeField]
        private UIWidgetDialogueChoice m_ChoicePrefab;
        
        [SerializeField]
        private RectTransform m_ChoicesBoxTransform;
        
        [SerializeField]
        private UIDialogueController m_DialogueController;
        
        private List<UIWidgetDialogueChoice> m_Choices = new List<UIWidgetDialogueChoice>();
        
        private void Start()
        {
            // Clean placeHolders from m_ChoicesBoxTransform
            for (int i = 0; i < m_ChoicesBoxTransform.childCount; i++)
            {
                Destroy(m_ChoicesBoxTransform.GetChild(i).gameObject);
            }
        }
        
        public void OnDialogueNext(DialogueNode dialogueNode)
        {
            CleanCurrentChoices();
            // Check if the dialogue node has choices
            if (dialogueNode.Choices != null && dialogueNode.Choices.Length > 0)
            {
                // Display the choices
                foreach (ChoiceDialogueOption choice in dialogueNode.Choices)
                {
                    UIWidgetDialogueChoice choiceController = Instantiate(m_ChoicePrefab, m_ChoicesBoxTransform);
                    choiceController.ChoiceText.text = choice.DialogueLine.Text;
                    choiceController.DialogueController = m_DialogueController;
                    choiceController.ReferencedNode = dialogueNode;
                    choiceController.ChoiceIndex = choice.Index;
                    m_Choices.Add(choiceController);
                }
            }
            m_SpeakerText.text = dialogueNode.DialogueLine.Speaker.CharacterName;
            m_DialogueText.text = dialogueNode.DialogueLine.Text;
        }   

        public void OnDialogueChoice(DialogueNode dialogueNode, in int choiceIndex)
        {
            CleanCurrentChoices();
            ChoiceDialogueOption choice = null;
            foreach (ChoiceDialogueOption option in dialogueNode.Choices)
            {
                if (option.Index == choiceIndex)
                {
                    choice = option;
                    break;
                }
            }
            DialogueFlowManager.Instance.OnDialogueChoice(dialogueNode, choice);
        }
        
        public void CleanCurrentChoices()
        {
            m_Choices.ForEach(choice => Destroy(choice.gameObject));
            m_Choices.Clear();
        }
    }
}