using DG.Tweening;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Animations
{
    public class ModalOptionAnimationWithDOTTween : MonoBehaviour
    {
        EventTrigger eventTrigger;

        private UnityEngine.UI.Image selectionImage;
        
        private TextMeshProUGUI text;

        private Color originalColor;

        private Tween pulseTween;

        private void Awake()
        {
            selectionImage = GetComponentInChildren<UnityEngine.UI.Image>();
            eventTrigger = GetComponent<EventTrigger>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            eventTrigger.triggers.Clear();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnPointerEnter(); });
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((data) => { OnPointerExit(); });
            eventTrigger.triggers.Add(entry);
        }


        private void Start()
        {
            // selectionImage.enabled = false;
            selectionImage.DOFade(0f, 0f);
            originalColor = text.color;
        }

        public void OnPointerEnter()
        {
            selectionImage.DOFade(1f, .2f);
            // selectionImage.enabled = true;

            // RGB #0029FF
            Color blue = new Color(0f / 255f, 41f / 255f, 255f / 255f);
            text.DOColor(blue, .2f).SetEase(Ease.InOutQuad);
        }

        public void OnPointerExit()
        {
            // selectionImage.enabled = false;
            selectionImage.DOFade(0f, .2f);
            if (pulseTween != null && pulseTween.IsActive())
            {
                pulseTween.Kill();
            }

            text.DOColor(originalColor, .2f)
                .SetEase(Ease.InOutQuad);

        }

    }
}