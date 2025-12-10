using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    private void Awake()
    {
        // Se você veio de um pause/death menu, isso garante que nada fique "congelado"
        Time.timeScale = 1f;
    }

    public void Iniciar()
    {
        Time.timeScale = 1f; // garantia extra
        SceneManager.LoadScene(1);
    }

    public void Sair()
    {
        Application.Quit();
    }

    
}
