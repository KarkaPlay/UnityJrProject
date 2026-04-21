using UnityEngine;

namespace PlayerControl
{
    public class ExplorationState : PlayerStateBase
    {
        public ExplorationState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            UI.HideInspection();
            Movement.enabled = true;
            Camera.enabled = true;
            Interaction.enabled = true;
        }

        public override void Exit()
        {

        }
    }
}