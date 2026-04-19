using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace InteractableSystem
{
    public class InteractableOutline : Outline
    {
        private Color _defaultColor;
        private float _defaultWidth;
        private bool _isInitialized;

        private void Start()
        {
            enabled = false;
        }

        public void Initialize(Color color, float width)
        {
            _defaultColor = color;
            _defaultWidth = width;
            _isInitialized = true;
        }

        public void SetOutlineActive(bool isActive)
        {
            if (enabled == isActive) return;

            if (_isInitialized)
            {
                OutlineColor = _defaultColor;
                OutlineWidth = _defaultWidth;
            }

            enabled = isActive;
        }

        public void SetInspectionOutline(Color color, float width)
        {
            OutlineColor = color;
            OutlineWidth = width;
            enabled = true;
        }

        public void ResetToDefault()
        {
            enabled = false;
        }
    }
}