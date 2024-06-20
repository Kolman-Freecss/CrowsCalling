// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModalsModule.Scripts
{
    public class UIWidgetModalDialogueOption : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_ChoiceText;

        public TextMeshProUGUI ChoiceText
        {
            get => m_ChoiceText;
            set => m_ChoiceText = value;
        }

        private UIModalDialogueController m_Controller;

        public UIModalDialogueController Controller
        {
            get => m_Controller;
            set => m_Controller = value;
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
            m_Controller.OnOptionSelected(m_choiceIndex);
        }
    }
}