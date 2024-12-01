using UnityEngine;

public class EnemyDisappear : MonoBehaviour
{
    [Header("Tags for enemies that can be destroyed")]
    public string[] destroyableTags = { "Enemy", "EnemyGood", "FakeEnemy" }; // Pøidán tag "FakeEnemy"

    void OnCollisionEnter(Collision collision)
    {
        // Zjistíme, zda objekt, který zasáhl, má správný tag
        foreach (string tag in destroyableTags)
        {
            if (collision.collider.CompareTag(tag))
            {
                Destroy(gameObject); // Znièíme tento objekt
                return;
            }
        }
    }

    // Pro raycast zásah
    public void TakeDamage()
    {
        Destroy(gameObject); // Znièíme tento objekt
    }
}
