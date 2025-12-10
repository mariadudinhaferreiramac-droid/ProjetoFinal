using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int currentScore = 0;
    private List<int> lastScores = new List<int>();
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // mantém entre cenas
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 🔹 TESTE DE VITÓRIA (pressione V)
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("🔹 Vitória simulada! Salvando pontuação...");

            // Adiciona pontos aleatórios só pra testar
            AddPoints(Random.Range(50, 200));

            SaveCurrentScore();

            // Pausa o jogo como se tivesse vencido
            Time.timeScale = 0f;

            Debug.Log("✅ Pontuação salva e jogo pausado!");
            Debug.Log("Ranking atual (top 5):");

            var scores = GetScoresOrdered();
            for (int i = 0; i < scores.Count; i++)
            {
                Debug.Log($"{i + 1}º - {scores[i]} pontos");
            }
        }

        // 🔹 TESTE DE REINÍCIO (pressione R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScore();
            Time.timeScale = 1f;
            Debug.Log("🔁 Reinício do teste: pontuação zerada e jogo retomado.");
        }
    }

    // Adiciona pontos (zumbi morto, etc)
    public void AddPoints(int amount)
    {
        currentScore += amount;
        Debug.Log("Pontuação atual: " + currentScore);
    }

    // Remove pontos (dano recebido, etc)
    public void RemovePoints(int amount)
    {
        currentScore -= amount;
        if (currentScore < 0) currentScore = 0;
        Debug.Log("Pontuação atual: " + currentScore);
    }

    // Salva a pontuação no final do jogo
    public void SaveCurrentScore()
    {
        if (currentScore <= 0)
        {
            Debug.Log("⚠ Nenhuma pontuação para salvar.");
            return;
        }

        // Adiciona a pontuação atual à lista
        lastScores.Add(currentScore);

        // Ordena do maior pro menor
        lastScores.Sort((a, b) => b.CompareTo(a));

        // Mantém só os 5 melhores
        if (lastScores.Count > 5)
            lastScores.RemoveAt(lastScores.Count - 1);

        // Salva no PlayerPrefs
        for (int i = 0; i < lastScores.Count; i++)
        {
            PlayerPrefs.SetInt("Score_" + i, lastScores[i]);
        }
        PlayerPrefs.SetInt("ScoreCount", lastScores.Count);
        PlayerPrefs.Save();

        Debug.Log("💾 Pontuação salva: " + currentScore);

        // ✅ Zera a pontuação para o próximo jogo
        currentScore = 0;
    }

    // Carrega os últimos scores salvos
    private void LoadScores()
    {
        lastScores.Clear();
        int count = PlayerPrefs.GetInt("ScoreCount", 0);
        for (int i = 0; i < count; i++)
        {
            lastScores.Add(PlayerPrefs.GetInt("Score_" + i, 0));
        }
    }

    // Retorna a lista ordenada (maior → menor)
    public List<int> GetScoresOrdered()
    {
        List<int> ordered = new List<int>(lastScores);
        ordered.Sort((a, b) => b.CompareTo(a));
        return ordered;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void ResetScore()
    {
        currentScore = 0;
    }

 public void ResetAllRankings()
{
    int count = PlayerPrefs.GetInt("ScoreCount", 0);
    for (int i = 0; i < count; i++)
        PlayerPrefs.DeleteKey("Score_" + i);

    PlayerPrefs.DeleteKey("ScoreCount");
    PlayerPrefs.Save();

    lastScores.Clear();
    Debug.Log("🗑 Todos os rankings foram resetados!");
}


}
