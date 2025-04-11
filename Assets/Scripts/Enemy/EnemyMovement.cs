using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyState { Idle, Chase }

    [Header("이동 설정")]
    public float moveSpeed = 3f;
    public Transform target;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    private EnemyState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                break;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Chase:
                FixedUpdateChaseState();
                break;
        }
    }

    // ========================
    // States
    // ========================
    void ChangeState(EnemyState newState)
    {
        ExitState(currentState);
        currentState = newState;
        EnterState(newState);
    }

    void EnterState(EnemyState state)
    {
        if (state == EnemyState.Chase)
        {
            // Good to use Later
        }
    }

    void ExitState(EnemyState state)
    {
        if (state == EnemyState.Chase)
        {
            // Good to use Later
        }
    }

    // ========================
    // Updates
    // ========================
    void UpdateIdleState()
    {
        if (target != null)
        {
            ChangeState(EnemyState.Chase);
        }
    }

    void UpdateChaseState()
    {
        if (target == null)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        UpdateMoveDirection();
        UpdateAnimation();
        HandleSpriteFlip();
    }

    // ========================
    // FixedUpdates
    // ========================
    void FixedUpdateChaseState()
    {
        Move();
    }

    // ========================
    // Methods
    // ========================
    void UpdateMoveDirection()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        moveDirection = direction;

        if (moveDirection != Vector2.zero)
        {
            lastMoveDirection = moveDirection;
        }
    }

    void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        animator.SetFloat("MoveMagnitude", moveDirection.sqrMagnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    void HandleSpriteFlip()
    {
        if (moveDirection.x != 0)
        {
            spriteRenderer.flipX = moveDirection.x < 0;
        }
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}