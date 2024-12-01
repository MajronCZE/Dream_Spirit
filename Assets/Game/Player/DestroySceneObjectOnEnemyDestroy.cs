using UnityEngine;

public class DestroySceneObjectOnEnemyDestroy : MonoBehaviour
{
    [Header("Linked Scene Object to Destroy")]
    public GameObject linkedSceneObject; // Objekt ze sc�ny, kter� m� b�t zni�en

    private void OnDestroy()
    {
        // Zkontroluj, zda je propojen� objekt nastaven
        if (linkedSceneObject != null)
        {
            // Zni� propojen� objekt ve sc�n�
            Destroy(linkedSceneObject);
        }
    }
}