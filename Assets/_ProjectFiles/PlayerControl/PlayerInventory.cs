using Items;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerControl
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Vector3 _holdPosition = new Vector3(0.3f, -0.3f, 0.5f);
        [SerializeField] private Vector3 _holdRotation = new Vector3(0f, 0f, 0f);
        [SerializeField] private float _holdLerpSpeed = 10f;

        public Item CurrentItem { get; private set; }
        public bool HasItem => CurrentItem != null;
        public bool IsBusy { get; private set; }

        public UnityEvent<Item> OnItemPickedUp;
        public UnityEvent OnItemDropped;

        private Coroutine _activeCoroutine;

        public void PickUp(Item item)
        {
            if (HasItem || IsBusy) return;

            CurrentItem = item;
            CurrentItem.transform.SetParent(_cameraTransform);

            StartMovement(MoveToHoldPosition());
            OnItemPickedUp?.Invoke(item);
        }

        public void Drop()
        {
            if (!HasItem) return;

            ForceStopMovement();

            var item = CurrentItem;
            CurrentItem = null;

            item.transform.SetParent(null);
            OnItemDropped?.Invoke();
        }

        public bool HasItemOfType<T>() where T : Item
        {
            return CurrentItem is T;
        }

        private void StartMovement(IEnumerator routine)
        {
            ForceStopMovement();
            _activeCoroutine = StartCoroutine(routine);
        }

        private void ForceStopMovement()
        {
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
                _activeCoroutine = null;
            }
            IsBusy = false;
        }

        private IEnumerator MoveToHoldPosition()
        {
            IsBusy = true;

            while (CurrentItem != null &&
                   Vector3.Distance(CurrentItem.transform.localPosition, _holdPosition) > 0.01f)
            {
                if (CurrentItem == null) break;

                CurrentItem.transform.localPosition = Vector3.Lerp(
                    CurrentItem.transform.localPosition,
                    _holdPosition,
                    Time.deltaTime * _holdLerpSpeed
                );

                CurrentItem.transform.localRotation = Quaternion.Lerp(
                    CurrentItem.transform.localRotation,
                    Quaternion.Euler(_holdRotation),
                    Time.deltaTime * _holdLerpSpeed
                );

                yield return null;
            }

            if (CurrentItem != null)
            {
                CurrentItem.transform.localPosition = _holdPosition;
                CurrentItem.transform.localRotation = Quaternion.Euler(_holdRotation);
            }

            IsBusy = false;
            _activeCoroutine = null;
        }

        public void DestroyCurrentItem()
        {
            if (!HasItem) return;

            ForceStopMovement();

            var item = CurrentItem;
            CurrentItem = null;

            item.Consume(() => OnItemDropped?.Invoke());
        }
    }
}