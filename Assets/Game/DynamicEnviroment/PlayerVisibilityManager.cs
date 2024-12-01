using UnityEngine;
using System.Collections.Generic;

public class PlayerVisibilityManager : MonoBehaviour
{
    [Header("Settings")]
    public float changeProbability = 1f;
    public float minTimeBetweenChanges = 1f;
    public float maxTimeBetweenChanges = 5f;
    public FOVTrigger fovTrigger; // Reference na FOVTrigger skript

    [Header("Objects to Manage")]
    public GameObject[] objectsToManage;

    private Dictionary<GameObject, ObjectState> objectStates = new Dictionary<GameObject, ObjectState>();

    private class ObjectState
    {
        public bool hasBeenSeen = false;
        public bool isVisibleToPlayer = false;
        public float nextChangeTime = 0f;
    }

    void Start()
    {
        if (fovTrigger == null)
        {
            Debug.LogError("Není přiřazen FOVTrigger.");
            return;
        }

        foreach (GameObject obj in objectsToManage)
        {
            if (obj != null)
            {
                ObjectState state = new ObjectState();
                state.nextChangeTime = Time.time + Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
                objectStates.Add(obj, state);
            }
        }
    }

    void Update()
    {
        foreach (var item in objectStates)
        {
            GameObject obj = item.Key;
            ObjectState state = item.Value;

            if (obj == null)
                continue;

            if (!obj.activeSelf)
            {
                // Objekt je neaktivní, zkontrolujeme, zda je čas na jeho opětovnou aktivaci
                if (Time.time >= state.nextChangeTime)
                {
                    // Náhodně rozhodneme, zda objekt znovu aktivovat
                    if (Random.value <= changeProbability)
                    {
                        obj.SetActive(true);
                        // Obnovíme stav
                        state.hasBeenSeen = false;
                        state.isVisibleToPlayer = false;
                    }
                    // Nastavíme čas pro další změnu
                    state.nextChangeTime = Time.time + Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
                }
                continue; // Pokračujeme na další objekt
            }

            state.isVisibleToPlayer = fovTrigger.objectsInFOV.Contains(obj);

            if (!state.hasBeenSeen && state.isVisibleToPlayer)
            {
                // Objekt byl viděn hráčem
                state.hasBeenSeen = true;
            }

            if (state.hasBeenSeen)
            {
                if (!state.isVisibleToPlayer)
                {
                    // Objekt je mimo zorné pole hráče
                    ChangeObject(obj, state);
                    state.hasBeenSeen = false; // Aby se změna neopakovala
                }
            }
        }
    }

    void ChangeObject(GameObject obj, ObjectState state)
    {
        if (Time.time >= state.nextChangeTime)
        {
            if (Random.value <= changeProbability)
            {
                // Přepneme aktivitu objektu
                obj.SetActive(!obj.activeSelf);
                // Pokud objekt zmizel, je třeba resetovat stav
                if (!obj.activeSelf)
                {
                    state.hasBeenSeen = false;
                    state.isVisibleToPlayer = false;
                }
            }

            // Nastavíme čas pro další změnu
            state.nextChangeTime = Time.time + Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
        }
    }
}