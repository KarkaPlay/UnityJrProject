using InteractionSystem;
using PlayerControl;
using QuestSystem;
using UnityEngine;

namespace DialogueSystem
{
    [RequireComponent(typeof(InteractableOutline))]
    public class NPC : MonoBehaviour, IInteractable
    {
        [Header("NPC Settings")]
        [SerializeField] private string _npcName = "Незнакомец";

        [Header("Texts")]
        [SerializeField] private string _talkText = "Разговор";
        [SerializeField] private string _giveItemText = "Отдать предмет";

        [Header("Post-Quest Dialogues")]
        [SerializeField] private string _waitingTemplate = "Я всё ещё жду от тебя {0}.";
        [SerializeField] private string _completedMessage = "Спасибо, что нашёл то, что мне нужно.";

        [Header("References")]
        [SerializeField] private DialogueData[] _dialogues;
        [SerializeField] private FetchQuest _fetchQuest;

        private PlayerInventory Inventory => GameManager.Instance.Inventory;
        private DialogueManager DialogueManager => GameManager.Instance.DialogueManager;
        private int _currentDialogueIndex = 0;

        public string NPCName => _npcName;

        public string GetInteractText()
        {
            if (_fetchQuest != null && _fetchQuest.IsActive && Inventory.HasItem)
                return _giveItemText;

            if (_currentDialogueIndex < _dialogues.Length)
                return _talkText;

            if (_fetchQuest != null && (_fetchQuest.IsActive || _fetchQuest.IsCompleted))
                return _talkText;

            return "";
        }

        public void OnInteract()
        {
            if (_fetchQuest != null && _fetchQuest.IsActive && Inventory.HasItem)
            {
                if (_fetchQuest.TryCompleteQuest(Inventory.CurrentItem))
                {
                    Inventory.DestroyCurrentItem();
                    return;
                }
            }

            if (_fetchQuest != null && _fetchQuest.IsCompleted)
            {
                DialogueManager.ShowSingleMessage(_npcName, _completedMessage);
                return;
            }

            if (_fetchQuest != null && _fetchQuest.IsActive)
            {
                string message = string.Format(_waitingTemplate, _fetchQuest.GetTargetItemName());
                DialogueManager.ShowSingleMessage(_npcName, message);
                return;
            }

            if (_currentDialogueIndex < _dialogues.Length)
            {
                DialogueManager.StartDialogue(_dialogues[_currentDialogueIndex], this);
                _currentDialogueIndex++;
            }
        }

        public void OnHoldInteract() { }
        public void OnStopInteract() { }
    }
}