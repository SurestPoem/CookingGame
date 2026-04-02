using UnityEngine;

public class Equippable : MonoBehaviour, IInteractable
{
    [SerializeField] private HotbarItem hotbarItem;

    public void Interact(GameObject interactor)
    {
        Hotbar interactorsHotbar = interactor.GetComponent<Hotbar>();
        if (interactorsHotbar != null)
        {
            interactorsHotbar.AddNewItem(hotbarItem);
        }
    }

    public string GetInteractionPrompt()
    {
        return "Equip";
    }
}