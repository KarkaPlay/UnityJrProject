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
}
