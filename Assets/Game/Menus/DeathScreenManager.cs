using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [Header("Death Screen UI")]
    public GameObject deathScreen; // Odkaz na Death Screen panel

    void Start()
    {
        // Ujistìte se, že Death Screen je na zaèátku vypnutý
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }

    public void ShowDeathScreen()
    {
        // Aktivujeme Death Screen
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }

        // Zastavíme èas
        Time.timeScale = 0f;

        // Odemkneme a zobrazíme kurzor
        Cursor.lockState = CursorLockMode.None; // Odemknutí kurzoru
        Cursor.visible = true; // Zobrazení kurzoru

        Debug.Log("Death screen shown.");
    }

    public void LoadMainMenu()
    {
        // Obnovíme èas a naèteme hlavní menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void RestartGame()
    {
        // Obnovíme èas a restartujeme aktuální scénu
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}