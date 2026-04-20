using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
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
        [SerializeField] private string _speakerName;
        [SerializeField][TextArea(2, 5)] private string _text;
        [SerializeField] private List<DialogueChoice> _choices;
        [SerializeField] private int _nextNodeIndex = -1;
        [SerializeField] private bool _triggersQuest = false;

        public string SpeakerName => _speakerName;
        public string Text => _text;
        public List<DialogueChoice> Choices => _choices;
        public int NextNodeIndex => _nextNodeIndex;
        public bool HasChoices => _choices != null && _choices.Count > 0;
        public bool TriggersQuest => _triggersQuest;
    }
}