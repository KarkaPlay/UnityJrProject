using UnityEngine;

namespace InteractableSystem
{
    public class HasItemCondition : ConditionBase
    {
        //[SerializeField] private ItemType _requiredItemType;

        public override bool IsMet()
        {
            return true;
            //return PlayerInventory.Instance.CurrentItemType == _requiredItemType;
        }
    }
}