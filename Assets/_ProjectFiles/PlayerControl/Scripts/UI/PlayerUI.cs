using InteractionSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private InteractionHintUI _interactionHint;
        [SerializeField] private InspectionUI _inspectionUI;
        [SerializeField] private HackTerminalUI _hackTerminalUI;

        public Vector2 MouseDelta { get; private set; }
        public bool IsMousePressed { get; private set; }

        public HackTerminalUI HackTerminalUI => _hackTerminalUI;

        public void OnMouseClick(InputAction.CallbackContext context)
        {
            if (context.started)
                IsMousePressed = true;
            if (context.canceled)
                IsMousePressed = false;
        }

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse != null)
            {
                IsMousePressed = mouse.leftButton.isPressed;
                MouseDelta = mouse.delta.ReadValue();
            }
        }

        public void ShowHint(string text)
        {
            _interactionHint.Show(text);
        }

        public void HideHint()
        {
            _interactionHint.Hide();
        }

        public void ShowInspection(string itemName, string description)
        {
            _inspectionUI.Show(itemName, description);
        }

        public void HideInspection()
        {
            _inspectionUI.Hide();
        }

        public void ShowHackTerminal(float successMin, float successMax)
        {
            _hackTerminalUI.Show(successMin, successMax);
        }

        public void SetHackTerminalSlider(float value)
        {
            _hackTerminalUI.SetSliderValue(value);
        }

        public void HideHackTerminal()
        {
            _hackTerminalUI.Hide();
        }
    }
}