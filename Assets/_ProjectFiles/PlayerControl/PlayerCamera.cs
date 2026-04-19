using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerCamera : MonoBehaviour
    {
        public PlayerConfig config;

        public Transform cameraTransform;

        private float _pitch = 0f;
        private Vector2 _lookInput;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        private void LateUpdate()
        {
            float lookX = _lookInput.x * config.mouseSensitivity;
            float lookY = _lookInput.y * config.mouseSensitivity;

            _pitch -= lookY;
            _pitch = Mathf.Clamp(_pitch, config.UpperLookLimit, config.LowerLookLimit);
            cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);

            transform.Rotate(Vector3.up * lookX);
        }
    }
}