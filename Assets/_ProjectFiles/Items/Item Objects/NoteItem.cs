using System.Collections;
using TMPro;
using UnityEngine;
using static Items.Item;

namespace Items
{
    public class NoteItem : Item
    {
        [Header("Item Text")]
        [SerializeField] private string _readText = "Прочитать";

        [Header("Note Settings")]
        [SerializeField] private Transform _rightPagePivot;
        [SerializeField] private float _openDuration = 0.8f;
        [SerializeField] private float _closedAngle = 180f;
        [SerializeField] private float _openedAngle = 0f;
        [SerializeField] private Vector3 _rotationAxis = new Vector3(0f, 1f, 0f);

        [Header("Note Text")]
        [SerializeField] private TextMeshProUGUI _leftPageText;
        [SerializeField] private TextMeshProUGUI _rightPageText;
        [SerializeField][TextArea(5, 15)] private string _leftPageContent = "Левая страница";
        [SerializeField][TextArea(5, 15)] private string _rightPageContent = "Правая страница";

        private Coroutine _animationCoroutine;
        private bool _isOpened;

        protected override void Awake()
        {
            base.Awake();
            _rightPagePivot.localRotation = Quaternion.Euler(0f, _closedAngle, 0f);

            if (_leftPageText != null) _leftPageText.text = _leftPageContent;
            if (_rightPageText != null) _rightPageText.text = _rightPageContent;
        }

        public override string GetInteractText()
        {
            return CurrentState switch
            {
                ItemState.InSocket => Inventory.HasItem ? "" : _readText,
                ItemState.BeingInspected => _isOpened ? _takeText : "",
                _ => ""
            };
        }

        public override void OnInteract()
        {
            if (Inventory.IsBusy) return;

            switch (CurrentState)
            {
                case ItemState.InSocket:
                    if (Inventory.HasItem) return;
                    StartInspection();
                    break;

                case ItemState.BeingInspected:
                    if (_isOpened)
                        CloseAndPickUp();
                    break;
            }
        }

        public void PlayOpenAnimation()
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(AnimatePages(_closedAngle, _openedAngle, () =>
            {
                _isOpened = true;
            }));
        }

        private void CloseAndPickUp()
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(AnimatePages(_openedAngle, _closedAngle, () =>
            {
                _isOpened = false;
                PickUp();
            }));
        }

        private IEnumerator AnimatePages(float fromAngle, float toAngle, System.Action onComplete)
        {
            float elapsed = 0f;

            while (elapsed < _openDuration)
            {
                float t = elapsed / _openDuration;
                t = t * t * (3f - 2f * t);

                float angle = Mathf.Lerp(fromAngle, toAngle, t);
                _rightPagePivot.localRotation = Quaternion.Euler(_rotationAxis * angle);

                elapsed += Time.deltaTime;
                yield return null;
            }

            _rightPagePivot.localRotation = Quaternion.Euler(_rotationAxis * toAngle);
            _animationCoroutine = null;
            onComplete?.Invoke();
        }
    }
}