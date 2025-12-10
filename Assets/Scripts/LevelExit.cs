using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    public string bossSceneName = "SceneBoss";

    [Header("Fade")]
    public CanvasGroup fadePanel;
    public float fadeTime = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
        float timer = 0;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            fadePanel.alpha = timer / fadeTime;
            yield return null;
        }

        SceneManager.LoadScene("SceneBoss");
    }
}
