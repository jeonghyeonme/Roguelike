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

        // 🎯 Player 태그로 타겟 자동 설정
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
    // 상태 전이 메서드
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
            // 추후 초기화 로직 삽입 가능
        }
    }

    void ExitState(EnemyState state)
    {
        if (state == EnemyState.Chase)
        {
            // 추후 종료 로직 삽입 가능
        }
    }

    // ========================
    // 상태 업데이트
    // ========================
    void UpdateIdleState()
    {
        if (target != null)
        {
            // ※ 추후 거리 조건 추가 가능
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
    // FixedUpdate 전용
    // ========================
    void FixedUpdateChaseState()
    {
        Move();
    }

    // ========================
    // 내부 메서드
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