using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
{
    public class HoldInteractable : InteractableBase
    {
        [Header("Texts")]
        [SerializeField] private string _holdText = "Крутить";

        [Header("Hold Settings")]
        [SerializeField] private float _holdSpeed = 1f;
        [SerializeField] private float _returnSpeed = 0.5f;

        [Header("Hold Events")]
        public UnityEvent<float> OnProgressChanged;

        private float _progress = 0f;
        private bool _isHolding = false;

        public override string GetInteractText()
        {
            return GetTextOrBlocked(_holdText);
        }

        public override void OnInteract() { }

        public override void OnHoldInteract()
        {
            if (!CanInteract())
                return;

            _isHolding = true;
            _progress = Mathf.Clamp01(_progress + _holdSpeed * Time.deltaTime);
            OnProgressChanged?.Invoke(_progress);
        }

        public override void OnStopInteract()
        {
            _isHolding = false;
            OnInteractEvent?.Invoke();
        }

        private void Update()
        {
            if (!_isHolding && _progress > 0)
            {
                _progress = Mathf.Clamp01(_progress - _returnSpeed * Time.deltaTime);
                OnProgressChanged?.Invoke(_progress);
            }
        }
    }
}