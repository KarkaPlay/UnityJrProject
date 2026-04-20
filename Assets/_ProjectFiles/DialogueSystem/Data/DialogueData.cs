using DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private List<DialogueNode> _nodes;

    public List<DialogueNode> Nodes => _nodes;
}
