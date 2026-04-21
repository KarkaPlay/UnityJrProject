using UnityEngine;

namespace Objects
{
    public class Valve : MonoBehaviour
    {
        [SerializeField] private float _maxRotationAngle = 360f;
        [SerializeField] private Vector3 _rotationAxis = Vector3.forward;

        private Quaternion _initialRotation;

        private void Start()
        {
            _initialRotation = transform.localRotation;
        }

        public void SetProgress(float progress)
        {
            float angle = progress * _maxRotationAngle;
            transform.localRotation = _initialRotation * Quaternion.AngleAxis(angle, _rotationAxis);
        }
    }
}