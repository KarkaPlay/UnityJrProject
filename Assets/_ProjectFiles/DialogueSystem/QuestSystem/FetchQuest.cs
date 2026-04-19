using Items;
using PlayerControl;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    public class FetchQuest : MonoBehaviour
    {
        [Header("Available Items (excluding key and note)")]
        [SerializeField] private Item[] _possibleItems;
        [SerializeField] private QuestUI _questUI;

        public UnityEvent OnQuestCompleted;

        private Item _targetItem;
        private bool _isActive;
        private bool _isCompleted;

        private void Start()
        {
            _possibleItems = FindObjectsByType<Item>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .Where(item => item is not KeyItem && item is not NoteItem)
                .ToArray();
        }

        public void StartQuest()
        {
            if (_isActive || _isCompleted) return;
            if (_possibleItems.Length == 0) return;

            _targetItem = _possibleItems[Random.Range(0, _possibleItems.Length)];
            _isActive = true;

            _questUI.Show($"Принести: {_targetItem.ItemName}");
        }

        public string GetTargetItemName()
        {
            return _targetItem != null ? _targetItem.ItemName : "";
        }

        public bool TryCompleteQuest(Item item)
        {
            if (!_isActive || _isCompleted) return false;
            if (item != _targetItem) return false;

            _isCompleted = true;
            _isActive = false;
            _questUI.MarkCompleted();
            OnQuestCompleted?.Invoke();
            return true;
        }
    }
}