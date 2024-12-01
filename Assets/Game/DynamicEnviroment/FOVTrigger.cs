using UnityEngine;
using System.Collections.Generic;

public class FOVTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string targetTag = "Interactable"; // Tag objektù, které chceme detekovat

    [HideInInspector]
    public HashSet<GameObject> objectsInFOV = new HashSet<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        // Pøidáme objekt do seznamu objektù v zorném poli
        if (other.CompareTag(targetTag))
        {
            objectsInFOV.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Odstraníme objekt ze seznamu objektù v zorném poli
        if (other.CompareTag(targetTag))
        {
            objectsInFOV.Remove(other.gameObject);
        }
    }
}