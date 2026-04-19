using UnityEngine;

namespace PlayerControl
{
    public abstract class PlayerStateBase
    {
        protected PlayerStateMachine stateMachine;

        public PlayerStateBase(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
