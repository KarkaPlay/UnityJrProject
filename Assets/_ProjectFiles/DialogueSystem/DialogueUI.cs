using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private TextMeshProUGUI _speakerText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Transform _choicesContainer;
    [SerializeField] private Button _choiceButtonPrefab;
    [SerializeField] private Button _continueButton;

    private DialogueManager _manager => GameManager.Instance.DialogueManager;

    private void Awake()
    {
        _container.SetActive(false);
        _continueButton.onClick.AddListener(() => _manager.AdvanceLinear());
    }

    public void ShowNode(DialogueNode node)
    {
        _container.SetActive(true);
        _speakerText.text = node.SpeakerName;
        _dialogueText.text = node.Text;

        ClearChoices();

        if (node.HasChoices)
        {
            _continueButton.gameObject.SetActive(false);

            for (int i = 0; i < node.Choices.Count; i++)
            {
                int choiceIndex = i;
                Button button = Instantiate(_choiceButtonPrefab, _choicesContainer);
                button.GetComponentInChildren<TextMeshProUGUI>().text = node.Choices[i].Text;
                button.onClick.AddListener(() => _manager.SelectChoice(choiceIndex));
            }
        }
        else
        {
            _continueButton.gameObject.SetActive(true);
        }
    }

    public void ShowNode(DialogueNode node, string overrideText)
    {
        _container.SetActive(true);
        _speakerText.text = node.SpeakerName;
        _dialogueText.text = overrideText;

        ClearChoices();
        _continueButton.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _container.SetActive(false);
    }

    private void ClearChoices()
    {
        foreach (Transform child in _choicesContainer)
            Destroy(child.gameObject);
    }
}
