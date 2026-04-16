using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public PlayerControls inputs;
    private InputAction interactAction;
    private PlayerReferences references;
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask interactLayerMask;
    
    private void Awake()
    {
        references = GetComponent<PlayerReferences>();
        inputs = new PlayerControls();
        interactAction = inputs.PlayerCharacter.Interact;
    }
    private void OnEnable()
    {
        inputs.PlayerCharacter.Enable();
    }

    private void OnDisable()
    {
        inputs.PlayerCharacter.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        GetInteractableObject();
    }

    private void GetInteractableObject()
    {
        if (Physics.Raycast(references.playersCameraTransform.position, references.playersCameraTransform.forward, out RaycastHit raycastHit, interactRange, interactLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                references.interactUI.Show(interactable.GetInteractionPrompt());
                if (interactAction.triggered)
                {
                    interactable.Interact(gameObject);
                }
            }
            else
            {
                references.interactUI.Hide();
            }
        }
        else
        {
            references.interactUI.Hide();
        }
    }

    private void GetCurrentDevice()
    {
        //Ignore for now
    }

    private void GetInteractBinding()
    {
        //Ignore for now
    }
}