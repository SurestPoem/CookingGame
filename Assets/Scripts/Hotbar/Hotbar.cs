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
    private InputAction holsterAction;
    private InputAction hotbarSelectionAction;
    [SerializeField] private int currentSlotIndex = 0;
    [SerializeField] private HotbarItem[] hotbarSlots = new HotbarItem[9];
    [SerializeField] private GameObject currentlyHeldItem;
    [SerializeField] private float throwForce = 3f;
    private PlayerGrabObject grabObject;

    private PlayerReferences references;


    private void Awake()
    {
        references = GetComponent<PlayerReferences>();
        inputs = new PlayerControls();
        scrollAction = inputs.PlayerCharacter.ScrollHotbar;
        useAction = inputs.PlayerCharacter.Use;
        dropAction = inputs.PlayerCharacter.Drop;
        holsterAction = inputs.PlayerCharacter.Holster;
        hotbarSelectionAction = inputs.PlayerCharacter.HotbarSelection;
        grabObject = GetComponent<PlayerGrabObject>();
    }

    void OnEnable()
    {
        scrollAction.Enable();
        useAction.Enable();
        dropAction.Enable();
        holsterAction.Enable();
        hotbarSelectionAction.Enable();
    }

    void OnDisable()
    {
        scrollAction.Disable();
        useAction.Disable();
        dropAction.Disable();
        holsterAction.Disable();
        hotbarSelectionAction.Disable();
    }

    private void Start()
    {
        references.hotbarUiManager.UpdateUI(hotbarSlots, currentSlotIndex, currentlyHeldItem != null);
    }
    void Update()
    {
        if (scrollAction.triggered)
        {
            float scrollValue = scrollAction.ReadValue<float>();
            ScrollHotbar(scrollValue);
        }
        
        if (useAction.triggered)
        {
            UseCurrentItem();
        }

        if (dropAction.triggered)
        {
            DropItem();
        }

        if (holsterAction.triggered)
        {
            HolsterCurrentItem();
        }

        if (hotbarSelectionAction.triggered)
        {
            SwitchItem(hotbarSelectionAction.ReadValue<int>());
        }
    }

    private void ScrollHotbar(float scrollValue)
    {
        int previousSlot = currentSlotIndex;
        if (scrollValue > 0)
        {
            currentSlotIndex--;
            if (currentSlotIndex < 0)
            {
                currentSlotIndex = hotbarSlots.Length - 1;
            }
            Debug.Log("Scrolled Up");
        }
        else if (scrollValue < 0)
        {
            currentSlotIndex++;
            if (currentSlotIndex >= hotbarSlots.Length)
            {
                currentSlotIndex = 0;
            }
            Debug.Log("Scrolled Down");
        }
        if (currentSlotIndex != previousSlot)
        {
            PullOutItem();
            if (currentlyHeldItem != null && hotbarSlots[currentSlotIndex] == null)
            {
                PutAwayItem();
            }
        }
        references.hotbarUiManager.UpdateUI(hotbarSlots, currentSlotIndex, currentlyHeldItem != null);
    }

    private void SwitchItem(int desiredSlot)
    {
        if (desiredSlot == currentSlotIndex) { return; }
        if (desiredSlot < 0 || desiredSlot >= hotbarSlots.Length) { return; }

        currentSlotIndex = desiredSlot;

        PullOutItem();
    }

    public void AddNewItem(HotbarItem item)
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarSlots[i] == null)
            {
                hotbarSlots[i] = item;
                PullOutItem(); //If the current slot just gained an item, this pulls it out
                references.hotbarUiManager.UpdateUI(hotbarSlots, currentSlotIndex, currentlyHeldItem != null);
                return;
            }
        }
        ReplaceItem(item);
    }

    private void ReplaceItem(HotbarItem newItem)
    {
        DropItem();
        hotbarSlots[currentSlotIndex] = newItem;
        PullOutItem();
        references.hotbarUiManager.UpdateUI(hotbarSlots, currentSlotIndex, currentlyHeldItem != null);
    }

    public void DropItem()
    {
        if (hotbarSlots[currentSlotIndex] == null) { return; }
        GameObject itemToDrop = hotbarSlots[currentSlotIndex].worldPrefab;
        Vector3 spawnPosition = references.playersCameraTransform.position + references.playersCameraTransform.forward * 0.5f;
        if (currentlyHeldItem != null)
        {
            PutAwayItem();
        }
        GameObject droppedItem = Instantiate(itemToDrop, spawnPosition, Quaternion.identity);
        hotbarSlots[currentSlotIndex] = null;
        if (droppedItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float torqueMultiplier = throwForce / 7f * 2f; 
            Vector3 throwDirection = references.playerHand.transform.forward + Vector3.up * 0.2f;
            rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);

            rb.AddTorque(Random.insideUnitSphere * torqueMultiplier, ForceMode.Impulse);
        }
        references.hotbarUiManager.UpdateUI(hotbarSlots, currentSlotIndex, currentlyHeldItem != null);
    }

    public void PullOutItem()
    {
        if (hotbarSlots[currentSlotIndex] == null) { return; }
        if (currentlyHeldItem != null) { PutAwayItem(); }
        GameObject itemToPullOut = hotbarSlots[currentSlotIndex].handPrefab;
        if (grabObject != null)
        {
            if (grabObject.IsHoldingObject())
            {
                grabObject.DropObject();
            }
            currentlyHeldItem = Instantiate(itemToPullOut, references.playerHand.transform);
        }
        references.hotbarUiManager.UpdateUI(hotbarSlots, currentSlotIndex, currentlyHeldItem != null);
    }

    public void PutAwayItem()
    {
        Destroy(currentlyHeldItem);
        currentlyHeldItem = null;
        references.hotbarUiManager.UpdateUI(hotbarSlots, currentSlotIndex, currentlyHeldItem != null);
    }

    private void HolsterCurrentItem()
    {
        if (currentlyHeldItem != null)
        {
            PutAwayItem();
        }
        else
        {
            PullOutItem();
        }
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
