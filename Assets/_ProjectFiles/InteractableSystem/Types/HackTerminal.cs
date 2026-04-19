using UnityEngine;
using UnityEngine.Events;

namespace InteractableSystem
{
    public class HackTerminal : InteractableBase
    {
        [Header("Hack Terminal")]
        [SerializeField] private float _sliderSpeed = 0.4f;
        [SerializeField] private float _successZoneCenter = 0.5f;
        [SerializeField] private float _successZoneSize = 0.2f;
        [SerializeField] private HackTerminalUI _ui;

        [Header("Events")]
        public UnityEvent OnSuccess;
        public UnityEvent OnFailure;

        private float _sliderPosition = 0f;
        private int _direction = 1;
        private bool _isActive = false;
        private bool _isUsed = false;

        private float SuccessMin => _successZoneCenter - _successZoneSize / 2f;
        private float SuccessMax => _successZoneCenter + _successZoneSize / 2f;

        public override string GetInteractText()
        {
            if (_isUsed)
                return "";

            return _isActive ? "Взломать" : _interactText;
        }

        public override void OnInteract()
        {
            if (_isUsed)
                return;

            if (!_isActive)
            {
                _isActive = true;
                _ui.Show(SuccessMin, SuccessMax);
                return;
            }

            _isActive = false;
            _isUsed = true;
            _ui.Hide();

            if (_sliderPosition >= SuccessMin && _sliderPosition <= SuccessMax)
                OnSuccess?.Invoke();
            else
                OnFailure?.Invoke();
        }

        public override void OnHoldInteract()
        {

        }

        public override void OnStopInteract()
        {

        }

        private void Update()
        {
            if (!_isActive)
                return;

            _sliderPosition += _direction * _sliderSpeed * Time.deltaTime;

            if (_sliderPosition >= 1f)
            {
                _sliderPosition = 1f;
                _direction = -1;
            }
            else if (_sliderPosition <= 0f)
            {
                _sliderPosition = 0f; _direction = 1;
            }

            _ui.SetSliderValue(_sliderPosition);
        }
    }
}
