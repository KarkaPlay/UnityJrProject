using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private Transform _cameraTransform;

        private IInteractable _currentInteractable;
        private bool _isHolding;

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started) _isHolding = true;

            if (context.performed && _currentInteractable != null)
                _currentInteractable.OnInteract();

            if (context.canceled)
            {
                _isHolding = false;
                _currentInteractable?.OnStopInteract();
            }
        }

        void Update()
        {
            CheckRaycast();

            if (_isHolding && _currentInteractable != null)
                _currentInteractable.OnHoldInteract();
        }

        void CheckRaycast()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, _config.InteractDistance, _config.InteractableLayer))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    if (_currentInteractable != interactable)
                    {
                        _currentInteractable = interactable;
                    }
                    return;
                }
            }

            if (_currentInteractable != null)
            {
                _currentInteractable.OnStopInteract();
                _currentInteractable = null;
            }
        }
    }
}
