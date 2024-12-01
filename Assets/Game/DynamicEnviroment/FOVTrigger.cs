using UnityEngine;
using System.Collections.Generic;

public class FOVTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string targetTag = "Interactable"; // Tag objekt�, kter� chceme detekovat

    [HideInInspector]
    public HashSet<GameObject> objectsInFOV = new HashSet<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        // P�id�me objekt do seznamu objekt� v zorn�m poli
        if (other.CompareTag(targetTag))
        {
            objectsInFOV.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Odstran�me objekt ze seznamu objekt� v zorn�m poli
        if (other.CompareTag(targetTag))
        {
            objectsInFOV.Remove(other.gameObject);
        }
    }
}