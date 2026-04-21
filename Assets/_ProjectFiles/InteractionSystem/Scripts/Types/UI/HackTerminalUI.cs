using UnityEngine;
using UnityEngine.UI;

namespace InteractionSystem
{
    public class HackTerminalUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private RectTransform _successZone;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(float successMin, float successMax)
        {
            gameObject.SetActive(true);

            Canvas.ForceUpdateCanvases();
            PositionSuccessZone(successMin, successMax);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetSliderValue(float value)
        {
            _slider.value = value;
        }

        private void PositionSuccessZone(float min, float max)
        {
            float sliderWidth = _slider.GetComponent<RectTransform>().rect.width;

            float zoneWidth = (max - min) * sliderWidth;

            float zoneLeft = min * sliderWidth - sliderWidth / 2f;
            float zoneCenterX = zoneLeft + zoneWidth / 2f;

            _successZone.anchoredPosition = new Vector2(zoneCenterX, 0f);
            _successZone.sizeDelta = new Vector2(zoneWidth, _successZone.sizeDelta.y);
        }
    }
}