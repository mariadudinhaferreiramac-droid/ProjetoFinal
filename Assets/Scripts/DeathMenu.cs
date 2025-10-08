using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public static DeathMenu Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CanvasGroup overlay;  // CanvasGroup no DeathOverlay
    [SerializeField] private Image deadImage;      
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    [Header("Config")]
    [SerializeField] private float fadeDuration = 0.6f;
    [SerializeField] private string menuSceneName = ""; 

    private bool isShowing = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (overlay != null)
        {
            overlay.alpha = 0f;
            overlay.interactable = false;
            overlay.blocksRaycasts = false;
            overlay.gameObject.SetActive(false); // já começa desativado
        }
    }

    public void Show()
    {
        if (isShowing) return;
        isShowing = true;

        if (overlay != null)
        {
            overlay.gameObject.SetActive(true); // ativa painel visual
        }

        StartCoroutine(FadeInRoutine());
    }

    private System.Collections.IEnumerator FadeInRoutine()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        overlay.blocksRaycasts = true;
        overlay.interactable = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            overlay.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        overlay.alpha = 1f;

        restartButton?.Select();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(menuSceneName))
            SceneManager.LoadScene(menuSceneName);
        else
            SceneManager.LoadScene(0);
    }
}
