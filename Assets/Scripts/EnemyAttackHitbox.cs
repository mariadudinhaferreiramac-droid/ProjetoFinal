using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    public PlayerHealth ph;
    private Transform enemyTransform;

    private void Start()
    {
        enemyTransform = GetComponentInParent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHurtbox"))
        {
            if (ph != null)
            {
                Vector2 knockbackDirection = (other.transform.position - enemyTransform.position).normalized;
                ph.TakeDamage(1, knockbackDirection);

            }
            else
            {
                Debug.LogWarning("PlayerHealth NÃO foi encontrado no pai da hurtbox!");
            }
        }
    }
}