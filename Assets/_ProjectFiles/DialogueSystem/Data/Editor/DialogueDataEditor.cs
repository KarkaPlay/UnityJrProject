using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueData))]
public class DialogueDataEditor : Editor
{
    private bool[] _nodeFoldouts;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var nodesProperty = serializedObject.FindProperty("_nodes");

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Dialogue Nodes", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // Инициализация массива фолдаутов
        if (_nodeFoldouts == null || _nodeFoldouts.Length != nodesProperty.arraySize)
            _nodeFoldouts = new bool[nodesProperty.arraySize];

        for (int i = 0; i < nodesProperty.arraySize; i++)
        {
            var nodeProperty = nodesProperty.GetArrayElementAtIndex(i);

            var speakerName = nodeProperty.FindPropertyRelative("_speakerName");
            var text = nodeProperty.FindPropertyRelative("_text");
            var choices = nodeProperty.FindPropertyRelative("_choices");
            var nextNodeIndex = nodeProperty.FindPropertyRelative("_nextNodeIndex");
            var triggersQuest = nodeProperty.FindPropertyRelative("_triggersQuest");

            string preview = text.stringValue.Length > 40
                ? text.stringValue.Substring(0, 40) + "..."
                : text.stringValue;

            EditorGUILayout.BeginVertical("box");

            _nodeFoldouts[i] = EditorGUILayout.Foldout(
                _nodeFoldouts[i],
                $"[{i}] {speakerName.stringValue}: {preview}",
                true
            );

            if (_nodeFoldouts[i])
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(speakerName, new GUIContent("Speaker"));
                EditorGUILayout.PropertyField(text, new GUIContent("Text"));

                EditorGUILayout.Space(5);

                EditorGUILayout.PropertyField(triggersQuest, new GUIContent("Triggers Quest"));

                EditorGUILayout.Space(5);

                if (choices.arraySize > 0)
                {
                    EditorGUILayout.LabelField("Choices", EditorStyles.boldLabel);

                    for (int j = 0; j < choices.arraySize; j++)
                    {
                        var choice = choices.GetArrayElementAtIndex(j);
                        var choiceText = choice.FindPropertyRelative("_text");
                        var choiceNext = choice.FindPropertyRelative("_nextNodeIndex");

                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.PropertyField(choiceText, GUIContent.none);

                        EditorGUILayout.LabelField("→", GUILayout.Width(15));

                        choiceNext.intValue = EditorGUILayout.IntField(
                            choiceNext.intValue,
                            GUILayout.Width(40));

                        if (choiceNext.intValue >= 0 && choiceNext.intValue < nodesProperty.arraySize)
                        {
                            var targetSpeaker = nodesProperty
                                .GetArrayElementAtIndex(choiceNext.intValue)
                                .FindPropertyRelative("_speakerName").stringValue;

                            EditorGUILayout.LabelField(
                                $"[{targetSpeaker}]",
                                EditorStyles.miniLabel,
                                GUILayout.Width(100));
                        }
                        else if (choiceNext.intValue == -1)
                        {
                            EditorGUILayout.LabelField("[END]", EditorStyles.miniLabel);
                        }

                        if (GUILayout.Button("×", GUILayout.Width(20)))
                            choices.DeleteArrayElementAtIndex(j);

                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("Next Node →", GUILayout.Width(90));

                    nextNodeIndex.intValue = EditorGUILayout.IntField(
                        nextNodeIndex.intValue,
                        GUILayout.Width(40));

                    if (nextNodeIndex.intValue >= 0 && nextNodeIndex.intValue < nodesProperty.arraySize)
                    {
                        var targetSpeaker = nodesProperty
                            .GetArrayElementAtIndex(nextNodeIndex.intValue)
                            .FindPropertyRelative("_speakerName").stringValue;

                        EditorGUILayout.LabelField(
                            $"[{targetSpeaker}]",
                            EditorStyles.miniLabel);
                    }
                    else if (nextNodeIndex.intValue == -1)
                    {
                        EditorGUILayout.LabelField("[END]", EditorStyles.miniLabel);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space(5);

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("+ Choice"))
                    choices.InsertArrayElementAtIndex(choices.arraySize);

                if (GUILayout.Button("Delete Node"))
                {
                    nodesProperty.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(3);
        }

        EditorGUILayout.Space(5);

        if (GUILayout.Button("+ Add Node", GUILayout.Height(30)))
            nodesProperty.InsertArrayElementAtIndex(nodesProperty.arraySize);

        serializedObject.ApplyModifiedProperties();
    }
}