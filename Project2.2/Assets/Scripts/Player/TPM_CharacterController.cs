using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TPM_CharacterController : MonoBehaviour {
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius;
    private bool isGrounded;

    [Header("Input")]
    private Vector3 moveRawInput;
    private Vector2 mouseRawInput;
    private float xRotation;
    private float yRotation;
    private Vector3 moveVelocity;

    [Header("Player Movement")]
    private float yAxisVelocity;
    [SerializeField] private float mouseSensitivity, mouseYMax, mouseYMin;
    [SerializeField] private float regularSpeed, sprintSpeed;
    [SerializeField] private float gravity = -9.8f;
    private float moveSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;

    [Header("Misc")]
    private Mouse mouse;
    private Camera cam;
    private CharacterController characterController;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        mouse = Mouse.current;

        cam = Camera.main;
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;

        moveSpeed = regularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        yAxisVelocity += gravity * Time.deltaTime;

        moveVelocity = transform.TransformDirection(moveRawInput) * moveSpeed;
        moveVelocity += new Vector3(0, yAxisVelocity, 0);
        characterController.Move(moveVelocity * Time.deltaTime);
        CursorCheck();
    }

    private void CursorCheck() {
        if (mouse.rightButton.wasPressedThisFrame) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void LookEvent(InputAction.CallbackContext value)
    {
        mouseRawInput = value.ReadValue<Vector2>();

        xRotation -= mouseRawInput.y * mouseSensitivity * Time.deltaTime;
        yRotation += mouseRawInput.x * mouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, mouseYMin, mouseYMax);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void MoveEvent(InputAction.CallbackContext value)
    {
        moveRawInput = new Vector3(value.ReadValue<Vector2>().x, 0, value.ReadValue<Vector2>().y);

        anim.SetBool("WalkFront", moveRawInput.z > 0);
        anim.SetBool("WalkBack", moveRawInput.z < 0);
        anim.SetBool("WalkLeft", moveRawInput.x < 0);
        anim.SetBool("WalkRight", moveRawInput.x > 0);
    }

    public void JumpEvent()
    {
        if (isGrounded) {
            yAxisVelocity = jumpForce;
            anim.SetTrigger("Jump");
        }
    }

    public void SprintEvent(InputAction.CallbackContext value)
    {
        if (value.started) {
            moveSpeed = sprintSpeed;
        } else if (value.canceled) {
            moveSpeed = regularSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Axe") || other.CompareTag("Turtle") || other.CompareTag("DeathZone")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadius, groundMask);
    }

    // public void TakeKnockback(Vector3 knockbackDirection, float knockbackSpeed, float knockbackDuration)
    // {
    //     moveVelocity = knockbackDirection * knockbackSpeed;
    //     knockbackTimer = knockbackDuration;
    //     isKnockbackApplied = true;
    // }
}
