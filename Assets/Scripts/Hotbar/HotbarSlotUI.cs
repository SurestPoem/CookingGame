using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image icon;
    [SerializeField] private CanvasGroup canvasGroup;
    
    [Header("General settings")]
    [SerializeField] private bool unselectedHotbarFade = true;
    [SerializeField] private bool dontScaleWhenNotHolding = true;
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

    private void Update()
    {
        if (SmoothUI)
        {
            if ((transform.localScale - targetScale).sqrMagnitude > 0.0001f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / smoothTime);
            }
            else
            {
                transform.localScale = targetScale;
            }
        }
    }

    public void ScaleHotbarSlot(bool isSelected, bool isPrevious, float selectedScale, bool isHoldingSomething)
    {
        if (isHoldingSomething || !dontScaleWhenNotHolding)
        {
            if (SmoothUI)
            {
                //When SmoothUI is on, previous slot is smooth scale down back to 1.0. However all other non selected slots are snapped to 1.0 to prevent multiple slots being mid smooth scaling
                if (isSelected || isPrevious)
                {
                    targetScale = Vector3.one * (isSelected ? selectedScale : 1.0f);
                }
                else
                {
                    transform.localScale = Vector3.one;
                }
            }
            else
            {
                transform.localScale = Vector3.one * (isSelected ? selectedScale : 1.0f);
            }
        }
        else
        {
            if (SmoothUI)
            {
                targetScale = Vector3.one;
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }


        if (unselectedHotbarFade)
        {
            canvasGroup.alpha = isSelected ? 1.0f : 0.5f;
        }
        
        if (borderImage != null)
        {
            borderImage.color = isSelected ? selectedColor : normalColor;
        }
    }
}
