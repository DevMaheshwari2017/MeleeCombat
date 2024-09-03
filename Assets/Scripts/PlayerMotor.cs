using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float gravity = -9.8f;
    [SerializeField]
    private float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    //Getter
    public Vector3 GetPlayerVelocity() => playerVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        isGrounded = controller.isGrounded;
    }

    //takes input from the inputmanager.cs and pass it to the character controller
    public void ProcessMovement(Vector2 input) 
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);
    }

    public void Jump() 
    {
        if (isGrounded) 
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * gravity * -3.0f);
        }
    }

}
