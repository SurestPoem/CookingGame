using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    [Header("PlayerTransforms")]
    public Transform playersCameraTransform;
    public Transform objectGrabPointTransform;
    public GameObject playerHand;
    [Header("UI Managers")]
    public HotbarUIManager hotbarUiManager;
    public InteractUI interactUI;
}