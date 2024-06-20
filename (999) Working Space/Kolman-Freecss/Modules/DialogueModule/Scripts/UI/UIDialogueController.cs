// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data.Nodes;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.UI
{
    public enum DialogueContainerType
    {
        Dialogue,
        Choice
    }
    
    public class UIDialogueController : MonoBehaviour
    {
        [HideInInspector]
        public DialogueContainerType m_CurrentContainerType;
        
        [SerializeField]
        private UIDialogueContainer m_DialogueContainer;
        
        [SerializeField]
        private UIDialogueChoiceContainer m_ChoiceContainer;

        private void Start()
        {
            m_CurrentContainerType = DialogueContainerType.Dialogue;
            Hide(DialogueContainerType.Dialogue);
            Hide(DialogueContainerType.Choice);
        }
        
        public void Hide(in DialogueContainerType containerType, in bool hide = true)
        {
            switch (containerType)
            {
                case DialogueContainerType.Dialogue:
                    m_DialogueContainer.gameObject.SetActive(!hide);
                    break;
                case DialogueContainerType.Choice:
                    m_ChoiceContainer.gameObject.SetActive(!hide);
                    ShowMouseCursor(!hide);
                    break;
            }
        }
        
        private void ShowMouseCursor(in bool show = true)
        {
            Cursor.visible = show;
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        public void OnDialogueNext(DialogueNode dialogueNode)
        {
            Hide(m_CurrentContainerType);
            SetCurrentContainerType(dialogueNode);
            Hide(m_CurrentContainerType, false);
            switch (m_CurrentContainerType)
            {
                case DialogueContainerType.Dialogue:
                    m_DialogueContainer.OnDialogueNext(dialogueNode);
                    break;
                case DialogueContainerType.Choice:
                    m_ChoiceContainer.OnDialogueNext(dialogueNode);
                    break;
            }
        }   
        
        public void OnDialogueEnd(DialogueNode dialogueNode)
        {
            if (dialogueNode != null)
            {
                SetCurrentContainerType(dialogueNode);
            }
            CleanCurrentChoices();
            Hide(m_CurrentContainerType);
            m_CurrentContainerType = DialogueContainerType.Dialogue;
        }

        private void SetCurrentContainerType(in DialogueNode dialogueNode)
        {
            m_CurrentContainerType = dialogueNode.Choices != null && dialogueNode.Choices.Length > 0
                ? DialogueContainerType.Choice
                : DialogueContainerType.Dialogue;
        }
        
        public DialogueContainerType GetCurrentContainerType()
        {
            return m_CurrentContainerType;
        }
        
        public void OnDialogueChoice(DialogueNode dialogueNode, in int choiceIndex)
        {
            m_ChoiceContainer.OnDialogueChoice(dialogueNode, choiceIndex);
        }
        
        private void CleanCurrentChoices()
        {
            m_ChoiceContainer.CleanCurrentChoices();
        }
        
    }
}