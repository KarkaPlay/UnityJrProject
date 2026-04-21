using UnityEngine;

namespace PlayerControl
{
    public class DialogueState : PlayerStateBase
    {
        public DialogueState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Movement.enabled = false;
            Camera.enabled = false;
            Interaction.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public override void Exit()
        {
            Movement.enabled = true;
            Camera.enabled = true;
            Interaction.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}