using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _999__Working_Space.Kolman_Freecss.Modules.Common.UI
{
    public class MapTextAnimationWithDOTween : MonoBehaviour
    {
        EventTrigger eventTrigger;
        private TextMeshProUGUI text;

        private Color originalColor;
        private Vector3 originalScale;
        
        private Tween pulseTween;

        private void Awake()
        {
            eventTrigger = GetComponent<EventTrigger>();
            text = GetComponent<TextMeshProUGUI>();
        }

        // private void OnEnable()
        // {
        //     eventTrigger.triggers.Clear();
        //     EventTrigger.Entry entry = new EventTrigger.Entry();
        //     entry.eventID = EventTriggerType.PointerEnter;
        //     entry.callback.AddListener((data) => { OnPointerEnter(); });
        //     eventTrigger.triggers.Add(entry);
        //
        //     entry = new EventTrigger.Entry();
        //     entry.eventID = EventTriggerType.PointerExit;
        //     entry.callback.AddListener((data) => { OnPointerExit(); });
        //     eventTrigger.triggers.Add(entry);
        // }

        private void Start()
        {
            originalScale = transform.localScale;
            originalColor = text.color;
        }

        private void OnEnable()
        {
            // Show the text with a fade in effect
            text.DOFade(1, 1f);
        }

        private void OnDisable()
        {
            // Hide the text with a fade out effect
            text.DOFade(0, 1f);
        }
    }
}