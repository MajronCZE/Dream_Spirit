using UnityEngine;

public class DestroySceneObjectOnEnemyDestroy : MonoBehaviour
{
    [Header("Linked Scene Object to Destroy")]
    public GameObject linkedSceneObject; // Objekt ze scény, který má být znièen

    private void OnDestroy()
    {
        // Zkontroluj, zda je propojený objekt nastaven
        if (linkedSceneObject != null)
        {
            // Zniè propojený objekt ve scénì
            Destroy(linkedSceneObject);
        }
    }
}