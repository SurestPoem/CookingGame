using UnityEngine;

public class Spatula : MonoBehaviour, IUsable
{
    [SerializeField] private float range = 2f;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private Transform playersCameraTransform;

    public void Use(GameObject user)
    {
        if (Physics.Raycast(playersCameraTransform.position, playersCameraTransform.forward, out RaycastHit raycastHit, range, interactableLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<ObjectGrabbable>(out ObjectGrabbable objectGrabbable))
            {
                objectGrabbable.Flip();
            }
        }
    }
}