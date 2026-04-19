using TMPro;
using UnityEngine;

public class InspectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private void Awake()
    {
        _container.SetActive(false);
    }

    public void Show(string itemName, string description)
    {
        _container.SetActive(true);
        _itemNameText.text = itemName;
        _descriptionText.text = description;
    }

    public void Hide()
    {
        _container.SetActive(false);
    }
}
