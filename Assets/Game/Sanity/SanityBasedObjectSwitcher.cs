using UnityEngine;

public class SanityBasedObjectSwitcher : MonoBehaviour
{
    [Header("References")]
    public SanityManager sanityManager; // Reference na SanityManager
    public GameObject[] sanityObjects; // Pole objektù pro rùzné sanity sektory

    [Header("Sanity Thresholds")]
    public float sanityStep = 10f; // Velikost sektoru sanity (napø. 10 sanity na sektor)

    private int currentActiveIndex = -1; // Index aktuálnì aktivního objektu

    private void Start()
    {
        // Kontrola, zda jsou sanity objekty nastaveny
        if (sanityManager == null)
        {
            Debug.LogError("SanityManager is not assigned! Please assign it in the inspector.");
        }

        if (sanityObjects == null || sanityObjects.Length == 0)
        {
            Debug.LogError("No objects assigned to SanityObjects array! Please assign them in the inspector.");
        }

        // Vypnout všechny objekty na zaèátku
        DisableAllObjects();
    }

    private void Update()
    {
        // Kontrola sanity a aktualizace objektù
        UpdateActiveObjectBasedOnSanity();
    }

    private void UpdateActiveObjectBasedOnSanity()
    {
        if (sanityManager == null || sanityObjects == null || sanityObjects.Length == 0)
            return;

        // Vypoèítat index sektoru podle sanity
        int sectorIndex = Mathf.FloorToInt(sanityManager.currentSanity / sanityStep);

        // Omezit index na rozsah sanityObjects
        sectorIndex = Mathf.Clamp(sectorIndex, 0, sanityObjects.Length - 1);

        // Pokud už je aktivní správný objekt, nic nemìnit
        if (currentActiveIndex == sectorIndex) return;

        // Aktualizovat objekty
        DisableAllObjects();
        sanityObjects[sectorIndex].SetActive(true);
        currentActiveIndex = sectorIndex;

        Debug.Log($"Sanity changed to sector {sectorIndex}. Activated object: {sanityObjects[sectorIndex].name}");
    }

    private void DisableAllObjects()
    {
        foreach (GameObject obj in sanityObjects)
        {
            obj.SetActive(false);
        }
    }
}
