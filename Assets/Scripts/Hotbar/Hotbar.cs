using NUnit.Framework;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hotbar : MonoBehaviour
{
    public PlayerControls inputs;
    private InputAction scrollAction;
    private InputAction useAction;
    private InputAction dropAction;
    [SerializeField] private int hotbarSize = 9;
    [SerializeField] private int selectedSlot = 0;
    [SerializeField] private HotbarItem[] hotbarSlots = new HotbarItem[9];
    [SerializeField] private GameObject currentlyHeldItem;
    [SerializeField] private GameObject playerHand;


    private void Awake()
    {
        inputs = new PlayerControls();
        scrollAction = inputs.PlayerCharacter.ScrollHotbar;
        useAction = inputs.PlayerCharacter.Use;
        dropAction = inputs.PlayerCharacter.Drop;
    }

    void OnEnable()
    {
        scrollAction.Enable();
        useAction.Enable();
        dropAction.Enable();
    }

    void OnDisable()
    {
        scrollAction.Disable();
        useAction.Disable();
        dropAction.Disable();
    }

    void Update()
    {
        float scrollValue = scrollAction.ReadValue<float>();
        SwitchItem(scrollValue);

        if (useAction.triggered)
        {
            UseCurrentItem();
        }

        if (dropAction.triggered)
        {
            DropItem();
        }
    }

    private void SwitchItem(float scrollValue)
    {
        if (scrollValue > 0)
        {
            selectedSlot--;
            if (selectedSlot < 0)
            {
                selectedSlot = hotbarSlots.Length - 1;
            }
            Debug.Log("Scrolled Up");
        }
        else if (scrollValue < 0)
        {
            selectedSlot++;
            if (selectedSlot >= hotbarSlots.Length)
            {
                selectedSlot = 0;
            }
            Debug.Log("Scrolled Down");
        }

        PullOutItem();
    }

    public void AddNewItem(HotbarItem item)
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarSlots[i] == null)
            {
                hotbarSlots[i] = item;
                return;
            }
        }
        ReplaceItem(item);
    }

    private void ReplaceItem(HotbarItem newItem)
    {
        DropItem();
        hotbarSlots[selectedSlot] = newItem;
    }

    public void DropItem()
    {
        if (hotbarSlots[selectedSlot] == null) { return; }
        GameObject itemToDrop = hotbarSlots[selectedSlot].worldPrefab;
        Vector3 spawnPosition = playerHand.transform.position + playerHand.transform.forward * 0.5f;
        if (currentlyHeldItem != null)
        {
            PutAwayItem();
        }
        Instantiate(itemToDrop, spawnPosition, Quaternion.identity);
        hotbarSlots[selectedSlot] = null;
    }

    public void PullOutItem()
    {
        if (hotbarSlots[selectedSlot] == null) { return; }
        GameObject itemToPullOut = hotbarSlots[selectedSlot].handPrefab;
        PlayerGrabObject grabObject = GetComponent<PlayerGrabObject>();
        if (grabObject != null)
        {
            if (grabObject.IsHoldingObject())
            {
                grabObject.DropObject();
            }
            currentlyHeldItem = Instantiate(itemToPullOut, playerHand.transform);
        }
    }

    public void PutAwayItem()
    {
        Destroy(currentlyHeldItem);
        currentlyHeldItem = null;
    }

    private void UseCurrentItem()
    {
        if (currentlyHeldItem != null)
        {
            if (currentlyHeldItem.TryGetComponent<IUsable>(out IUsable usable))
            {
                usable.Use(gameObject);
            }
        }
    }
}
