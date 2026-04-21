using DialogueSystem;
using PlayerControl;
using QuestSystem;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueUI _dialogueUI;
        [SerializeField] private FetchQuest _fetchQuest;

        public UnityEvent OnDialogueStarted;
        public UnityEvent OnDialogueEnded;
        public UnityEvent<DialogueData> OnQuestStarted;

        private DialogueData _currentDialogue;
        private int _currentNodeIndex;
        private NPC _activeNPC;

        private PlayerStateMachine StateMachine => GameManager.Instance.StateMachine;

        public void StartDialogue(DialogueData dialogue, NPC npc)
        {
            _activeNPC = npc;
            _currentDialogue = dialogue;
            _currentNodeIndex = 0;

            StateMachine.TransitionTo(StateMachine.DialogueState);
            OnDialogueStarted?.Invoke();
            ShowCurrentNode();
        }

        public void SelectChoice(int choiceIndex)
        {
            var node = _currentDialogue.Nodes[_currentNodeIndex];
            int nextIndex = node.Choices[choiceIndex].NextNodeIndex;

            GoToNode(nextIndex);
        }

        public void AdvanceLinear()
        {
            if (_currentDialogue == null)
            {
                _dialogueUI.Hide();
                StateMachine.TransitionTo(StateMachine.ExplorationState);
                return;
            }

            var node = _currentDialogue.Nodes[_currentNodeIndex];
            GoToNode(node.NextNodeIndex);
        }

        private void GoToNode(int nodeIndex)
        {
            if (nodeIndex < 0 || nodeIndex >= _currentDialogue.Nodes.Count)
            {
                EndDialogue();
                return;
            }

            _currentNodeIndex = nodeIndex;
            ShowCurrentNode();
        }

        private void ShowCurrentNode()
        {
            var node = _currentDialogue.Nodes[_currentNodeIndex];

            string speakerName = (node.Speaker == DialogueSpeaker.Player)
                ? GameManager.Instance.Config.PlayerName
                : _activeNPC.NPCName;

            if (node.TriggersQuest && _fetchQuest != null)
            {
                _fetchQuest.StartQuest();
                string itemName = _fetchQuest.GetTargetItemName();
                _dialogueUI.ShowNode(speakerName, node, $"Принеси мне {itemName}. Буду ждать тебя здесь.");
            }
            else
            {
                _dialogueUI.ShowNode(speakerName, node);
            }
        }

        private void EndDialogue()
        {
            _dialogueUI.Hide();

            _currentDialogue = null;
            StateMachine.TransitionTo(StateMachine.ExplorationState);
            OnDialogueEnded?.Invoke();
        }

        public void ShowSingleMessage(string speakerName, string message)
        {
            StateMachine.TransitionTo(StateMachine.DialogueState);

            var node = new DialogueNode(message, DialogueSpeaker.NPC);
            _dialogueUI.ShowNode(speakerName, node);

            _currentDialogue = null;
            _currentNodeIndex = -1;
        }
    }
}