using UnityEngine;

public class ObjectVisibilityController : MonoBehaviour
{
    [Header("Settings")]
    public float changeProbability = 1f; // Pravd�podobnost zm�ny
    public float minTimeBetweenChanges = 1f; // Minim�ln� �as mezi zm�nami
    public float maxTimeBetweenChanges = 5f; // Maxim�ln� �as mezi zm�nami

    [Header("References")]
    public FOVTrigger fovTrigger; // Reference na FOVTrigger skript (ku�el)

    private bool hasBeenSeen = false;
    private bool isVisibleToPlayer = false;
    private float nextChangeTime = 0f;

    void Start()
    {
        if (fovTrigger == null)
        {
            Debug.LogError("Nen� p�i�azen FOVTrigger ke " + gameObject.name);
            return;
        }

        nextChangeTime = Time.time + Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
    }

    void Update()
    {
        if (fovTrigger == null)
            return;

        // Kontrola, zda je objekt v zorn�m poli
        isVisibleToPlayer = fovTrigger.objectsInFOV.Contains(gameObject);

        if (!hasBeenSeen && isVisibleToPlayer)
        {
            // Objekt byl vid�n hr��em
            hasBeenSeen = true;
        }

        if (hasBeenSeen)
        {
            if (!isVisibleToPlayer)
            {
                // Objekt je mimo zorn� pole hr��e
                ChangeObject();
                hasBeenSeen = false; // Aby se zm�na neopakovala
            }
        }
    }

    void ChangeObject()
    {
        if (Time.time >= nextChangeTime)
        {
            if (Random.value <= changeProbability)
            {
                // P�epneme aktivitu objektu
                gameObject.SetActive(!gameObject.activeSelf);
                // Pokud objekt zmizel, je t�eba resetovat stav
                if (!gameObject.activeSelf)
                {
                    hasBeenSeen = false;
                    isVisibleToPlayer = false;
                }
            }

            // Nastav�me �as pro dal�� zm�nu
            nextChangeTime = Time.time + Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
        }
    }
}