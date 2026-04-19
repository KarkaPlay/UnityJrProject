using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableSystem
{
    [RequireComponent(typeof(InteractableOutline))]
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interaction Text")]
        [SerializeField] protected string _interactText = "Взаимодействовать";

        [Header("Conditions")]
        [SerializeField] private List<ConditionBase> _conditions;

        [Header("Events")]
        public UnityEvent OnInteractEvent;

        private Outline _outline;

        public virtual string GetInteractText()
        {
            return _interactText;
        }

        protected bool CanInteract()
        {
            foreach (var condition in _conditions)
            {
                if (!condition.IsMet())
                    return false;
            }
            return true;
        }

        public abstract void OnInteract();
        public virtual void OnHoldInteract() { }
        public virtual void OnStopInteract() { }
    }
}