using Items;
using PlayerControl;
using UnityEngine;

namespace InteractionSystem
{
    public class HasItemCondition : ConditionBase
    {
        [Header("Required Item")]
        [SerializeField] private Item _requiredItem;
        [SerializeField] private bool _consumeOnUse = true;

        private PlayerInventory Inventory => GameManager.Instance.Inventory;
        private bool _isFulfilled;

        public override bool IsMet()
        {
            if (_isFulfilled)
                return true;

            if (!Inventory.HasItem)
                return false;

            if (_requiredItem == null)
                return true;

            return Inventory.CurrentItem == _requiredItem;
        }

        public void Fulfill()
        {
            if (_isFulfilled)
                return;

            _isFulfilled = true;

            if (_consumeOnUse)
                Inventory.DestroyCurrentItem();
        }
    }
}