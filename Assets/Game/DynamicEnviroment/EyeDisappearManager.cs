using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDisappearManager : MonoBehaviour
{
    [Header("Settings")]
    public string targetTag = "Eyes"; // Tag objektù, které budou mizet
    public float disappearChance = 0.5f; // Šance, že objekt zmizí (0 = nikdy, 1 = vždy)
    public float respawnChance = 0.3f; // Šance, že objekt se objeví zpìt (0 = nikdy, 1 = vždy)
    public float respawnCheckInterval = 5f; // Interval, jak èasto se kontroluje možnost respawnu (v sekundách)

    private List<GameObject> objectsInView = new List<GameObject>(); // Seznam objektù aktuálnì v zorném poli
    private HashSet<GameObject> disappearedObjects = new HashSet<GameObject>(); // Seznam objektù, které už zmizely

    private void Start()
    {
        // Spustíme Coroutine, která bude pravidelnì kontrolovat možnost respawnu
        StartCoroutine(CheckRespawn());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Pokud objekt má správný tag a ještì nezmizel, pøidáme ho do seznamu
        if (other.CompareTag(targetTag) && !disappearedObjects.Contains(other.gameObject))
        {
            objectsInView.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Pokud objekt opustí zorné pole, zpracujeme ho
        if (other.CompareTag(targetTag) && objectsInView.Contains(other.gameObject))
        {
            // Náhodnì rozhodneme, zda objekt zmizí
            if (Random.value < disappearChance)
            {
                // Zmizí objekt
                other.gameObject.SetActive(false);
                disappearedObjects.Add(other.gameObject);
                Debug.Log($"Object {other.gameObject.name} has disappeared!");
            }

            // Odebereme objekt ze seznamu
            objectsInView.Remove(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Ujistíme se, že objekty, které jsou stále v zorném poli, zùstávají aktivní (jen pro jistotu)
        if (other.CompareTag(targetTag) && other.gameObject.activeSelf && !disappearedObjects.Contains(other.gameObject))
        {
            if (!objectsInView.Contains(other.gameObject))
            {
                objectsInView.Add(other.gameObject);
            }
        }
    }

    private IEnumerator CheckRespawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnCheckInterval); // Èekejme nastavený interval

            // Projdeme všechny zmizelé objekty
            List<GameObject> objectsToRespawn = new List<GameObject>();
            foreach (GameObject obj in disappearedObjects)
            {
                // Pokud je objekt mimo zorné pole a náhodnì rozhodneme, že se má objevit zpìt
                if (!objectsInView.Contains(obj) && Random.value < respawnChance)
                {
                    objectsToRespawn.Add(obj);
                }
            }

            // Obnovíme vybrané objekty
            foreach (GameObject obj in objectsToRespawn)
            {
                if (obj != null)
                {
                    obj.SetActive(true); // Znovu aktivujeme objekt
                    disappearedObjects.Remove(obj); // Odebereme ho ze seznamu zmizelých
                    Debug.Log($"Object {obj.name} has respawned!");
                }
            }
        }
    }

    public void ResetDisappearedObjects()
    {
        // Volitelná metoda pro resetování zmizelých objektù (napø. pro testování nebo nový level)
        foreach (GameObject obj in disappearedObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        disappearedObjects.Clear();
    }
}