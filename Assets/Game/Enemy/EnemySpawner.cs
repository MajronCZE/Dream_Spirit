using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject enemyPrefab; // Prefab nepøítele
    public Transform player; // Reference na hráèe
    public float spawnInterval = 2f; // Interval mezi spawny nepøátel

    [Header("Enemy Settings")]
    public int maxEnemies = 10; // Maximální poèet nepøátel na mapì
    private int currentEnemyCount = 0;

    [Header("Spawn Zone")]
    public BoxCollider spawnZone; // Box Collider urèující zónu spawnování

    private void Start()
    {
        // Spouštíme opakované spawnování nepøátel
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    private void SpawnEnemy()
    {
        // Kontrola, zda je hráè uvnitø zóny
        if (!IsPlayerInsideZone())
        {
            Debug.Log("Player is outside the spawn zone. No enemies spawned.");
            return;
        }

        // Kontrola, zda není pøekroèen maximální poèet nepøátel
        if (currentEnemyCount >= maxEnemies) return;

        // Náhodná pozice uvnitø spawnovací zóny
        Vector3 spawnPosition = GetRandomPositionWithinZone();

        // Spawn nepøítele
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Zvýšení poètu nepøátel
        currentEnemyCount++;
    }

    private Vector3 GetRandomPositionWithinZone()
    {
        if (spawnZone == null)
        {
            Debug.LogError("Spawn zone is not assigned! Please attach a BoxCollider to define the spawn area.");
            return Vector3.zero;
        }

        // Vygenerujeme náhodnou pozici uvnitø Box Collideru
        Vector3 zoneCenter = spawnZone.bounds.center;
        Vector3 zoneSize = spawnZone.bounds.size;

        float x = Random.Range(zoneCenter.x - zoneSize.x / 2, zoneCenter.x + zoneSize.x / 2);
        float y = Random.Range(zoneCenter.y - zoneSize.y / 2, zoneCenter.y + zoneSize.y / 2);
        float z = Random.Range(zoneCenter.z - zoneSize.z / 2, zoneCenter.z + zoneSize.z / 2);

        return new Vector3(x, y, z);
    }

    private bool IsPlayerInsideZone()
    {
        if (spawnZone == null)
        {
            Debug.LogError("Spawn zone is not assigned! Please attach a BoxCollider to define the spawn area.");
            return false;
        }

        // Kontrola, zda je hráè uvnitø Box Collideru
        return spawnZone.bounds.Contains(player.position);
    }

    public void EnemyDestroyed()
    {
        // Snižuje poèet nepøátel, když je nepøítel znièen
        currentEnemyCount--;
    }
}
