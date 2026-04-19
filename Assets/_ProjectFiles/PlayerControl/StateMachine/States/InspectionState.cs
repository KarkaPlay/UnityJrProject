using InteractableSystem;
using Items;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerControl
{
    public class InspectionState : PlayerStateBase
    {
        private Item _inspectedItem;
        private Transform _cameraTransform;

        private const float _inspectionDistance = 0.6f;
        private const float _inspectionOffsetX = -0.2f;
        private const float _inspectionOffsetY = 0;
        private const float _rotationSpeed = 50;
        private const float _lerpSpeed = 5f;

        private bool _isMovingToPosition;

        public InspectionState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public void StartInspecting(Item item)
        {
            _inspectedItem = item;
            _cameraTransform = stateMachine.Camera.cameraTransform;
            _isMovingToPosition = true;

            _inspectedItem.transform.SetParent(null);

            if (item is not NoteItem)
                stateMachine.UI.ShowInspection(item.ItemName, item.Description);
        }

        public override void Enter()
        {
            stateMachine.Movement.enabled = false;
            stateMachine.Camera.enabled = false;

            stateMachine.Interaction.IsRaycastEnabled = false;
            stateMachine.Interaction.SetCurrentInteractable(_inspectedItem);

            var outline = _inspectedItem.GetComponent<InteractableOutline>();
            var config = stateMachine.Camera.config;
            outline?.SetInspectionOutline(config.InspectionOutlineColor, config.InspectionOutlineWidth);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public override void Update()
        {
            if (_inspectedItem == null)
                return;

            if (_isMovingToPosition)
            {
                MoveItemToInspectionPosition();
                return;
            }

            HandleRotation();
        }

        public override void Exit()
        {
            var outline = _inspectedItem?.GetComponent<InteractableOutline>();
            outline?.ResetToDefault();

            stateMachine.Movement.enabled = true;
            stateMachine.Camera.enabled = true;

            stateMachine.Interaction.IsRaycastEnabled = true;
            stateMachine.Interaction.ClearCurrentInteractable();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            stateMachine.UI.HideInspection();
            _inspectedItem = null;
        }

        private void MoveItemToInspectionPosition()
        {
            float distance, offsetX, offsetY;

            if (_inspectedItem is NoteItem)
            {
                var config = stateMachine.Camera.config;
                distance = config.NoteInspectionDistance;
                offsetX = config.NoteInspectionOffsetX;
                offsetY = config.NoteInspectionOffsetY;
            }
            else
            {
                distance = _inspectionDistance;
                offsetX = _inspectionOffsetX;
                offsetY = _inspectionOffsetY;
            }

            Vector3 targetPosition = _cameraTransform.TransformPoint(new Vector3(offsetX, offsetY, distance));

            _inspectedItem.transform.position = Vector3.Lerp(
                _inspectedItem.transform.position,
                targetPosition,
                Time.deltaTime * _rotationSpeed
            );

            if (Vector3.Distance(_inspectedItem.transform.position, targetPosition) < 0.01f)
            {
                _inspectedItem.transform.position = targetPosition;
                _isMovingToPosition = false;

                if (_inspectedItem is NoteItem note)
                    note.PlayOpenAnimation();
                else if (_inspectedItem is PocketWatch watch)
                    watch.PlayOpenAnimation();
            }
        }

        private void HandleRotation()
        {
            if (!stateMachine.UI.IsMousePressed) return;

            Vector2 mouseDelta = stateMachine.UI.MouseDelta * _rotationSpeed * Time.deltaTime;

            _inspectedItem.transform.Rotate(_cameraTransform.up, -mouseDelta.x, Space.World);
            _inspectedItem.transform.Rotate(_cameraTransform.right, mouseDelta.y, Space.World);
        }
    }
}