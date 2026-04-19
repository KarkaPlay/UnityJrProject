using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem
{

    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Toggle _checkbox;
        [SerializeField] private TextMeshProUGUI _questText;

        private void Awake()
        {
            if (_container != null)
                _container.SetActive(false);
            else
                gameObject.SetActive(false);

            if (_checkbox != null)
                _checkbox.interactable = false;
        }

        public void Show(string text)
        {
            if (_container != null)
                _container.SetActive(true);
            else
                gameObject.SetActive(true);

            _questText.text = text;
            _checkbox.isOn = false;
        }

        public void MarkCompleted()
        {
            if (_checkbox != null)
                _checkbox.isOn = true;
        }
    }
}