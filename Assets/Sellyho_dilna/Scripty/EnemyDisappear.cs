using UnityEngine;

public class EnemyDisappear : MonoBehaviour
{
    [Header("Tags for enemies that can be destroyed")]
    public string[] destroyableTags = { "Enemy", "EnemyGood", "FakeEnemy" }; // P�id�n tag "FakeEnemy"

    void OnCollisionEnter(Collision collision)
    {
        // Zjist�me, zda objekt, kter� zas�hl, m� spr�vn� tag
        foreach (string tag in destroyableTags)
        {
            if (collision.collider.CompareTag(tag))
            {
                Destroy(gameObject); // Zni��me tento objekt
                return;
            }
        }
    }

    // Pro raycast z�sah
    public void TakeDamage()
    {
        Destroy(gameObject); // Zni��me tento objekt
    }
}
