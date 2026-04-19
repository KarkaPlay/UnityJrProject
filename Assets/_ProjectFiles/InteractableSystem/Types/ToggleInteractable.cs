using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace InteractableSystem
{
    public class ToggleInteractable : InteractableBase
    {
        [Header("Toggle Settings")]
        [SerializeField] private string _interactTextOn = "Открыть";
        [SerializeField] private string _interactTextOff = "Закрыть";
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animatorBoolName = "IsOpen";

        [Header("Toggle Events")]
        public UnityEvent OnTurnOn;
        public UnityEvent OnTurnOff;

        private bool _isOn = false;

        public override string GetInteractText()
        {
            if (!CanInteract())
                return "Недоступно";

            return _isOn ? _interactTextOff : _interactTextOn;
        }

        public override void OnInteract()
        {
            if (!CanInteract())
                return;

            _isOn = !_isOn;
            _animator?.SetBool(_animatorBoolName, _isOn);

            if (_isOn)
                OnTurnOn?.Invoke();
            else
                OnTurnOff?.Invoke();

            OnInteractEvent?.Invoke();
        }
    }
}