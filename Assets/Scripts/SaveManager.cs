using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySave
    {
        public float x;
        public float y;
        public bool isDead;
    }

    [System.Serializable]
    public class SaveData
    {
        public string sceneName;
        public float playerX;
        public float playerY;

        public List<EnemySave> enemies = new List<EnemySave>();
    }

    private string savePath;

    // Guardamos o listener para poder remover depois.
    private UnityEngine.Events.UnityAction<Scene, LoadSceneMode> sceneLoadCallback;


    private void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }

    // ============================
    // SALVAR O JOGO
    // ============================
    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        SaveData data = new SaveData();
        data.sceneName = SceneManager.GetActiveScene().name;
        data.playerX = player.transform.position.x;
        data.playerY = player.transform.position.y;

        // Salvar inimigos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject e in enemies)
        {
            EnemyHealth eh = e.GetComponent<EnemyHealth>();
            if (eh == null) continue;

            EnemySave save = new EnemySave();
            save.x = e.transform.position.x;
            save.y = e.transform.position.y;
            save.isDead = eh.isDead;

            data.enemies.Add(save);
        }

        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
        Debug.Log("💾 Jogo salvo incluindo inimigos!");
    }

    // ============================
    // CARREGAR O JOGO
    // ============================
    public void LoadGame()
    {
        if (!File.Exists(Application.persistentDataPath + "/save.json"))
{
    Debug.Log("❌ Nenhum save disponível");
    return;
}


        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));

        Debug.Log("📥 Carregando cena salva: " + data.sceneName);

        // Remove listeners antigos para evitar duplicação
        if (sceneLoadCallback != null)
            SceneManager.sceneLoaded -= sceneLoadCallback;

        // Cria callback novo
        sceneLoadCallback = (scene, mode) =>
        {
            SceneManager.sceneLoaded -= sceneLoadCallback;

            // Player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector2(data.playerX, data.playerY);
            }

            // Inimigos
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            int count = Mathf.Min(enemies.Length, data.enemies.Count);

            for (int i = 0; i < count; i++)
            {
                GameObject enemy = enemies[i];
                EnemyHealth eh = enemy.GetComponent<EnemyHealth>();

                enemy.transform.position = new Vector2(
                    data.enemies[i].x,
                    data.enemies[i].y
                );

                if (data.enemies[i].isDead && eh != null)
                {
                    eh.ApplyDeadStateNoScore(); // agora sem erro
                }
            }

            Debug.Log("✔ Jogo carregado com inimigos restaurados!");
        };

        SceneManager.sceneLoaded += sceneLoadCallback;
        SceneManager.LoadScene(data.sceneName);
    }

    // ============================
    // SALVAR E VOLTAR AO MENU
    // ============================
    public void SaveAndReturnToMenu(string menuScene)
    {
        SaveGame();
        SceneManager.LoadScene(menuScene);
    }

    // ============================
    // REINICIAR FASE
    // ============================
    public void RestartLevel()
    {
        Time.timeScale = 1f;

        // Remove listeners velhos pra evitar bug de load repetido
        if (sceneLoadCallback != null)
            SceneManager.sceneLoaded -= sceneLoadCallback;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ============================
// APAGAR SAVE (AO VENCER O JOGO)
// ============================
public void DeleteSave()
{
    string path = Application.persistentDataPath + "/save.json";

    if (File.Exists(path))
    {
        File.Delete(path);
        Debug.Log("🗑️ Save apagado após vitória!");
    }
}

}