using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableSystem
{
    [RequireComponent(typeof(InteractableOutline))]
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Conditions")]
        [SerializeField] private List<ConditionBase> _conditions;

        [Header("Texts")]
        [SerializeField] private string _blockedText = "Недоступно";

        [Header("Events")]
        public UnityEvent OnInteractEvent;

        protected string BlockedText => _blockedText;

        protected bool CanInteract()
        {
            foreach (var condition in _conditions)
            {
                if (!condition.IsMet())
                    return false;
            }

            return true;
        }

        protected string GetTextOrBlocked(string activeText)
        {
            return CanInteract() ? activeText : _blockedText;
        }

        public abstract string GetInteractText();
        public abstract void OnInteract();
        public virtual void OnHoldInteract() { }
        public virtual void OnStopInteract() { }
    }
}