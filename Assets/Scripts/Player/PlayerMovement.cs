using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState { Idle, Move, Dash }

    [Header("Movement Config")]
    public float moveSpeed = 5f;

    [Header("Dash Config")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private PlayerState currentState;
    private float dashTimer;
    private float lastDashTime;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trail;

    private Vector2 movement;
    private Vector2 lastMoveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();

        ChangeState(PlayerState.Idle);
    }

    void Update()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                UpdateIdleState();
                break;
            case PlayerState.Move:
                UpdateMoveState();
                break;
            case PlayerState.Dash:
                UpdateDashState();
                break;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
            case PlayerState.Move:
                FixedUpdateMoveState();
                break;
            case PlayerState.Dash:
                FixedUpdateDashState();
                break;
        }
    }

    // ========================
    // Updates
    // ========================

    void UpdateIdleState()
    {
        HandleInput();
        UpdateDirection();
        UpdateAnimation();
        HandleSpriteFlip();

        if (Input.GetKeyDown(KeyCode.Space) && CanDash())
            ChangeState(PlayerState.Dash);
        else if (movement != Vector2.zero)
            ChangeState(PlayerState.Move);
    }

    void UpdateMoveState()
    {
        HandleInput();
        UpdateDirection();
        UpdateAnimation();
        HandleSpriteFlip();

        if (Input.GetKeyDown(KeyCode.Space) && CanDash())
            ChangeState(PlayerState.Dash);
        else if (movement == Vector2.zero)
            ChangeState(PlayerState.Idle);
    }

    void UpdateDashState()
    {
        dashTimer += Time.deltaTime;
        if (dashTimer >= dashDuration)
        {
            lastDashTime = Time.time;
            ChangeState(PlayerState.Idle);
        }
    }

    // ========================
    // FixedUpdate
    // ========================

    void FixedUpdateMoveState()
    {
        //UnityEngine.Debug.Log($"[FixedUpdate] moving with {movement}");
        if (movement == Vector2.zero) return;
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void FixedUpdateDashState()
    {
        rb.MovePosition(rb.position + lastMoveDirection * dashSpeed * Time.fixedDeltaTime);
    }

    // ========================
    // States
    // ========================

    void ChangeState(PlayerState newState)
    {
        ExitState(currentState);
        currentState = newState;
        EnterState(newState);
    }

    void EnterState(PlayerState state)
    {
        if (state == PlayerState.Dash)
        {
            dashTimer = 0f;
            trail.emitting = true;
        }
    }

    void ExitState(PlayerState state)
    {
        if (state == PlayerState.Dash)
        {
            trail.emitting = false;
        }
    }

    // ========================
    // Methods
    // ========================

    void HandleInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = new Vector2(movement.x, movement.y);


        if (movement.sqrMagnitude < 0.01f)
            movement = Vector2.zero;
    }

    void UpdateDirection()
    {
        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement.normalized;
        }
    }

    void UpdateAnimation()
    {
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("MoveMagnitude", movement.sqrMagnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void HandleSpriteFlip()
    {
        if (movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;
    }

    bool CanDash()
    {
        return Time.time >= lastDashTime + dashCooldown;
    }
}