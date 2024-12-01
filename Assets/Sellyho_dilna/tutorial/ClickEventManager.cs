using System.Collections.Generic;
using UnityEngine;

public class ClickEventManager : MonoBehaviour
{
    [Header("Assign objects to toggle in sequence")]
    public List<GameObject> objectsToToggle; // Seznam objektù pro postupné pøepínání

    [Header("Key to interact")]
    public KeyCode interactKey = KeyCode.E; // Klávesa pro interakci

    private int currentIndex = 0; // Index aktuálního objektu

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        // Zkontrolujeme, zda seznam není prázdný
        if (objectsToToggle.Count == 0) return;

        // Vypneme aktuální objekt
        if (objectsToToggle[currentIndex] != null)
        {
            objectsToToggle[currentIndex].SetActive(false);
        }

        // Posuneme se na další objekt
        currentIndex = (currentIndex + 1) % objectsToToggle.Count;

        // Zapneme další objekt
        if (objectsToToggle[currentIndex] != null)
        {
            objectsToToggle[currentIndex].SetActive(true);
        }
    }
}
