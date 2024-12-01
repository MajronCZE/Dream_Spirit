using UnityEngine;
using UnityEngine.SceneManagement; // Pro práci se scénami
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Odkaz na MainMenuCanvas
    public GameObject optionsCanvas; // Odkaz na OptionsCanvas
    public Slider volumeSlider;      // Odkaz na slider pro hlasitost

    void Start()
    {
        // Ujisti se, že OptionsCanvas je vypnutý na zaèátku
        optionsCanvas.SetActive(false);

        // Nastav výchozí hodnotu hlasitosti (napø. 50 %)
        AudioListener.volume = 0.5f;
        volumeSlider.value = 0.5f;

        // Pøidej listener na slider pro zmìnu hlasitosti
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void StartGame()
    {
        // Naèti herní scénu
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
        // Deaktivuj OptionsCanvas a aktivuj zpìt MainMenuCanvas
        optionsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        // Ukonèi hru (toto funguje pouze v buildu, ne v editoru)
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