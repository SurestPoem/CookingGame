using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Input Actions")]
    public PlayerControls inputs;
    private InputAction lookAction;

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inputs = new PlayerControls();
        lookAction = inputs.PlayerCharacter.Look;
    }
    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        float lookX = lookAction.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        float lookY = lookAction.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * lookX);

        
    }
}
