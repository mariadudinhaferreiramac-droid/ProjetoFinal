using System.Collections;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public float speed = 2f;
    public float followStopRange = 0.5f;
    public float reactionDelay = 0.4f; // tempo pra começar a seguir

    private Rigidbody2D rb;
    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isStunned = false;
    private bool isFollowing = false;
    private bool waitingToFollow = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        // Ignora colisão com o player (mantém isso!)
        Collider2D dogCol = GetComponent<Collider2D>();
        Collider2D playerCol = target.GetComponent<Collider2D>();
        if (dogCol && playerCol)
            Physics2D.IgnoreCollision(dogCol, playerCol);
    }

    void Update()
    {
        // ATIVAR ATAQUE NO BOTÃO DIREITO DO MOUSE
        if (Input.GetMouseButtonDown(1)) // botão direito
        {
            animator.SetTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        if (target == null || isStunned) return;

        float distance = Vector2.Distance(transform.position, target.position);

        // se estiver longe o bastante, prepara pra seguir
        if (distance > followStopRange)
        {
            if (!isFollowing && !waitingToFollow)
                StartCoroutine(StartFollowingAfterDelay());
        }
        else
        {
            isFollowing = false;
            animator.SetBool("isWalk", false);
        }

        // movimento
        if (isFollowing)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            animator.SetBool("isWalk", true);
        }

        // vira o sprite pro lado certo
        spriteRenderer.flipX = target.position.x < transform.position.x;

        UpdateSortingOrder();
    }

    IEnumerator StartFollowingAfterDelay()
    {
        waitingToFollow = true;
        yield return new WaitForSeconds(reactionDelay);
        isFollowing = true;
        waitingToFollow = false;
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
}
