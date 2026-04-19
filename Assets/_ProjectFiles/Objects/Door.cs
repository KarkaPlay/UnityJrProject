using UnityEngine;

namespace Objects
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Vector3 _openOffset = new Vector3(0, 3f, 0);
        private Vector3 _initialPosition;

        private void Start()
        {
            _initialPosition = transform.localPosition;
        }

        public void SetProgress(float progress)
        {
            transform.localPosition = _initialPosition + _openOffset * progress;
        }
    }
}