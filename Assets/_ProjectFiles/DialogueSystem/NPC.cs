using InteractableSystem;
using PlayerControl;
using QuestSystem;
using UnityEngine;

namespace DialogueSystem
{
    [RequireComponent(typeof(InteractableOutline))]
    public class NPC : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueData[] _dialogues;
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private FetchQuest _fetchQuest;

        private PlayerInventory _inventory;
        private int _currentDialogueIndex = 0;

        private void Start()
        {
            _inventory = FindFirstObjectByType<PlayerInventory>();
        }

        public string GetInteractText()
        {
            if (_fetchQuest != null && _inventory.HasItem)
                return "Отдать предмет";

            if (_currentDialogueIndex < _dialogues.Length)
                return "Разговор";

            return "";
        }

        public void OnInteract()
        {
            if (_fetchQuest != null && _inventory.HasItem)
            {
                if (_fetchQuest.TryCompleteQuest(_inventory.CurrentItem))
                {
                    _inventory.DestroyCurrentItem();
                    return;
                }
            }

            if (_currentDialogueIndex < _dialogues.Length)
            {
                _dialogueManager.StartDialogue(_dialogues[_currentDialogueIndex]);
                _currentDialogueIndex++;
            }
        }

        public void OnHoldInteract() { }
        public void OnStopInteract() { }
    }
}