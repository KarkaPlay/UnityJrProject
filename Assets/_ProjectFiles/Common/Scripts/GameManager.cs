using DialogueSystem;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    [Header("Player")]
    [SerializeField] private PlayerControl.PlayerStateMachine _playerStateMachine;
    [SerializeField] private PlayerConfig _playerConfig;

    [Header("Systems")]
    [SerializeField] private DialogueManager _dialogueManager;

    // Быстрый доступ к компонентам игрока
    public PlayerControl.PlayerStateMachine StateMachine => _playerStateMachine;
    public PlayerControl.PlayerMovement Movement => _playerStateMachine.Movement;
    public PlayerControl.PlayerCamera Camera => _playerStateMachine.Camera;
    public PlayerControl.PlayerInteraction Interaction => _playerStateMachine.Interaction;
    public PlayerControl.PlayerInventory Inventory => _playerStateMachine.Inventory;
    public PlayerControl.PlayerUI UI => _playerStateMachine.UI;
    public PlayerConfig Config => _playerConfig;
    public DialogueManager DialogueManager => _dialogueManager;
}
