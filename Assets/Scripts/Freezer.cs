using UnityEngine;

public class Freezer : MonoBehaviour, IInteractable
{
    [SerializeField] private ObjectGrabbable ingredientPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay = 1f;
    private float lastInteractionTime = -Mathf.Infinity;

    public void Interact(GameObject interactor)
    {
        if (Time.time - lastInteractionTime < spawnDelay)
            return;

        if (interactor.TryGetComponent<PlayerGrabObject>(out PlayerGrabObject playerGrabObject))
        {
            if (!playerGrabObject.IsHoldingObject())
            {             
                ObjectGrabbable newGrabbedObject = Instantiate(ingredientPrefab, spawnPoint);
                playerGrabObject.PickupObject(newGrabbedObject);
                lastInteractionTime = Time.time;
            }
        }
    }

    public string GetInteractionPrompt()
    {
        return "Take Ingredient";
    }
}
