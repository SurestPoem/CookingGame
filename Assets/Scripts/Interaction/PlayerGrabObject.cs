using UnityEngine;

public class PlayerGrabObject : MonoBehaviour
{
    private PlayerReferences references;
    private ObjectGrabbable objectGrabbable;
    private Hotbar hotbar;

    private void Awake()
    {
        hotbar = GetComponent<Hotbar>();
        references = GetComponent<PlayerReferences>();
    }
    public void OnGrabTry(ObjectGrabbable objectToBeGrabbed)
    {
        if (objectGrabbable == null)
        {
            PickupObject(objectToBeGrabbed);
        }
        else
        {
            DropObject();
        }
    }

    public void PickupObject(ObjectGrabbable objectToBeGrabbed)
    {
        if (hotbar != null)
        {
            hotbar.PutAwayItem();
        }
        objectToBeGrabbed.Grab(references.objectGrabPointTransform);
        objectGrabbable = objectToBeGrabbed;
    }

    public void DropObject()
    {
        objectGrabbable.Drop();
        objectGrabbable = null;
    }

    public bool IsHoldingObject()
    {
        return objectGrabbable != null;
    }
}
