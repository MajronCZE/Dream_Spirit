using System.Collections.Generic;
using UnityEngine;

public class ClickEventManager : MonoBehaviour
{
    [Header("Assign objects to toggle in sequence")]
    public List<GameObject> objectsToToggle; // Seznam objekt� pro postupn� p�ep�n�n�

    [Header("Key to interact")]
    public KeyCode interactKey = KeyCode.E; // Kl�vesa pro interakci

    private int currentIndex = 0; // Index aktu�ln�ho objektu

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        // Zkontrolujeme, zda seznam nen� pr�zdn�
        if (objectsToToggle.Count == 0) return;

        // Vypneme aktu�ln� objekt
        if (objectsToToggle[currentIndex] != null)
        {
            objectsToToggle[currentIndex].SetActive(false);
        }

        // Posuneme se na dal�� objekt
        currentIndex = (currentIndex + 1) % objectsToToggle.Count;

        // Zapneme dal�� objekt
        if (objectsToToggle[currentIndex] != null)
        {
            objectsToToggle[currentIndex].SetActive(true);
        }
    }
}
