using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputSystem playerControls;

    private InputAction move;

    [SerializeField] private float moveSpeed = 5f;

    Vector2 movement;
    Vector2 mousePos;

    [SerializeField] private Rigidbody2D rb;

    public Camera cam;
    public Animator animator;
    
    private bool isRunning;
    private void Awake()
    {
        playerControls = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Update()
    {
        movement = move.ReadValue<Vector2>();

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        Animation();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if(Mathf.Abs(rb.velocity.x )>= 0.1f || Mathf.Abs(rb.velocity.y) >= 0.1f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void Animation()
    {
        animator.SetBool("isRunning?", isRunning);
    }
}
