using InteractableSystem;
using PlayerControl;
using System;
using System.Collections;
using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(InteractableOutline))]
    public class Item : MonoBehaviour, IInteractable
    {
        [Header("Item Settings")]
        [SerializeField] private string _itemName = "Предмет";
        [SerializeField] private string _inspectText = "Поднять";
        [SerializeField] protected string _takeText = "Взять";
        [SerializeField][TextArea] private string _description = "Описание предмета";
        [SerializeField] private bool _excludeFromQuests = false;

        [Header("Hold Settings")]
        [SerializeField] private Vector3 _customHoldPosition;
        [SerializeField] private Vector3 _customHoldRotation;
        [SerializeField] private bool _useCustomHoldPosition = false;

        public enum ItemState { InSocket, BeingInspected, InHand }
        public ItemState CurrentState { get; private set; } = ItemState.InSocket;

        public string Description => _description;
        public string ItemName => _itemName;
        public bool ExcludeFromQuests => _excludeFromQuests;
        public Vector3 originalScale { get; private set; }

        protected PlayerStateMachine StateMachine => GameManager.Instance.StateMachine;
        protected PlayerInventory Inventory => GameManager.Instance.Inventory;

        protected ItemSocket _originSocket;

        protected virtual void Awake()
        {
            originalScale = transform.lossyScale;
        }

        public Vector3 GetHoldPosition()
        {
            return _useCustomHoldPosition ? _customHoldPosition : Vector3.zero;
        }

        public Vector3 GetHoldRotation()
        {
            return _useCustomHoldPosition ? _customHoldRotation : Vector3.zero;
        }

        #region IInteractable

        public virtual string GetInteractText()
        {
            return CurrentState switch
            {
                ItemState.InSocket => Inventory.HasItem ? "" : _inspectText,
                ItemState.BeingInspected => _takeText,
                _ => ""
            };
        }

        public virtual void OnInteract()
        {
            if (Inventory.IsBusy || Inventory.HasItem) return;

            switch (CurrentState)
            {
                case ItemState.InSocket:
                    StartInspection();
                    break;

                case ItemState.BeingInspected:
                    PickUp();
                    break;
            }
        }

        public virtual void OnHoldInteract() { }
        public virtual void OnStopInteract() { }

        #endregion

        #region Логика

        public void SetOriginSocket(ItemSocket socket)
        {
            _originSocket = socket;
        }

        public ItemSocket GetOriginSocket()
        {
            return _originSocket;
        }

        protected void StartInspection()
        {
            CurrentState = ItemState.BeingInspected;
            StateMachine.InspectionState.StartInspecting(this);
            StateMachine.TransitionTo(StateMachine.InspectionState);
        }

        protected void PickUp()
        {
            if (Inventory.HasItem) return;

            _originSocket?.ClearSocket();
            CurrentState = ItemState.InHand;
            Inventory.PickUp(this);
            StateMachine.TransitionTo(StateMachine.ExplorationState);
        }

        public virtual void OnReturnToSocket()
        {
            CurrentState = ItemState.InSocket;
        }

        #endregion

        #region Consume

        public virtual void Consume(Action onComplete = null)
        {
            StartCoroutine(ConsumeAnimation(onComplete));
        }

        private IEnumerator ConsumeAnimation(Action onComplete)
        {
            transform.SetParent(null);

            Vector3 startScale = transform.localScale;
            Vector3 startPosition = transform.position;

            // Немного выдвигаем вперёд и увеличиваем
            Camera cam = Camera.main;
            Vector3 peakPosition = cam.transform.position + cam.transform.forward * 0.4f;
            Vector3 peakScale = startScale * 1.3f;

            float duration = 0.6f;
            float elapsed = 0f;

            // Фаза 1: выдвигаем и увеличиваем
            while (elapsed < duration * 0.4f)
            {
                float t = elapsed / (duration * 0.4f);
                t = t * t * (3f - 2f * t);

                transform.position = Vector3.Lerp(startPosition, peakPosition, t);
                transform.localScale = Vector3.Lerp(startScale, peakScale, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Фаза 2: сжимаем в ноль
            Vector3 currentPosition = transform.position;
            elapsed = 0f;

            while (elapsed < duration * 0.6f)
            {
                float t = elapsed / (duration * 0.6f);
                t = t * t;

                transform.localScale = Vector3.Lerp(peakScale, Vector3.zero, t);
                transform.position = Vector3.Lerp(currentPosition, peakPosition, t * 0.2f);

                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
            onComplete?.Invoke();
        }

        #endregion
    }
}