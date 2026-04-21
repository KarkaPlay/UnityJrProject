using InteractionSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;

        private PlayerStateMachine _stateMachine;
        private IInteractable _currentInteractable;
        private InteractableOutline _currentOutline;
        private bool _isButtonDown;
        private float _pressStartTime;
        private const float _holdThreshold = 0.2f;

        private PlayerConfig _config => GameManager.Instance.Config;

        public bool IsRaycastEnabled { get; set; } = true;

        private void Awake()
        {
            _stateMachine = GetComponent<PlayerStateMachine>();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isButtonDown = true;
                _pressStartTime = Time.time;
            }

            if (context.canceled)
            {
                if (Time.time - _pressStartTime < _holdThreshold)
                    _currentInteractable?.OnInteract();

                _isButtonDown = false;
                _currentInteractable?.OnStopInteract();
            }
        }

        private void Update()
        {
            if (IsRaycastEnabled)
                CheckRaycast();

            if (_isButtonDown && _currentInteractable != null)
            {
                if (Time.time - _pressStartTime >= _holdThreshold)
                {
                    _currentInteractable.OnHoldInteract();
                }
            }
        }

        private void CheckRaycast()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _config.InteractDistance, _config.InteractableLayer))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (_currentInteractable != interactable)
                    {
                        if (_currentOutline != null)
                            _currentOutline.SetOutlineActive(false);

                        if (_isButtonDown)
                            _currentInteractable?.OnStopInteract();

                        _currentInteractable = interactable;

                        if (hit.collider.TryGetComponent(out _currentOutline))
                        {
                            _currentOutline.Initialize(_config.DefaultOutlineColor, _config.DefaultOutlineWidth);
                            _currentOutline.SetOutlineActive(true);
                        }
                    }

                    string text = _currentInteractable.GetInteractText();
                    if (string.IsNullOrEmpty(text))
                        _stateMachine.UI.HideHint();
                    else
                        _stateMachine.UI.ShowHint(text);

                    return;
                }
            }

            if (_currentInteractable != null)
            {
                if (_currentOutline != null)
                    _currentOutline.SetOutlineActive(false);
                _currentOutline = null;
                if (_isButtonDown)
                    _currentInteractable.OnStopInteract();
                _currentInteractable = null;
            }

            _stateMachine.UI.HideHint();
        }

        public void SetCurrentInteractable(IInteractable interactable)
        {
            _currentInteractable = interactable;
        }

        public void ClearCurrentInteractable()
        {
            _currentOutline?.SetOutlineActive(false);
            _currentOutline = null;
            _currentInteractable = null;
            _stateMachine.UI.HideHint();
        }
    }
}
