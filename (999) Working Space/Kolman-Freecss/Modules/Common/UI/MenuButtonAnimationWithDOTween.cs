// -------------------------------------------------------------------
// @author Kolman-Freecss https://github.com/Kolman-Freecss
// MIT License
// -------------------------------------------------------------------

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _999__Working_Space.Kolman_Freecss.Modules.Common.UI
{
    public class MenuButtonAnimationWithDOTween : MonoBehaviour
    {
        EventTrigger eventTrigger;

        private TextMeshProUGUI text;

        private Color originalColor;
        private Vector3 originalScale;

        private Tween pulseTween;

        private void Awake()
        {
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
            originalScale = transform.localScale;
            originalColor = text.color;
        }

        public void OnPointerEnter()
        {
            pulseTween = text.transform.DOScale(originalScale * 1.1f, 1f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => OnExpansionComplete())
                .SetLoops(-1);

            // RGB #322C1C
            Color orange = new Color(50f / 255f, 44f / 255f, 28f / 255f);
            text.DOColor(orange, .2f).SetEase(Ease.InOutQuad);
        }

        public void OnPointerExit()
        {
            if (pulseTween != null && pulseTween.IsActive())
            {
                pulseTween.Kill();
            }

            text.DOColor(originalColor, .2f)
                .SetEase(Ease.InOutQuad);

            text.transform.DOScale(originalScale, .2f)
                .SetEase(Ease.InQuad);
        }

        private void OnExpansionComplete()
        {
            text.transform.DOScale(originalScale, 1f)
                .SetEase(Ease.InQuad);
        }
    }
}