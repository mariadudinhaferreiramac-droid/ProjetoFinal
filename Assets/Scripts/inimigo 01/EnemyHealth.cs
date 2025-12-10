using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 4;
    public int currentHealth;

    private Animator animator;
    private EnemyController enemyController;
    private Collider2D col;

    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        col = GetComponent<Collider2D>();

        // Se o inimigo já estava morto no LoadGame, garantir que continue morto.
        if (isDead)
        {
            ApplyDeadStateNoScore();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        animator.SetBool("Hurt", true);

        if (enemyController != null)
        {
            enemyController.Stun(1f);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        currentHealth = 0;

        if (animator != null)
     animator.SetBool("Die", true);


        if (col != null)
            col.enabled = false;

        if (enemyController != null)
            enemyController.enabled = false;

        // Pontos somente quando morre "ao vivo"
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddPoints(30);
    }

    // ===========================================================
    //  USADO PELO LOADGAME — restaura inimigo morto SEM DAR PONTOS
    // ===========================================================
    public void ApplyDeadStateNoScore()
    {
        // Forçar referências
        if (animator == null)
            animator = GetComponent<Animator>();

        if (enemyController == null)
            enemyController = GetComponent<EnemyController>();

        if (col == null)
            col = GetComponent<Collider2D>();

        // Aplicar estado morto
        isDead = true;
        currentHealth = 0;

        // Deixa na animação final de morte
        if (animator != null)
        {
          animator.SetBool("Die", true);
          animator.Update(0f); // força avaliação imediata (IMPORTANTE p/ load)
        }

        // Desativa tudo
        if (col != null)
            col.enabled = false;

        if (enemyController != null)
            enemyController.enabled = false;

        Debug.Log("Inimigo restaurado como morto (sem pontos): " + gameObject.name);
    }
}
