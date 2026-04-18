using UnityEngine;

public class HotbarUIManager : MonoBehaviour
{
    [SerializeField] HotbarSlotUI[] hotbarSlotUIs = new HotbarSlotUI[9];
    [SerializeField] private float selectionScale = 1.2f;

    public void UpdateUI(HotbarItem[] hotbarSlots, int currentIndex, int previousIndex, bool isHoldingSomething) //Called in Hotbar when the hotbar changes in any way
    {
        if (hotbarSlots.Length != hotbarSlotUIs.Length)
        {
            // https://imgur.com/a/IWDzmmb
            Debug.LogWarning("Length of arrays is different");
            return; //Genuinely don't know why this would happen. Possible - Add proper logic to continue working normally as best as possible, but for now this is fine.
        }
        for (int i = 0;  i < hotbarSlots.Length; i++)
        {
            HotbarSlotUI currentSlotUI = hotbarSlotUIs[i];
            bool isPrevious = (i == previousIndex);
            bool isSelected = (i == currentIndex);
            Sprite slotIcon = hotbarSlots[i] != null ? hotbarSlots[i].itemIcon : null;
            currentSlotUI.UpdateSlotVisuals(isSelected, isPrevious, selectionScale, isHoldingSomething, slotIcon);
        }
    }
}