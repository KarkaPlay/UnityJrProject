using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _config;

        private CharacterController _controller;
        private Vector2 _moveInput;
        private Vector3 _velocity;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            ApplyMovement();
            ApplyGravity();
        }

        private void ApplyMovement()
        {
            Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
            _controller.Move(move * _config.moveSpeed * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (_controller.isGrounded && _velocity.y < 0)
                _velocity.y = -2f;

            _velocity.y += _config.gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}