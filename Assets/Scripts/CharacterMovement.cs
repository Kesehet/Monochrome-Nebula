using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 5.0f; // Movement speed
    [SerializeField] private float acceleration = 10.0f; // Acceleration
    [SerializeField] private float maxVelocity = 5.0f; // Maximum velocity
    [SerializeField] private float damping = 0.9f; // Damping factor to slow down the player

    [Header("Dash Parameters")]
    [SerializeField] private float dashSpeed = 10.0f; // Dash speed
    [SerializeField] private float dashDuration = 0.2f; // Dash duration
    [SerializeField] private float dashCooldown = 1.0f; // Dash cooldown

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float lastDash = -100.0f; // Last dash time

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Get horizontal input (A/D or Left/Right arrow)
        float verticalInput = Input.GetAxis("Vertical"); // Get vertical input (W/S or Up/Down arrow)

        Vector2 movement = new Vector2(horizontalInput, verticalInput); // Create movement vector

        // Normalize the movement vector if it exceeds 1 in combined length:
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }

        // Apply force to the Rigidbody2D:
        rb.AddForce(movement * acceleration, ForceMode2D.Force);

        // Clamp the velocity to the maximum velocity:
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        // Apply damping if no input is detected and set walking to false
        if (movement.sqrMagnitude == 0)
        {
            rb.velocity *= damping;
            animator.SetBool("walking", false);
        }
        // If the player is moving left or right
        else{
            animator.SetBool("walking", true);
        }

        // Rotate the transform based on the direction of movement:
        if (horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Moving right
        }
        else if (horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Moving left
        }

        // Update the walk_speed parameter in the Animator:
        float absoluteSpeed = rb.velocity.magnitude;
        animator.SetFloat("walk_speed", absoluteSpeed);

        // Dash functionality:
        if (Input.GetKey(KeyCode.Space) && Time.time > lastDash + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        float dashEndTime = Time.time + dashDuration;

        // Get the direction of the dash from the input
        Vector2 dashDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Normalize the dash direction
        if (dashDirection.sqrMagnitude > 1)
        {
            dashDirection.Normalize();
        }

        while (Time.time < dashEndTime)
        {
            // Set the velocity for the dash
            rb.velocity = dashSpeed * dashDirection;
            yield return null;
        }

        lastDash = Time.time;
    }



}
