using System.Collections;
using UnityEngine;

namespace InteractionSystem
{
    public class OneTimeInteractable : InteractableBase
    {
        [Header("Texts")]
        [SerializeField] private string _availableText = "Использовать";
        [SerializeField] private string _usedText = "";

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animatorBoolName = "IsOpen";

        private bool _isUsed = false;

        public override string GetInteractText()
        {
            if (_isUsed)
                return _usedText;

            return GetTextOrBlocked(_availableText);
        }

        public override void OnInteract()
        {
            if (_isUsed || !CanInteract())
                return;

            _isUsed = true;
            _animator?.SetBool(_animatorBoolName, true);
            OnInteractEvent?.Invoke();
        }
    }
}