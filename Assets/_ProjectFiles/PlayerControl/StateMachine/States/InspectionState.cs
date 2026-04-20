using InteractableSystem;
using Items;
using UnityEngine;

namespace PlayerControl
{
    public class InspectionState : PlayerStateBase
    {
        private Item _inspectedItem;
        private Transform _cameraTransform;

        private bool _isMovingToPosition;

        public InspectionState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public void StartInspecting(Item item)
        {
            _inspectedItem = item;
            _cameraTransform = Camera.cameraTransform;
            _isMovingToPosition = true;

            _inspectedItem.transform.SetParent(null);
            _inspectedItem.transform.localScale = item.originalScale;

            if (item is not NoteItem)
                UI.ShowInspection(item.ItemName, item.Description);
        }

        public override void Enter()
        {
            Movement.enabled = false;
            Camera.enabled = false;

            Interaction.IsRaycastEnabled = false;
            Interaction.SetCurrentInteractable(_inspectedItem);

            var outline = _inspectedItem.GetComponent<InteractableOutline>();
            outline.SetInspectionOutline(Config.InspectionOutlineColor, Config.InspectionOutlineWidth);

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

            Movement.enabled = true;
            Camera.enabled = true;

            Interaction.IsRaycastEnabled = true;
            Interaction.ClearCurrentInteractable();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            UI.HideInspection();
            _inspectedItem = null;
        }

        private void MoveItemToInspectionPosition()
        {
            float distance, offsetX, offsetY;

            if (_inspectedItem is NoteItem)
            {
                distance = Config.NoteInspectionDistance;
                offsetX = Config.NoteInspectionOffsetX;
                offsetY = Config.NoteInspectionOffsetY;
            }
            else
            {
                distance = Config.InspectionDistance;
                offsetX = Config.InspectionOffsetX;
                offsetY = Config.InspectionOffsetY;
            }

            Vector3 targetPosition = _cameraTransform.TransformPoint(new Vector3(offsetX, offsetY, distance));

            _inspectedItem.transform.position = Vector3.Lerp(
                _inspectedItem.transform.position,
                targetPosition,
                Time.deltaTime * Config.InspectionMoveSpeed
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
            if (!UI.IsMousePressed) return;

            Vector2 mouseDelta = UI.MouseDelta * Config.InspectionRotationSpeed * Time.deltaTime;

            _inspectedItem.transform.Rotate(_cameraTransform.up, -mouseDelta.x, Space.World);
            _inspectedItem.transform.Rotate(_cameraTransform.right, mouseDelta.y, Space.World);
        }
    }
}