using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        // Zumbi
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        
        if (enemy != null)
        {
            enemy.TakeDamage(1);
            return;
        }

        // Boss
        BossHealth boss = collision.GetComponent<BossHealth>();
        if (boss != null)
        {
            boss.TakeDamage(1);
        }
    }
}
