using InteractionSystem;
using PlayerControl;
using System.Collections;
using UnityEngine;

namespace Items
{
    public class ItemSocket : MonoBehaviour, IInteractable
    {
        [Header("Texts")]
        [SerializeField] private string _placeText = "Положить";

        [SerializeField] private Item _startingItem;
        [SerializeField] private Transform _socketTransform;
        [SerializeField] private float _placementSpeed = 5f;

        private PlayerInventory Inventory => GameManager.Instance.Inventory;
        private Item _currentItem;
        private Coroutine _activeCoroutine;
        private bool _isBusy;

        private void Start()
        {
            if (_startingItem != null)
            {
                _currentItem = _startingItem;
                _startingItem.SetOriginSocket(this);
                FixItemScale(_startingItem);
            }
        }

        private void FixItemScale(Item item)
        {
            Vector3 parentScale = _socketTransform.lossyScale;
            item.transform.localScale = new Vector3(
                item.originalScale.x / parentScale.x,
                item.originalScale.y / parentScale.y,
                item.originalScale.z / parentScale.z
            );
        }

        public bool IsEmpty()
        {
            return !_isBusy && _currentItem == null;
        }

        public string GetInteractText()
        {
            if (_isBusy || Inventory.IsBusy)
                return "";

            if (IsEmpty() && Inventory.HasItem)
                return _placeText;

            return "";
        }

        public void OnInteract()
        {
            if (_isBusy || Inventory.IsBusy)
                return;
            if (IsEmpty() && Inventory.HasItem)
                PlaceItem();
        }

        public void OnHoldInteract() { }
        public void OnStopInteract() { }

        public void ClearSocket()
        {
            ForceStopMovement();
            _currentItem = null;
        }

        private void PlaceItem()
        {
            Item item = Inventory.CurrentItem;
            Inventory.Drop();

            _currentItem = item;
            item.SetOriginSocket(this);
            item.OnReturnToSocket();

            ForceStopMovement();
            _activeCoroutine = StartCoroutine(MoveToSocket(item));
        }

        private void ForceStopMovement()
        {
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
                _activeCoroutine = null;
            }
            _isBusy = false;
        }

        private IEnumerator MoveToSocket(Item item)
        {
            _isBusy = true;

            item.transform.SetParent(_socketTransform);

            while (Vector3.Distance(item.transform.position, _socketTransform.position) > 0.01f)
            {
                item.transform.position = Vector3.Lerp(
                    item.transform.position,
                    _socketTransform.position,
                    Time.deltaTime * _placementSpeed
                );

                item.transform.rotation = Quaternion.Lerp(
                    item.transform.rotation,
                    _socketTransform.rotation,
                    Time.deltaTime * _placementSpeed
                );

                yield return null;
            }

            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
            FixItemScale(item);

            _isBusy = false;
            _activeCoroutine = null;
        }
    }
}