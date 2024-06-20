// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Data.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.UI
{
    public class UIWidgetDialogueChoice : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_ChoiceText;

        public TextMeshProUGUI ChoiceText
        {
            get => m_ChoiceText;
            set => m_ChoiceText = value;
        }
        
        private UIDialogueController m_dialogueController; // Setted by the UIDialogueController when dynamically creating the choices

        public UIDialogueController DialogueController
        {
            get => m_dialogueController;
            set => m_dialogueController = value;
        }
        
        private DialogueNode m_referencedNode;
        
        public DialogueNode ReferencedNode
        {
            get => m_referencedNode;
            set => m_referencedNode = value;
        }
        
        private int m_choiceIndex;
        
        public int ChoiceIndex
        {
            get => m_choiceIndex;
            set => m_choiceIndex = value;
        }

        private void Start()
        {
            GetComponentInChildren<Button>().onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            m_dialogueController.OnDialogueChoice(m_referencedNode, m_choiceIndex);
        }
    }
}