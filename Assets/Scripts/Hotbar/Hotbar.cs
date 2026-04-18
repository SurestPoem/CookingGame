using UnityEngine;
using UnityEngine.InputSystem;

public class Hotbar : MonoBehaviour //Note - this script needs a full rewrite in the near future. It works fine for now, so don't use as an excuse to procrastinate the food assembly system for the millionth time 
{
    public PlayerControls inputs;
    private InputAction scrollAction;
    private InputAction useAction;
    private InputAction dropAction;
    private InputAction holsterAction;
    private InputAction hotbarSelectionAction;
    private int currentSlotIndex = 0;
    private int previousSlotIndex = 0;
    [Header("Hotbar Settings")]
    [SerializeField] private HotbarItem[] hotbarSlots = new HotbarItem[9];
    [SerializeField] private GameObject currentlyHeldItem;
    [SerializeField] private bool PreferCurrentSlot; //Important - as soon as a settings menu is added, tranfer this to that
    [Header("Item Drop Settings")]
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
        UpdateUI();
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
            int desiredSlotIndex = (int)hotbarSelectionAction.ReadValue<float>();
            SwitchItem(desiredSlotIndex);
        }
    }

    private void ScrollHotbar(float scrollValue)
    {
        int desiredSlotIndex;
        if (scrollValue > 0)
        {
            desiredSlotIndex = currentSlotIndex - 1;
            if (desiredSlotIndex < 0)
            {
                desiredSlotIndex = hotbarSlots.Length - 1;
            }
            Debug.Log("Scrolled Up");
        }
        else if (scrollValue < 0)
        {
            desiredSlotIndex = currentSlotIndex + 1;
            if (desiredSlotIndex >= hotbarSlots.Length)
            {
                desiredSlotIndex = 0;
            }
            Debug.Log("Scrolled Down");
        }
        else
        {
            return;
        }
        SwitchItem(desiredSlotIndex);
    }

    private void SwitchItem(int desiredSlot)
    {
        if (desiredSlot == currentSlotIndex) { return; }
        if (desiredSlot < 0 || desiredSlot >= hotbarSlots.Length) { return; }
        previousSlotIndex = currentSlotIndex;
        currentSlotIndex = desiredSlot;

        if (currentlyHeldItem != null && hotbarSlots[currentSlotIndex] == null)
        {
            PutAwayItem();
        }
        PullOutItem();
        UpdateUI();
    }

    public void AddNewItem(HotbarItem item)
    {
        if (PreferCurrentSlot)
        {
            if (hotbarSlots[currentSlotIndex] == null)
            {
                hotbarSlots[currentSlotIndex] = item;
                PullOutItem();
                UpdateUI();
                return;
            }
        }
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarSlots[i] == null)
            {
                hotbarSlots[i] = item;
                PullOutItem(); //If the current slot just gained an item, this pulls it out
                UpdateUI();
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
        UpdateUI();
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
        UpdateUI();
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
        UpdateUI();
    }

    public void PutAwayItem()
    {
        Destroy(currentlyHeldItem);
        currentlyHeldItem = null;
        UpdateUI();
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

    private void UpdateUI()
    {
        references.hotbarUiManager.UpdateUI(hotbarSlots, currentSlotIndex, previousSlotIndex, currentlyHeldItem != null);
    }
}
