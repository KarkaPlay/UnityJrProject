using Items;
using PlayerControl;
using UnityEngine;

namespace InteractableSystem
{
    public class HasItemCondition : ConditionBase
    {
        [Header("Required Item")]
        [SerializeField] private Item _requiredItem;
        [SerializeField] private bool _consumeOnUse = true;

        private PlayerInventory _inventory;
        private bool _isFulfilled;

        private void Start()
        {
            _inventory = FindFirstObjectByType<PlayerInventory>();
        }

        public override bool IsMet()
        {
            if (_isFulfilled)
                return true;

            if (_inventory == null || !_inventory.HasItem)
                return false;

            if (_requiredItem == null)
                return true;

            return _inventory.CurrentItem == _requiredItem;
        }

        public void Fulfill()
        {
            if (_isFulfilled) return;

            _isFulfilled = true;

            if (_consumeOnUse && _inventory != null)
                _inventory.DestroyCurrentItem();
        }
    }
}