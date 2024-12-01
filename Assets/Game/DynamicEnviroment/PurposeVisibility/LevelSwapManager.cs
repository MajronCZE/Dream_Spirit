using UnityEngine;

public class LevelSwapManager : MonoBehaviour
{
    [Header("Level Objects")]
    public GameObject originalLevel;    // P�vodn� ��st �rovn�, kter� zmiz�
    public GameObject newLevel;         // Nov� ��st �rovn�, kter� se objev�

    [Header("Settings")]
    public bool destroyOriginal = false; // Zda p�vodn� ��st �rovn� zni�it nebo jen deaktivovat

    private bool hasSwapped = false;    // Kontrola, zda ji� do�lo k v�m�n�

    void OnTriggerEnter(Collider other)
    {
        if (hasSwapped)
            return;

        // Zkontrolujeme, zda hr�� vstoupil do triggeru
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
                // Zni��me p�vodn� ��st �rovn�
                Destroy(originalLevel);
            }
            else
            {
                // Deaktivujeme p�vodn� ��st �rovn�
                originalLevel.SetActive(false);
            }
        }

        if (newLevel != null)
        {
            // Aktivujeme novou ��st �rovn�
            newLevel.SetActive(true);
        }
    }
}