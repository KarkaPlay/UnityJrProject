using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
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
            string currentText = _isOn ? _interactTextOff : _interactTextOn;
            return GetTextOrBlocked(currentText);
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

        public void SetStateWithoutNotify(bool isOn)
        {
            if (_isOn == isOn)
                return;

            _isOn = isOn;
            _animator?.SetBool(_animatorBoolName, _isOn);
        }
    }
}