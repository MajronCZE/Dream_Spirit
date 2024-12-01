using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeDisappearManager : MonoBehaviour
{
    [Header("Settings")]
    public string targetTag = "Eyes"; // Tag objekt�, kter� budou mizet
    public float disappearChance = 0.5f; // �ance, �e objekt zmiz� (0 = nikdy, 1 = v�dy)
    public float respawnChance = 0.3f; // �ance, �e objekt se objev� zp�t (0 = nikdy, 1 = v�dy)
    public float respawnCheckInterval = 5f; // Interval, jak �asto se kontroluje mo�nost respawnu (v sekund�ch)

    private List<GameObject> objectsInView = new List<GameObject>(); // Seznam objekt� aktu�ln� v zorn�m poli
    private HashSet<GameObject> disappearedObjects = new HashSet<GameObject>(); // Seznam objekt�, kter� u� zmizely

    private void Start()
    {
        // Spust�me Coroutine, kter� bude pravideln� kontrolovat mo�nost respawnu
        StartCoroutine(CheckRespawn());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Pokud objekt m� spr�vn� tag a je�t� nezmizel, p�id�me ho do seznamu
        if (other.CompareTag(targetTag) && !disappearedObjects.Contains(other.gameObject))
        {
            objectsInView.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Pokud objekt opust� zorn� pole, zpracujeme ho
        if (other.CompareTag(targetTag) && objectsInView.Contains(other.gameObject))
        {
            // N�hodn� rozhodneme, zda objekt zmiz�
            if (Random.value < disappearChance)
            {
                // Zmiz� objekt
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
        // Ujist�me se, �e objekty, kter� jsou st�le v zorn�m poli, z�st�vaj� aktivn� (jen pro jistotu)
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
            yield return new WaitForSeconds(respawnCheckInterval); // �ekejme nastaven� interval

            // Projdeme v�echny zmizel� objekty
            List<GameObject> objectsToRespawn = new List<GameObject>();
            foreach (GameObject obj in disappearedObjects)
            {
                // Pokud je objekt mimo zorn� pole a n�hodn� rozhodneme, �e se m� objevit zp�t
                if (!objectsInView.Contains(obj) && Random.value < respawnChance)
                {
                    objectsToRespawn.Add(obj);
                }
            }

            // Obnov�me vybran� objekty
            foreach (GameObject obj in objectsToRespawn)
            {
                if (obj != null)
                {
                    obj.SetActive(true); // Znovu aktivujeme objekt
                    disappearedObjects.Remove(obj); // Odebereme ho ze seznamu zmizel�ch
                    Debug.Log($"Object {obj.name} has respawned!");
                }
            }
        }
    }

    public void ResetDisappearedObjects()
    {
        // Voliteln� metoda pro resetov�n� zmizel�ch objekt� (nap�. pro testov�n� nebo nov� level)
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