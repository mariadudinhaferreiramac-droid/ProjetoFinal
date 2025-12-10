using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance;

    public GameObject victoryPanel;
    public string menuSceneName = "Menu";
    public bool pauseOnVictory = true;

    private bool hasWon = false;

    private void Awake()
    {
        Instance = this;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void TriggerVictory()
    {
        if (hasWon) return;
        hasWon = true;

        Debug.Log("🎉 Vitória alcançada!");

        // Salva pontuação
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.SaveCurrentScore();

        if (pauseOnVictory)
            Time.timeScale = 0f;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    public void BackToMenu()
    {

          // Antes de trocar a cena, apagar o save
         SaveManager save = FindObjectOfType<SaveManager>();
         if (save != null)
          save.DeleteSave();

        Time.timeScale = 1f;
     

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();

        SceneManager.LoadScene(menuSceneName);

        
    }


}
