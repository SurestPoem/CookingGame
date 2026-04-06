using UnityEngine;

public class HotbarUIManager : MonoBehaviour
{
    [SerializeField] HotbarSlotUI[] hotbarSlotUIs = new HotbarSlotUI[9];
    [SerializeField] private float selectionScale = 1.2f;


    public void UpdateUI(HotbarItem[] hotbarSlots, int currentIndex, bool isHoldingSomething)
    {
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
            if (i == currentIndex)
            {
                currentSlotUI.ScaleHotbarSlot(true, selectionScale);
            }
            else
            {
                currentSlotUI.ScaleHotbarSlot(false, selectionScale);
            }
        }
    }

}
