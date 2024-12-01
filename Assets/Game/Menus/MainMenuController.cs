using UnityEngine;
using UnityEngine.SceneManagement; // Pro pr�ci se sc�nami
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Odkaz na MainMenuCanvas
    public GameObject optionsCanvas; // Odkaz na OptionsCanvas
    public Slider volumeSlider;      // Odkaz na slider pro hlasitost

    void Start()
    {
        // Ujisti se, �e OptionsCanvas je vypnut� na za��tku
        optionsCanvas.SetActive(false);

        // Nastav v�choz� hodnotu hlasitosti (nap�. 50 %)
        AudioListener.volume = 0.5f;
        volumeSlider.value = 0.5f;

        // P�idej listener na slider pro zm�nu hlasitosti
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void StartGame()
    {
        // Na�ti hern� sc�nu
        SceneManager.LoadScene("KUBIKOVA_TAJNA_MISTNOST");
    }

    public void OpenOptions()
    {
        // Aktivuj OptionsCanvas a deaktivuj MainMenuCanvas
        optionsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    public void CloseOptions()
    {
        // Deaktivuj OptionsCanvas a aktivuj zp�t MainMenuCanvas
        optionsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        // Ukon�i hru (toto funguje pouze v buildu, ne v editoru)
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    private void SetVolume(float volume)
    {
        // Nastav hodnotu hlasitosti
        AudioListener.volume = volume;
        Debug.Log("Volume set to: " + volume);
    }
}