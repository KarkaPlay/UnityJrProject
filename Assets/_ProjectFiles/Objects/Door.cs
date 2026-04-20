using System.Collections;
using UnityEngine;

namespace Objects
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Vector3 _openOffset = new Vector3(0, 3f, 0);
        [SerializeField] private float _openSpeed = 2f;

        private Vector3 _initialPosition;
        private Coroutine _activeCoroutine;

        private void Start()
        {
            _initialPosition = transform.localPosition;
        }

        public void SetProgress(float progress)
        {
            transform.localPosition = _initialPosition + _openOffset * progress;
        }

        public void OpenSmooth()
        {
            StopActiveCoroutine();
            _activeCoroutine = StartCoroutine(AnimateTo(_initialPosition + _openOffset));
        }

        public void CloseSmooth()
        {
            StopActiveCoroutine();
            _activeCoroutine = StartCoroutine(AnimateTo(_initialPosition));
        }

        private IEnumerator AnimateTo(Vector3 targetPosition)
        {
            while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
            {
                transform.localPosition = Vector3.Lerp(
                    transform.localPosition,
                    targetPosition,
                    Time.deltaTime * _openSpeed
                );

                yield return null;
            }

            transform.localPosition = targetPosition;
            _activeCoroutine = null;
        }

        private void StopActiveCoroutine()
        {
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
                _activeCoroutine = null;
            }
        }
    }
}