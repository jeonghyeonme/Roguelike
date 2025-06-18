using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState { Idle, Move, Dash }

    [Header("Camera")]
    public Camera mainCamera;

    [Header("Movement Config")]
    public float moveSpeed = 5f;

    [Header("Dash Config")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private PlayerState currentState;
    private float dashTimer;

    private float lastDashTime = -999f; // ✅ 대시 쿨타임 초기화
    public float LastDashTime => lastDashTime;

    private float inputBlockTimer = 0.3f; // ✅ 씬 시작 후 0.3초 동안 입력 무시

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trail;
    private DamageFeedback feedback;

    private Vector2 movement;
    private Vector2 lastMoveDirection;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();
        feedback = GetComponent<DamageFeedback>();

        movement = Vector2.zero;
        lastMoveDirection = Vector2.down;

        lastDashTime = Time.time; // ✅ 씬 시작 시 대시 쿨타임 방지
        ChangeState(PlayerState.Idle);
    }

    void Update()
    {
        if (isDead) return;
        if (feedback != null && feedback.isStunned) return;

        inputBlockTimer -= Time.deltaTime; // ✅ 입력 차단 타이머 감소

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
        if (isDead) return;
        if (feedback != null && feedback.isStunned) return;

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

    void UpdateIdleState()
    {
        HandleInput();
        UpdateDirection();
        UpdateAnimation();
        HandleSpriteFlip();

        if (inputBlockTimer <= 0f && Input.GetKeyDown(KeyCode.Space) && CanDash())
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

        if (inputBlockTimer <= 0f && Input.GetKeyDown(KeyCode.Space) && CanDash())
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

    void FixedUpdateMoveState()
    {
        if (movement == Vector2.zero) return;
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void FixedUpdateDashState()
    {
        rb.MovePosition(rb.position + lastMoveDirection * dashSpeed * Time.fixedDeltaTime);
    }

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
            AudioManager.Instance?.PlayDash(); // ✅ 진입 시점에서만 한 번 재생
            dashTimer = 0f;
            if (trail != null) trail.emitting = true;
        }
    }

    void ExitState(PlayerState state)
    {
        if (state == PlayerState.Dash)
        {
            if (trail != null) trail.emitting = false;
        }
    }

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
            lastMoveDirection = movement.normalized;
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
        if (mainCamera == null) return;

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        spriteRenderer.flipX = mouseWorld.x < transform.position.x;
    }

    bool CanDash()
    {
        return Time.time >= lastDashTime + dashCooldown;
    }

    public void OnDeath()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        movement = Vector2.zero;
        animator.SetFloat("MoveMagnitude", 0);

        if (trail != null)
            trail.emitting = false;
    }
}