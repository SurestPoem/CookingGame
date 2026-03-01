using UnityEngine;

public class PlayerGrabObject : MonoBehaviour
{
    [SerializeField] private Transform objectGrabPointTransform;
    private ObjectGrabbable objectGrabbable;

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
        objectToBeGrabbed.Grab(objectGrabPointTransform);
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
