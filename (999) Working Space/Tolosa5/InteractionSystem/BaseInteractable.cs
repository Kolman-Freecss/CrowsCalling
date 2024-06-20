using System;
using UnityEngine;

namespace _999__Working_Space.Tolosa5.InteractionSystem
{
    public class BaseInteractable : MonoBehaviour
    {
        public readonly int Index = Guid.NewGuid().GetHashCode();

        // private Outline outline;

        protected virtual void Awake()
        {
            // outline = GetComponent<Outline>();
        }

        protected virtual void Start()
        {
            // ShowOutline(false);
        }

        // TODO: Abstract this to a base class
        public void ShowOutline(in bool show = true)
        {
            // if (outline != null)
            // {
            //     outline.enabled = show;
            // }
        }
    }
}