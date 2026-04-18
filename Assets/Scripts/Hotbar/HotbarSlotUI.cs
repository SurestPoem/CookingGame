using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HotbarSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image icon;
    [SerializeField] private CanvasGroup canvasGroup;
    
    [Header("General settings")]
    [SerializeField] private bool unselectedHotbarFade = true;
    [SerializeField] private bool dontHighlightWhenNotHolding = true;
    [Header("Border settings")]
    [SerializeField] private Image borderImage;
    [SerializeField] private Color normalColor = new Color(110/255f, 110/255f, 110/255f);
    [SerializeField] private Color selectedColor = new Color(215/255f, 215/255f, 215/255f);
    [Header("Smooth UI Settings")] //For when global game settings is added (Will be for the SmoothUI setting) - Note for when adding more UI elements. SmoothUI must be implemented for them when you can
    [SerializeField] private bool SmoothUI = false;
    private Vector3 targetScale = Vector3.one;
    [SerializeField] private float smoothTime = 0.1f;
    public void SetIcon(Sprite sprite)
    {
        if (icon != null)
        {
            icon.sprite = sprite;
            icon.gameObject.SetActive(sprite != null);
        }
    }

    private void ScaleHotbarSlot(bool isSelected, bool isPrevious, float selectedScale, bool isHoldingSomething)
    {
        Vector3 scaleTarget = Vector3.one * (isSelected ? selectedScale : 1.0f);
        transform.DOKill();
        if (SmoothUI)
        {
            //When SmoothUI is on, previous slot is smooth scale down back to 1.0. However all other non selected slots are snapped to 1.0 to prevent multiple slots being mid smooth scaling
            if (isSelected || isPrevious)
            {
                transform.DOScale(scaleTarget, smoothTime);
            }
            else
            {                
                transform.localScale = scaleTarget;
            }
        }
        else
        {
            transform.localScale = scaleTarget;
        }
    }

    private Color GetBorderColour(bool isSelected, bool isHoldingSomething)
    {        
        if (!isHoldingSomething && dontHighlightWhenNotHolding)
        {
            return normalColor;
        }
        return isSelected ? selectedColor : normalColor;
    }

    public void UpdateSlotVisuals(bool isSelected, bool isPrevious, float selectedScale, bool isHoldingSomething, Sprite slotIcon)
    {
        SlotSelectionVisuals(isSelected, isPrevious, selectedScale, isHoldingSomething);
        SetIcon(slotIcon);
    }

    private void SlotSelectionVisuals(bool isSelected, bool isPrevious, float selectedScale, bool isHoldingSomething)
    {
        ScaleHotbarSlot(isSelected, isPrevious, selectedScale, isHoldingSomething);

        if (unselectedHotbarFade)
        {
            canvasGroup.alpha = isSelected ? 1.0f : 0.5f;
        }

        if (borderImage != null)
        {
            borderImage.DOKill();
            if (SmoothUI)
            {                
                borderImage.DOColor(GetBorderColour(isSelected, isHoldingSomething), smoothTime);
            }
            else
            {
                borderImage.color = GetBorderColour(isSelected, isHoldingSomething);
            }
                
        }
    }
}
