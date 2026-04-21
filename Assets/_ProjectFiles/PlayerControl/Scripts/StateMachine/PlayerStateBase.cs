using UnityEngine;

namespace PlayerControl
{
    public abstract class PlayerStateBase
    {
        protected readonly PlayerStateMachine StateMachine;

        protected PlayerMovement Movement => StateMachine.Movement;
        protected PlayerCamera Camera => StateMachine.Camera;
        protected PlayerInteraction Interaction => StateMachine.Interaction;
        protected PlayerInventory Inventory => StateMachine.Inventory;
        protected PlayerUI UI => StateMachine.UI;
        protected PlayerConfig Config => GameManager.Instance.Config;

        protected PlayerStateBase(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}
