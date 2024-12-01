using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuCanvas; // Odkaz na PauseMenuCanvas
    private bool isPaused = false;    // Stav hry (pauza nebo ne)

    void Start()
    {
        // Ujisti se, že PauseMenuCanvas je na zaèátku vypnutý
        pauseMenuCanvas.SetActive(false);
    }

    void Update()
    {
        // Kontrola, zda hráè stiskl klávesu Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        // Pokraèování ve høe
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f; // Obnov rychlost hry
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; // Uzamkni kurzor
        Cursor.visible = false; // Skryj kurzor
    }

    public void PauseGame()
    {
        // Pauza hry
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f; // Zastav èas
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; // Odemkni kurzor
        Cursor.visible = true; // Zobraz kurzor
    }

    public void LoadMainMenu()
    {
        // Obnov rychlost hry a naèti hlavní menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        // Obnov rychlost hry a restartuj aktuální scénu
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}