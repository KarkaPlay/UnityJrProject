using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5;
    public float gravity = -9.81f;

    [Header("Camera")]
    public float mouseSensitivity = 0.1f;
    public float UpperLookLimit = -90f;
    public float LowerLookLimit = 90f;

    [Header("Interaction")]
    public float InteractDistance = 3f;
    public LayerMask InteractableLayer;

    [Header("Outline")]
    public Color DefaultOutlineColor = Color.yellow;
    public float DefaultOutlineWidth = 4f;

    [Header("Inspection Outline")]
    public Color InspectionOutlineColor = Color.white;
    public float InspectionOutlineWidth = 2f;

    [Header("Inspection")]
    public float InspectionDistance = 0.6f;
    public float InspectionOffsetX = -0.2f;
    public float InspectionOffsetY = 0f;
    public float InspectionRotationSpeed = 50f;
    public float InspectionMoveSpeed = 5f;

    [Header("Note Inspection")]
    public float NoteInspectionDistance = 0.3f;
    public float NoteInspectionOffsetX = 0f;
    public float NoteInspectionOffsetY = 0f;
}
