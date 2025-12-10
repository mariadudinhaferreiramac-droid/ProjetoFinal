using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Health : MonoBehaviour
{
    public int maxHealth = 2;   // vida reduzida
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

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddPoints(30);
    }

    public void ApplyDeadStateNoScore()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (enemyController == null)
            enemyController = GetComponent<EnemyController>();

        if (col == null)
            col = GetComponent<Collider2D>();

        isDead = true;
        currentHealth = 0;

        if (animator != null)
        {
            animator.SetBool("Die", true);
            animator.Update(0f);
        }

        if (col != null)
            col.enabled = false;

        if (enemyController != null)
            enemyController.enabled = false;

        Debug.Log("Inimigo restaurado como morto (sem pontos): " + gameObject.name);
    }
}
