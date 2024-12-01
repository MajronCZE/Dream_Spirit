using UnityEngine;

public class LevelSwapManager : MonoBehaviour
{
    [Header("Level Objects")]
    public GameObject originalLevel;    // Pùvodní èást úrovnì, která zmizí
    public GameObject newLevel;         // Nová èást úrovnì, která se objeví

    [Header("Settings")]
    public bool destroyOriginal = false; // Zda pùvodní èást úrovnì znièit nebo jen deaktivovat

    private bool hasSwapped = false;    // Kontrola, zda již došlo k výmìnì

    void OnTriggerEnter(Collider other)
    {
        if (hasSwapped)
            return;

        // Zkontrolujeme, zda hráè vstoupil do triggeru
        if (other.CompareTag("Player"))
        {
            SwapLevels();
            hasSwapped = true;
        }
    }

    void SwapLevels()
    {
        if (originalLevel != null)
        {
            if (destroyOriginal)
            {
                // Znièíme pùvodní èást úrovnì
                Destroy(originalLevel);
            }
            else
            {
                // Deaktivujeme pùvodní èást úrovnì
                originalLevel.SetActive(false);
            }
        }

        if (newLevel != null)
        {
            // Aktivujeme novou èást úrovnì
            newLevel.SetActive(true);
        }
    }
}