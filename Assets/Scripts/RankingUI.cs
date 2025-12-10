using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class RankingUI : MonoBehaviour
{


  [Header("Painéis do Canvas")]
    public GameObject PainelInicial;
    public GameObject painelRanking;


    [Header("Textos do Ranking")]
    public TextMeshProUGUI rankingText;

  private void Start()
    {
        MostrarMenu(); // começa no menu principal
    }
   


    public void VoltarpMenuD()
    {
     
       painelRanking.SetActive(false);
        PainelInicial.SetActive(true);
    }


    // === RANKING ===
    public void MostrarRanking()
    {
       PainelInicial.SetActive(false);
        painelRanking.SetActive(true);


        if (ScoreManager.Instance != null)
        {
            List<int> orderedScores = ScoreManager.Instance.GetScoresOrdered();

            string text = " Ranking Últimos Jogos \n\n";
            if (orderedScores.Count == 0)
            {
                text += "Nenhum jogo registrado ainda!";
            }
            else
            {
                for (int i = 0; i < orderedScores.Count; i++)
                {
                    text += (i + 1) + "º - " + orderedScores[i] + " pontos\n";
                }
            }

            rankingText.text = text;
        }
    }

       // === RESETAR O RANKING ===
    public void ResetRanking()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetAllRankings(); // apaga tudo
            MostrarRanking(); // atualiza o painel imediatamente
        }
    }



    public void MostrarMenu()
    {
       PainelInicial.SetActive(true);
        painelRanking.SetActive(false);
    
    }
}





