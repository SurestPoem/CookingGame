using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public PlayerControls inputs;
    private InputAction moveAction;
    [SerializeField]private CharacterController controller;
    [SerializeField]private float speed = 5f;
    private Vector3 velocity;
    [SerializeField]private float gravity = -9.81f;
    void Awake()
    {
        inputs = new PlayerControls();
        moveAction = inputs.PlayerCharacter.Move;
        controller = GetComponent<CharacterController>();
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
        float x = moveAction.ReadValue<Vector2>().x;
        float z = moveAction.ReadValue<Vector2>().y;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if(controller != null)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
