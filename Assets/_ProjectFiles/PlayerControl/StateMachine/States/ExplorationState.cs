using UnityEngine;

namespace PlayerControl
{
    public class ExplorationState : PlayerStateBase
    {
        public ExplorationState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.UI.HideInspection();
            stateMachine.Movement.enabled = true;
            stateMachine.Camera.enabled = true;
            stateMachine.Interaction.enabled = true;
        }

        public override void Exit()
        {

        }
    }
}