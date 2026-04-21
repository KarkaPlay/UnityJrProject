using UnityEngine;

namespace InteractionSystem
{
    public abstract class ConditionBase : MonoBehaviour
    {
        public abstract bool IsMet();
    }
}