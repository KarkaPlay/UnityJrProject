using DialogueSystem;
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

            // Получаем свойства через новые имена полей
            var speakerProp = nodeProperty.FindPropertyRelative("_speaker");
            var text = nodeProperty.FindPropertyRelative("_text");
            var choices = nodeProperty.FindPropertyRelative("_choices");
            var nextNodeIndex = nodeProperty.FindPropertyRelative("_nextNodeIndex");
            var triggersQuest = nodeProperty.FindPropertyRelative("_triggersQuest");

            // Текст для заголовка (кто говорит)
            string speakerLabel = ((DialogueSpeaker)speakerProp.enumValueIndex).ToString();

            string preview = text.stringValue.Length > 40
                ? text.stringValue.Substring(0, 40) + "..."
                : text.stringValue;

            EditorGUILayout.BeginVertical("box");

            _nodeFoldouts[i] = EditorGUILayout.Foldout(
                _nodeFoldouts[i],
                $"[{i}] {speakerLabel}: {preview}",
                true
            );

            if (_nodeFoldouts[i])
            {
                EditorGUI.indentLevel++;

                // Выбор говорящего (Enum)
                EditorGUILayout.PropertyField(speakerProp, new GUIContent("Speaker Role"));
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

                        // Подсказка, кто будет говорить в следующей ноде
                        if (choiceNext.intValue >= 0 && choiceNext.intValue < nodesProperty.arraySize)
                        {
                            var targetNode = nodesProperty.GetArrayElementAtIndex(choiceNext.intValue);
                            var targetSpeaker = (DialogueSpeaker)targetNode.FindPropertyRelative("_speaker").enumValueIndex;

                            EditorGUILayout.LabelField(
                                $"[{targetSpeaker}]",
                                EditorStyles.miniLabel,
                                GUILayout.Width(70));
                        }
                        else if (choiceNext.intValue == -1)
                        {
                            EditorGUILayout.LabelField("[END]", EditorStyles.miniLabel, GUILayout.Width(70));
                        }

                        if (GUILayout.Button("×", GUILayout.Width(20)))
                        {
                            choices.DeleteArrayElementAtIndex(j);
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    // Линейная нода
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Next Node →", GUILayout.Width(90));

                    nextNodeIndex.intValue = EditorGUILayout.IntField(
                        nextNodeIndex.intValue,
                        GUILayout.Width(40));

                    if (nextNodeIndex.intValue >= 0 && nextNodeIndex.intValue < nodesProperty.arraySize)
                    {
                        var targetNode = nodesProperty.GetArrayElementAtIndex(nextNodeIndex.intValue);
                        var targetSpeaker = (DialogueSpeaker)targetNode.FindPropertyRelative("_speaker").enumValueIndex;

                        EditorGUILayout.LabelField($"[{targetSpeaker}]", EditorStyles.miniLabel);
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
                    return; // Выходим, чтобы избежать ошибок итерации
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(2);
        }

        EditorGUILayout.Space(5);

        if (GUILayout.Button("+ Add Node", GUILayout.Height(30)))
            nodesProperty.InsertArrayElementAtIndex(nodesProperty.arraySize);

        serializedObject.ApplyModifiedProperties();
    }
}