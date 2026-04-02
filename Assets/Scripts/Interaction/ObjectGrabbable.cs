using System.Collections;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour, IInteractable
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    [Header("Layer settings")]
    [SerializeField] private string interactableLayerName = "Interactable";
    [SerializeField] private string beingHeldLayerName = "BeingHeld";
    private int interactableLayerInt;
    private int beingHeldLayerInt;
    private Vector3 idealRotation;
    private bool isPickedUp = false;


    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        interactableLayerInt = LayerMask.NameToLayer(interactableLayerName);
        beingHeldLayerInt = LayerMask.NameToLayer(beingHeldLayerName);
        idealRotation = transform.rotation.eulerAngles;
    }

    public void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent<PlayerGrabObject>(out PlayerGrabObject playerGrabObject))
        {
            playerGrabObject.OnGrabTry(this);
        }
    }

    public void Grab(Transform objectGrabPointTranform)
    {
        this.objectGrabPointTransform = objectGrabPointTranform;
        objectRigidbody.useGravity = false;
        gameObject.layer = beingHeldLayerInt;
        isPickedUp = true;
    }

    public void Drop()
    {
        objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        gameObject.layer = interactableLayerInt;
        isPickedUp = false;
    }
    void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.fixedDeltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);
        }

        if (isPickedUp)
        {
            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(idealRotation)) > 0.1f)
            {
                Quaternion targetRotation = Quaternion.Euler(idealRotation);
                objectRigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));
            }
        }
    }

    //To do
    //Ensures object is on stable ground, gravity disabled,  lifts into the air, flips over, and gravity enabled again 
    public void Flip()
    {
        if (!isPickedUp)
        {
            StartCoroutine(FlipRoutine());
        }
    }

    private IEnumerator FlipRoutine()
    {
        objectRigidbody.useGravity = false;
        
        //Code here

        objectRigidbody.useGravity = true;
        return null;
    }

    public string GetInteractionPrompt()
    {
        return "Pick up";
    }
}
