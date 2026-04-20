using InteractableSystem;
using PlayerControl;
using QuestSystem;
using UnityEngine;

namespace DialogueSystem
{
    [RequireComponent(typeof(InteractableOutline))]
    public class NPC : MonoBehaviour, IInteractable
    {
        [Header("Texts")]
        [SerializeField] private string _talkText = "Разговор";
        [SerializeField] private string _giveItemText = "Отдать предмет";

        [SerializeField] private DialogueData[] _dialogues;
        [SerializeField] private FetchQuest _fetchQuest;

        private PlayerInventory Inventory => GameManager.Instance.Inventory;
        private DialogueManager DialogueManager => GameManager.Instance.DialogueManager;
        private int _currentDialogueIndex = 0;

        public string GetInteractText()
        {
            if (_fetchQuest != null && Inventory.HasItem)
                return _giveItemText;

            if (_currentDialogueIndex < _dialogues.Length)
                return _talkText;

            return "";
        }

        public void OnInteract()
        {
            if (_fetchQuest != null && Inventory.HasItem)
            {
                if (_fetchQuest.TryCompleteQuest(Inventory.CurrentItem))
                {
                    Inventory.DestroyCurrentItem();
                    return;
                }
            }

            if (_currentDialogueIndex < _dialogues.Length)
            {
                DialogueManager.StartDialogue(_dialogues[_currentDialogueIndex]);
                _currentDialogueIndex++;
            }
        }

        public void OnHoldInteract() { }
        public void OnStopInteract() { }
    }
}