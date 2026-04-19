using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace PlayerControl
{
    public class InteractionHintUI : MonoBehaviour
    {
        [SerializeField] private GameObject _hintContainer;
        [SerializeField] private TextMeshProUGUI _hintText;

        public void Show(string actionText)
        {
            _hintContainer.SetActive(true);
            _hintText.text = $"E - {actionText}";
        }

        public void Hide()
        {
            _hintContainer.SetActive(false);
        }
    }
}