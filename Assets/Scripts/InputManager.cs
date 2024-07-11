using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerController controller;

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
    void Awake()
    {
        controller = GetComponent<PlayerController>();
        look = GetComponent<PlayerLook>();
        motor = GetComponent<PlayerMotor>();
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        AssignInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (onFoot.Attack.IsPressed()) 
        {
            controller.Attack();
        }
    }
    private void FixedUpdate()
    {
        //tell playermotor to move using input from our movement action 
        motor.ProcessMovement(onFoot.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void AssignInputs() 
    {
        onFoot.Jump.performed += ctx => motor.jump();
        onFoot.Attack.started += ctx => controller.Attack();
    }

}
