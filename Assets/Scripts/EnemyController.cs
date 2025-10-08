using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public float visionRange = 5f;
    public float attackRange = 0.5f;

    private Rigidbody2D rb;
    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float chaseTimer = 0f;
    public float chaseDuration = 3f;
    private bool isChasing = false;

    private bool isStunned = false;
    private bool isAttacking = false;

    public float attackCooldown = 1.2f;
    private float lastAttackTime = -999f;

    public EnemyAttackHitbox attackHitbox; // Arraste a hitbox para este campo no Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        
        // Se a hitbox não for atribuída no Inspector, tente encontrá-la automaticamente.
        if (attackHitbox == null)
        {
            attackHitbox = GetComponentInChildren<EnemyAttackHitbox>();
        }
    }

    void FixedUpdate()
    {
        if (target == null || isStunned || isAttacking) return;

        float distance = Vector2.Distance(transform.position, target.position);
        bool playerInVision = distance < visionRange;

        if (playerInVision)
        {
            isChasing = true;
            chaseTimer = 0f;
        }
        else if (isChasing)
        {
            chaseTimer += Time.fixedDeltaTime;
            if (chaseTimer > chaseDuration)
            {
                isChasing = false;
            }
        }

        if ((playerInVision || isChasing) && distance > attackRange)
        {
            Vector2 direction = (target.position - transform.position);

            if (direction.magnitude > 0.01f)
            {
                direction.Normalize();
                rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            }
        }
        else if (distance <= attackRange)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                StartCoroutine(Attack());
            }
        }

        UpdateSortingOrder();

        bool isWalk = (playerInVision || isChasing) && distance > attackRange;
        animator.SetBool("isWalk", isWalk);
        
        // Esta linha é crucial e estava faltando no seu código.
        // Ela vira o sprite do zumbi para a direção do jogador.
        spriteRenderer.flipX = target.position.x < transform.position.x;
    }

    void UpdateSortingOrder()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        animator.SetBool("isWalk", false);
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        animator.SetBool("isWalk", false);
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(0.5f);

        animator.ResetTrigger("Attack");
        isAttacking = false;
    }
}