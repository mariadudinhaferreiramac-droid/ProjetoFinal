using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance;

    [Header("Configurações")]
    public GameObject victoryPanel; // painel da vitória no Canvas
    public string menuSceneName = "Menu"; // nome da cena do menu inicial
    public bool pauseOnVictory = true; // pausa o jogo ao vencer

    private bool hasWon = false;

    private void Awake()
    {
        Instance = this;

        if (victoryPanel != null)
            victoryPanel.SetActive(false); // começa desativado
    }

    /// <summary>
    /// Chame este método quando o jogador vencer.
    /// </summary>
    public void TriggerVictory()
    {
        if (hasWon) return;
        hasWon = true;

        Debug.Log("🎉 Vitória alcançada!");

        // Salva a pontuação no ScoreManager
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SaveCurrentScore();
            Debug.Log("💾 Pontuação salva no ranking!");
        }

        // ✅ APAGA O SAVE DEFINITIVAMENTE AO VENCER
       SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager != null)
        {
            saveManager.DeleteSave();
        }  

        // Pausa o jogo
        if (pauseOnVictory)
            Time.timeScale = 0f;

        // Mostra o painel de vitória
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    /// <summary>
    /// Botão para voltar ao menu
    /// </summary>
    public void BackToMenu()
    {
        // Reseta o tempo
        Time.timeScale = 1f;

        // Reseta a pontuação atual (não o ranking)
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();

        // Carrega a cena do menu
        SceneManager.LoadScene(menuSceneName);
    }
}
