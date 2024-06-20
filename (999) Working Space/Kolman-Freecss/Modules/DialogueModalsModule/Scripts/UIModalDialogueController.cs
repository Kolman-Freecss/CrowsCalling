// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModalsModule.Scripts
{
    public class UIModalDialogueController : MonoBehaviour
    {
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private PlayerModalOptionController playerModalOptionController;

        [SerializeField] private TextMeshProUGUI questionHeaderText;
        [SerializeField]
        UIWidgetModalDialogueOption[] choices;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            playerModalOptionController = GetComponentInParent<PlayerModalOptionController>();
            choices = GetComponentsInChildren<UIWidgetModalDialogueOption>();
        }
        
        public void OnOptionSelected(in int choiceIndex)
        {
            Debug.Log("Option selected: " + choiceIndex);
            playerModalOptionController.HandleOnOptionSelected(choiceIndex);
        }

        public void Invoke(List<String> choiceList, String question, in bool show = true)
        {
            if (choiceList == null)
            {
                Debug.LogError("Choice list is null");
                return;
            }
            if (show)
            {
                if (choiceList.Count > choices.Length)
                {
                    Debug.LogError("Too many choices to display in the modal. Max choices: " + choices.Length + " Choices to display: " + choiceList.Count);
                    return;
                }
                questionHeaderText.text = question;
                FillChoices(choiceList);
            }
            Show(show);
        }
        
        public void Show(in bool show = true)
        {
            canvas.enabled = show;
            canvasGroup.alpha = show ? 1.0f : 0.0f;
            ShowMouseCursor(show);
        }
        
        private void FillChoices(List<String> choiceList)
        {
            for (int i = 0; i < choices.Length; i++)
            {
                if (i < choiceList.Count)
                {
                    choices[i].ChoiceText.text = choiceList[i];
                    choices[i].Controller = this;
                    choices[i].ChoiceIndex = i;
                }
            }
        }
        
        private void ShowMouseCursor(in bool show = true)
        {
            Cursor.visible = show;
            Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}