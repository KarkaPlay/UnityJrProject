using System.Collections;
using UnityEngine;

namespace InteractableSystem
{
    public class OneTimeInteractable : InteractableBase
    {
        private bool _isUsed = false;

        public override string GetInteractText()
        {
            if (_isUsed)
                return "";
            if (!CanInteract())
                return "Недоступно";
            return _interactText;
        }

        public override void OnInteract()
        {
            if (_isUsed || !CanInteract())
                return;

            _isUsed = true;
            OnInteractEvent?.Invoke();
        }
    }
}