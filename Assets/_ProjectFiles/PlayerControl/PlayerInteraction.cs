using InteractableSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private InteractionHintUI _hintUI;

        private InteractableBase _currentInteractable;
        private bool _isButtonDown;
        private float _pressStartTime;
        private const float _holdThreshold = 0.2f;

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
                {
                    _currentInteractable?.OnInteract();
                }

                _isButtonDown = false;
                _currentInteractable?.OnStopInteract();
            }
        }

        private void Update()
        {
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
                if (hit.collider.TryGetComponent(out InteractableBase interactable))
                {
                    if (_currentInteractable != interactable)
                    {
                        if (_isButtonDown)
                        {
                            _currentInteractable?.OnStopInteract();
                            _currentInteractable?.SetOutlineActive(false);
                        }
                        _currentInteractable = interactable;
                    }

                    _currentInteractable.SetOutlineActive(true);

                    string text = _currentInteractable.GetInteractText();

                    if (string.IsNullOrEmpty(text))
                        _hintUI.Hide();
                    else
                        _hintUI.Show(text);

                    return;
                }
            }

            if (_currentInteractable != null)
            {
                if (_isButtonDown) _currentInteractable.OnStopInteract();
                _currentInteractable?.SetOutlineActive(false);
                _currentInteractable = null;
            }

            _hintUI.Hide();
        }
    }
}
