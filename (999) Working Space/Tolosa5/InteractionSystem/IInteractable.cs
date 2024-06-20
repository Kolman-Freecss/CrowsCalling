using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact(Transform interactorTransform);
    string GetInteractText();
    Transform GetTransform();
}
