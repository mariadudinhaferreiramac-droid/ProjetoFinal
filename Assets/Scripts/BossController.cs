using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 1.5f;
    public float visionRange = 8f;
    public float attackRange = 1.2f;

    public BossAttackHitbox attackHitbox;

    private Rigidbody2D rb;
    private Transform target;
    private SpriteRenderer sprite;
    private Animator animator;

    private bool isStunned = false;
    private bool isAttacking = false;

    public float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (attackHitbox == null)
            attackHitbox = GetComponentInChildren<BossAttackHitbox>();
    }

    void FixedUpdate()
    {
        if (target == null || isStunned || isAttacking) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > visionRange)
        {
            animator.SetBool("isWalk", false);
            return; // Boss parado longe
        }

        if (distance > attackRange)
            MoveTowardsPlayer();
        else
            TryAttack();

        sprite.flipX = target.position.x < transform.position.x;
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        animator.SetBool("isWalk", true);
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        animator.SetBool("isWalk", false);

        lastAttackTime = Time.time;

        yield return new WaitForSeconds(0.4f); // delay até o hit conectar

        // hitbox funciona separadamente

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
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
}
