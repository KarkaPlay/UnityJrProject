using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public enum DialogueSpeaker { Player, NPC }

    [Serializable]
    public class DialogueChoice
    {
        [SerializeField] private string _text;
        [SerializeField] private int _nextNodeIndex = -1;

        public string Text => _text;
        public int NextNodeIndex => _nextNodeIndex;
    }

    [Serializable]
    public class DialogueNode
    {
        [SerializeField] private DialogueSpeaker _speaker;
        [SerializeField][TextArea(2, 5)] private string _text;
        [SerializeField] private List<DialogueChoice> _choices;
        [SerializeField] private int _nextNodeIndex = -1;
        [SerializeField] private bool _triggersQuest = false;

        public DialogueSpeaker Speaker => _speaker;
        public string Text => _text;
        public List<DialogueChoice> Choices => _choices;
        public int NextNodeIndex => _nextNodeIndex;
        public bool HasChoices => _choices != null && _choices.Count > 0;
        public bool TriggersQuest => _triggersQuest;

        public DialogueNode(string text, DialogueSpeaker speaker)
        {
            _text = text;
            _speaker = speaker;
        }
    }
}