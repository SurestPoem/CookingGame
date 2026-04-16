using UnityEngine;

public class Spatula : MonoBehaviour, IUsable
{
    [SerializeField] private float range = 2f;
    [SerializeField] private LayerMask interactableLayerMask;
    private Transform playersCameraTransform;

    public void Use(GameObject user)
    {
        if (playersCameraTransform == null)
        {
            GetCameraTransform(user);
        }
        if (Physics.Raycast(playersCameraTransform.position, playersCameraTransform.forward, out RaycastHit raycastHit, range, interactableLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<ObjectGrabbable>(out ObjectGrabbable objectGrabbable))
            {
                objectGrabbable.Flip();
            }
        }
    }

    private void GetCameraTransform(GameObject gunOwner)
    {
        PlayerReferences references = gunOwner.GetComponent<PlayerReferences>();
        if (references != null)
        {
            playersCameraTransform = references.playersCameraTransform;
        }
    }
    public bool CanUse()
    {
        return true;
    }
    public string GetUseText()
    {
        return "Flip";
    }
}