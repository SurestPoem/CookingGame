using UnityEngine;

[CreateAssetMenu(fileName = "HotbarItem", menuName = "ScriptableObjects/Hotbar Item")]
public class HotbarItem : ScriptableObject
{
    public string itemName;
    public GameObject handPrefab;
    public GameObject worldPrefab;
    public Sprite itemIcon;
}