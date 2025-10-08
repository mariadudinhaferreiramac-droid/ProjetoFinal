using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 4;
    private int currentHealth;

    private Animator animator;
    private EnemyController enemyController;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Zumbi levou dano!");
        currentHealth -= damage;

        animator.SetTrigger("Hurt");
        if (enemyController != null)
        {
            enemyController.Stun(1f); // Zumbi fica parado por 1 segundo
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Zumbi morreu!");
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        if (enemyController != null)
        {
            enemyController.enabled = false; 
        }

        Destroy(gameObject, 3f); 
    }
}
