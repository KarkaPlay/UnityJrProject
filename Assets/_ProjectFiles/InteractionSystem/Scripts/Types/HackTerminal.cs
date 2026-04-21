using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
{
    public class HackTerminal : InteractableBase
    {
        [Header("Texts")]
        [SerializeField] private string _startText = "Взломать";
        [SerializeField] private string _activeText = "Подтвердить";
        [SerializeField] private string _usedText = "";

        [Header("Hack Terminal")]
        [SerializeField] private float _sliderSpeed = 0.4f;
        [SerializeField] private float _successZoneCenter = 0.5f;
        [SerializeField] private float _successZoneSize = 0.2f;

        [Header("Events")]
        public UnityEvent OnSuccess;
        public UnityEvent OnFailure;

        private float _sliderPosition = 0f;
        private int _direction = 1;
        private bool _isActive = false;
        private bool _isUsed = false;

        private float SuccessMin => _successZoneCenter - _successZoneSize / 2f;
        private float SuccessMax => _successZoneCenter + _successZoneSize / 2f;
        private HackTerminalUI _ui => GameManager.Instance.UI.HackTerminalUI;

        public override string GetInteractText()
        {
            if (_isUsed)
                return _usedText;

            string currentText = _isActive ? _activeText : _startText;
            return GetTextOrBlocked(currentText);
        }

        public override void OnInteract()
        {
            if (_isUsed || !CanInteract())
                return;

            if (!_isActive)
            {
                _isActive = true;
                _sliderPosition = 0f;
                _direction = 1;
                _ui.Show(SuccessMin, SuccessMax);
                return;
            }

            _isActive = false;
            _ui.Hide();

            if (_sliderPosition >= SuccessMin && _sliderPosition <= SuccessMax)
            {
                _isUsed = true;
                OnSuccess?.Invoke();
            }
            else
            {
                OnFailure?.Invoke();
            }
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
                _sliderPosition = 0f;
                _direction = 1;
            }

            _ui.SetSliderValue(_sliderPosition);
        }
    }
}
