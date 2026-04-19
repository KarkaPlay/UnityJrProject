using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private List<DialogueNode> _nodes;
    [SerializeField] private bool _startsQuest = false;

    public List<DialogueNode> Nodes => _nodes;
    public bool StartsQuest => _startsQuest;
}
