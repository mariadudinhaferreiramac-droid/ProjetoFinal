using UnityEngine;
using System.Collections;

public class HealthHudController : MonoBehaviour
{
    public PlayerHealth playerHealth;
    [Tooltip("Tempo em segundos para a animação da barra de vida transicionar.")]
    public float transitionSpeed = 0.25f;

    private Animator healthAnimator;
    private int lastHealth;
    public string healthAnimationName = "Health";
    private int maxHealth = 8; // A vida máxima do player agora é 8

    void Start()
    {
        healthAnimator = GetComponent<Animator>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth nao foi atribuído no HealthHUDController.");
            return;
        }

        if (healthAnimator == null)
        {
            Debug.LogError("Animator nao encontrado no objeto do HealthHUDController.");
            return;
        }

        lastHealth = playerHealth.currentHealth;
        UpdateHUD(true);
    }

    void Update()
    {
        if (playerHealth.currentHealth != lastHealth)
        {
            UpdateHUD(false);
            lastHealth = playerHealth.currentHealth;
        }
    }

    void UpdateHUD(bool instant)
    {
        // O número de barras cheias é igual à vida atual
        int barsFilled = Mathf.Clamp(playerHealth.currentHealth, 0, maxHealth);

        // O tempo normalizado para o estado final (0.0 a 1.0)
        float targetNormalizedTime = 1f - ((float)barsFilled / (float)maxHealth);
        
        if (instant)
        {
            healthAnimator.Play(healthAnimationName, -1, targetNormalizedTime);
            healthAnimator.speed = 0f;
        }
        else
        {
            StartCoroutine(AnimateHealthBar(targetNormalizedTime));
        }
    }

    private IEnumerator AnimateHealthBar(float targetNormalizedTime)
    {
        float elapsedTime = 0f;
        float startNormalizedTime = healthAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        
        healthAnimator.speed = 1f;
        
        while (elapsedTime < transitionSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newNormalizedTime = Mathf.Lerp(startNormalizedTime, targetNormalizedTime, elapsedTime / transitionSpeed);
            
            healthAnimator.Play(healthAnimationName, -1, newNormalizedTime);
            
            yield return null;
        }

        healthAnimator.Play(healthAnimationName, -1, targetNormalizedTime);
        healthAnimator.speed = 0f;
    }
}