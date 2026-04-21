using UnityEngine;

namespace InteractionSystem
{
    public class InteractableChild : MonoBehaviour, IInteractable
    {
        public InteractableBase interactableBase;

        public string GetInteractText() => interactableBase.GetInteractText();

        public void OnHoldInteract() => interactableBase.OnHoldInteract();

        public void OnInteract() => interactableBase.OnInteract();

        public void OnStopInteract() => interactableBase.OnStopInteract();
    }
}