using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 8;
    public int currentHealth;

    private Animator anim;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer; 

    public float blinkDuration = 1f; 
    public float blinkInterval = 0.1f;

    private Rigidbody2D rb;
    private Collider2D[] allColliders;

    private void Awake() 
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        anim = GetComponent<Animator>(); 
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        allColliders = GetComponentsInChildren<Collider2D>();
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (currentHealth <= 0) return; // já morto

        currentHealth -= damage;
        if (anim != null)
        {
            anim.SetTrigger("Hurt");
        }

        Debug.Log("Player levou dano! Vida atual: " + currentHealth);

        if (playerController != null)
        {
            playerController.ApplyKnockback(knockbackDirection);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player morreu! Iniciando efeito de piscar...");
        StartCoroutine(BlinkBeforeDeath());
    }

    private IEnumerator BlinkBeforeDeath()
    {
        if (spriteRenderer != null)
        {
            float timer = 0f;
            while (timer < blinkDuration)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(blinkInterval);
                timer += blinkInterval;
            }
            spriteRenderer.enabled = true;
        }

        // Para música normal e toca música de morte
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayDeathMusic();
        }

        // Chama o menu antes de destruir
        if (DeathMenu.Instance != null)
        {
            DeathMenu.Instance.Show();
        }

        Destroy(gameObject);
    }
}
