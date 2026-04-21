using Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    public class FetchQuest : MonoBehaviour
    {
        [SerializeField] private QuestUI _questUI;

        public UnityEvent OnQuestCompleted;

        private List<Item> _possibleItems;
        private Item _targetItem;
        private bool _isActive;
        private bool _isCompleted;

        public bool IsActive => _isActive;
        public bool IsCompleted => _isCompleted;

        private void Start()
        {
            _possibleItems = FindObjectsByType<Item>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .Where(item => !item.ExcludeFromQuests && item is not NoteItem)
                .ToList();
        }

        public void StartQuest()
        {
            if (_isActive || _isCompleted) return;
            if (_possibleItems.Count == 0) return;

            _targetItem = _possibleItems[Random.Range(0, _possibleItems.Count)];
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