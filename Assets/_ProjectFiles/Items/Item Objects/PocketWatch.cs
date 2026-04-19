using System.Collections;
using UnityEngine;

namespace Items
{
    public class PocketWatch : Item
    {
        [Header("Watch Settings")]
        [SerializeField] private Transform _lidPivot;
        [SerializeField] private Transform _minuteHandPivot;
        [SerializeField] private Transform _hourHandPivot;
        [SerializeField] private float _openDuration = 0.5f;
        [SerializeField] private float _closedAngle = 0f;
        [SerializeField] private float _openedAngle = -120f;
        [SerializeField] private Vector3 _lidRotationAxis = new Vector3(1f, 0f, 0f);
        [SerializeField] private float _minuteHandSpeed = 60f;
        [SerializeField] private float _hourHandSpeed = 5f;

        private Coroutine _animationCoroutine;
        private bool _isOpened;

        public override string GetInteractText()
        {
            return CurrentState switch
            {
                ItemState.InSocket => Inventory.HasItem ? "" : "Поднять",
                ItemState.BeingInspected => "Взять",
                _ => ""
            };
        }

        public void PlayOpenAnimation()
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(OpenLid());
        }

        public override void OnReturnToSocket()
        {
            base.OnReturnToSocket();
            _isOpened = false;

            if (_lidPivot != null)
                _lidPivot.localRotation = Quaternion.Euler(_lidRotationAxis * _closedAngle);
        }

        private void Update()
        {
            if (!_isOpened) return;

            if (_minuteHandPivot != null)
                _minuteHandPivot.Rotate(0f, _minuteHandSpeed * Time.deltaTime, 0f);

            if (_hourHandPivot != null)
                _hourHandPivot.Rotate(0f, _hourHandSpeed * Time.deltaTime, 0f);
        }

        private IEnumerator OpenLid()
        {
            float elapsed = 0f;

            while (elapsed < _openDuration)
            {
                float t = elapsed / _openDuration;
                t = t * t * (3f - 2f * t);

                float angle = Mathf.Lerp(_closedAngle, _openedAngle, t);
                _lidPivot.localRotation = Quaternion.Euler(_lidRotationAxis * angle);

                elapsed += Time.deltaTime;
                yield return null;
            }

            _lidPivot.localRotation = Quaternion.Euler(_lidRotationAxis * _openedAngle);
            _isOpened = true;
            _animationCoroutine = null;
        }
    }
}