using UnityEngine;

namespace InteractableSystem
{
    public abstract class ConditionBase : MonoBehaviour
    {
        public abstract bool IsMet();
    }
}