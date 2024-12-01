using UnityEngine;

public class ObjectVisibilityController : MonoBehaviour
{
    [Header("Settings")]
    public float changeProbability = 1f; // Pravdìpodobnost zmìny
    public float minTimeBetweenChanges = 1f; // Minimální èas mezi zmìnami
    public float maxTimeBetweenChanges = 5f; // Maximální èas mezi zmìnami

    [Header("References")]
    public FOVTrigger fovTrigger; // Reference na FOVTrigger skript (kužel)

    private bool hasBeenSeen = false;
    private bool isVisibleToPlayer = false;
    private float nextChangeTime = 0f;

    void Start()
    {
        if (fovTrigger == null)
        {
            Debug.LogError("Není pøiøazen FOVTrigger ke " + gameObject.name);
            return;
        }

        nextChangeTime = Time.time + Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
    }

    void Update()
    {
        if (fovTrigger == null)
            return;

        // Kontrola, zda je objekt v zorném poli
        isVisibleToPlayer = fovTrigger.objectsInFOV.Contains(gameObject);

        if (!hasBeenSeen && isVisibleToPlayer)
        {
            // Objekt byl vidìn hráèem
            hasBeenSeen = true;
        }

        if (hasBeenSeen)
        {
            if (!isVisibleToPlayer)
            {
                // Objekt je mimo zorné pole hráèe
                ChangeObject();
                hasBeenSeen = false; // Aby se zmìna neopakovala
            }
        }
    }

    void ChangeObject()
    {
        if (Time.time >= nextChangeTime)
        {
            if (Random.value <= changeProbability)
            {
                // Pøepneme aktivitu objektu
                gameObject.SetActive(!gameObject.activeSelf);
                // Pokud objekt zmizel, je tøeba resetovat stav
                if (!gameObject.activeSelf)
                {
                    hasBeenSeen = false;
                    isVisibleToPlayer = false;
                }
            }

            // Nastavíme èas pro další zmìnu
            nextChangeTime = Time.time + Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
        }
    }
}