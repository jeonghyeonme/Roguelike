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

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
            else
                UnityEngine.Debug.LogWarning("[EnemyMovement] Player not found in scene!");
        }

        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (isDead) return;

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
        if (isDead) return;

        if (currentState == EnemyState.Chase)
            FixedUpdateChaseState();
    }

    void ChangeState(EnemyState newState)
    {
        ExitState(currentState);
        currentState = newState;
        EnterState(newState);
    }

    void EnterState(EnemyState state) { }
    void ExitState(EnemyState state) { }

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

    void FixedUpdateChaseState()
    {
        Move();
    }

    void UpdateMoveDirection()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        moveDirection = direction;

        if (moveDirection != Vector2.zero)
            lastMoveDirection = moveDirection;
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
            spriteRenderer.flipX = moveDirection.x < 0;
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    public void OnDeath()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        moveDirection = Vector2.zero;
        animator.SetFloat("MoveMagnitude", 0);
    }
}
