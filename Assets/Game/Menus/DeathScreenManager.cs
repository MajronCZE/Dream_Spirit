using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [Header("Death Screen UI")]
    public GameObject deathScreen; // Odkaz na Death Screen panel

    void Start()
    {
        // Ujist�te se, �e Death Screen je na za��tku vypnut�
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

        // Zastav�me �as
        Time.timeScale = 0f;

        // Odemkneme a zobraz�me kurzor
        Cursor.lockState = CursorLockMode.None; // Odemknut� kurzoru
        Cursor.visible = true; // Zobrazen� kurzoru

        Debug.Log("Death screen shown.");
    }

    public void LoadMainMenu()
    {
        // Obnov�me �as a na�teme hlavn� menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void RestartGame()
    {
        // Obnov�me �as a restartujeme aktu�ln� sc�nu
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}