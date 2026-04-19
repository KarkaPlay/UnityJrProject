using PlayerControl;
using UnityEngine;

namespace PlayerControl
{
    public class DialogueState : PlayerStateBase
    {
        public DialogueState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.Movement.enabled = false;
            stateMachine.Camera.enabled = false;
            stateMachine.Interaction.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public override void Exit()
        {
            stateMachine.Movement.enabled = true;
            stateMachine.Camera.enabled = true;
            stateMachine.Interaction.enabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}