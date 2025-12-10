using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;

    public GameObject victoryPanel;

    public float damageDelay = 0.25f;
    private bool isDying = false;

    public int scoreOnDeath = 100; // << pontos do boss

    void Start()
    {
        currentHealth = maxHealth;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        if (isDying) return;

        StartCoroutine(DelayedDamage(amount));
    }

    private System.Collections.IEnumerator DelayedDamage(int amount)
    {
        yield return new WaitForSeconds(damageDelay);

        currentHealth -= amount;

        Debug.Log("Boss levou dano! Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDying = true;

        Debug.Log("Boss morreu!");

        // 💥 ADICIONA PONTOS ANTES DE PAUSAR O JOGO
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddPoints(scoreOnDeath);
            ScoreManager.Instance.SaveCurrentScore();
        }

        // Ativa painel de vitória
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // Pausa o jogo
        Time.timeScale = 0f;

        Destroy(gameObject);
    }
}
