using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _999__Working_Space.Kolman_Freecss.Modules.DialogueModule.Scripts.Animations
{
    public class ModalBodyAnimationWithDotTween : MonoBehaviour
    {
        EventTrigger eventTrigger;

        private Tween pulseTween;
        
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            eventTrigger = GetComponent<EventTrigger>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            canvasGroup.alpha = 0.0f;

            canvasGroup.DOFade(1.0f, 0.7f).SetEase(Ease.InOutQuad);
        }
        
        private void OnDisable()
        {
            canvasGroup.DOFade(0.0f, 0.7f).SetEase(Ease.InOutQuad);
        }
    }
}