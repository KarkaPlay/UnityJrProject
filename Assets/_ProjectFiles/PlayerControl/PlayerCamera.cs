using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _config;
        [SerializeField] private Transform _cameraTransform;

        private float _pitch = 0f;
        private Vector2 _lookInput;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        void LateUpdate()
        {
            float lookX = _lookInput.x * _config.mouseSensitivity;
            float lookY = _lookInput.y * _config.mouseSensitivity;

            _pitch -= lookY;
            _pitch = Mathf.Clamp(_pitch, _config.UpperLookLimit, _config.LowerLookLimit);
            _cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

            transform.Rotate(Vector3.up * lookX);
        }
    }
}