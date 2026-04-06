using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image icon;

    [Header("Smooth UI Settings")] //For when global game settings is added (Will be for the SmoothUI setting)
    [SerializeField] private bool SmoothUI = false;
    private Vector3 targetScale = Vector3.one;
    private Vector3 velocity = Vector3.zero;
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
        if (SmoothUI && (transform.localScale - targetScale).sqrMagnitude > 0.0001f)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref velocity, smoothTime);
        }
    }

    public void ScaleHotbarSlot(bool isSelected, float selectedScale)
    {
        if (SmoothUI)
        {
            targetScale = Vector3.one * (isSelected ? selectedScale : 1.0f);
        }
        else
        {
            transform.localScale = Vector3.one * (isSelected ? selectedScale : 1.0f);
        }
        //Add border code later
    }
}
