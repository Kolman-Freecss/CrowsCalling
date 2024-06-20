// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System;
using System.Collections.Generic;
using _999__Working_Space.Kolman_Freecss.Modules.DialogueModalsModule.Scripts;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModalsModule
{
    public class PlayerModalOptionController : MonoBehaviour
    {
        private UIModalDialogueController m_Controller;
        
        public Action<int> OnOptionSelected;
            
        private void Awake()
        {
            m_Controller = GetComponentInChildren<UIModalDialogueController>();
        }

        private void Start()
        {
            Show(false);
        }

        // To show the choices filling list
        public void Invoke(List<String> choiceList, String question) {
            m_Controller.Invoke(choiceList, question);
        }
        
        public void HandleOnOptionSelected(int IndexOptionSelected)
        {
            Show(false);
            OnOptionSelected?.Invoke(IndexOptionSelected);
        }
        
        // Generally when you want to hide the choices
        private void Show(in bool show = true)
        {
            m_Controller.Show(show);
        }
    }
}