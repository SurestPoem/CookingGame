using UnityEngine;

public class HotbarUIManager : MonoBehaviour
{
    [SerializeField] HotbarSlotUI[] hotbarSlotUIs = new HotbarSlotUI[9];
    [SerializeField] private float selectionScale = 1.2f;
    private int currentSelectedIndex;
    private int previousSelectedIndex;

    public void UpdateUI(HotbarItem[] hotbarSlots, int currentIndex, bool isHoldingSomething)
    {
        if (hotbarSlots.Length != hotbarSlotUIs.Length)
        {
            // https://imgur.com/a/IWDzmmb
            Debug.LogWarning("Length of arrays is different");
            return; //Genuinely don't know why this would happen. Possible - Add proper logic to continue working normally as best as possible, but for now this is fine.
        }
        if (currentIndex != currentSelectedIndex)
        {
            previousSelectedIndex = currentSelectedIndex;
            currentSelectedIndex = currentIndex;
        }

        for (int i = 0;  i < hotbarSlots.Length; i++)
        {
            HotbarSlotUI currentSlotUI = hotbarSlotUIs[i];
            if (hotbarSlots[i] != null)
            {
                currentSlotUI.SetIcon(hotbarSlots[i].itemIcon);
            }
            else
            {
                currentSlotUI.SetIcon(null);
            }
            bool isPrevious = (i == previousSelectedIndex);
            bool isSelected = (i == currentSelectedIndex);
            currentSlotUI.ScaleHotbarSlot(isSelected, isPrevious, selectionScale, isHoldingSomething);
        }
    }
}