using PlayerControl;
using System.Collections;
using UnityEngine;

namespace PlayerControl
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [HideInInspector] public PlayerMovement Movement;
        [HideInInspector] public PlayerCamera Camera;
        [HideInInspector] public PlayerInteraction Interaction;
        [HideInInspector] public PlayerInventory Inventory;
        [HideInInspector] public PlayerUI UI;

        private PlayerStateBase _currentState;

        public PlayerStateBase CurrentState => _currentState;

        // Все стейты должны быть тут
        public ExplorationState ExplorationState { get; private set; }
        public InspectionState InspectionState { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<PlayerMovement>();
            Camera = GetComponent<PlayerCamera>();
            Interaction = GetComponent<PlayerInteraction>();
            Inventory = GetComponent<PlayerInventory>();
            UI = GetComponent<PlayerUI>();

            ExplorationState = new ExplorationState(this);
            InspectionState = new InspectionState(this);
        }

        private void Start()
        {
            TransitionTo(ExplorationState);
        }

        private void Update()
        {
            _currentState?.Update();
        }

        public void TransitionTo(PlayerStateBase newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}