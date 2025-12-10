using UnityEngine;

public class BossAttackHitbox : MonoBehaviour
{
    public PlayerHealth ph;
    private Transform bossTransform;

    public int damage = 2;
    public float knockbackMultiplier = 2f;

    void Start()
    {
        bossTransform = GetComponentInParent<Transform>();

        if (ph == null)
            ph = FindObjectOfType<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerHurtbox")) return;

        if (ph != null)
        {
            Vector2 kb = (other.transform.position - bossTransform.position).normalized;
            kb *= knockbackMultiplier;

            ph.TakeDamage(damage, kb);
        }
        else
        {
            Debug.LogWarning("PlayerHealth não encontrado!");
        }
    }
}



