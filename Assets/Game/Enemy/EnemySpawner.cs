using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject enemyPrefab; // Prefab nep��tele
    public Transform player; // Reference na hr��e
    public float spawnInterval = 2f; // Interval mezi spawny nep��tel

    [Header("Enemy Settings")]
    public int maxEnemies = 10; // Maxim�ln� po�et nep��tel na map�
    private int currentEnemyCount = 0;

    [Header("Spawn Zone")]
    public BoxCollider spawnZone; // Box Collider ur�uj�c� z�nu spawnov�n�

    private void Start()
    {
        // Spou�t�me opakovan� spawnov�n� nep��tel
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    private void SpawnEnemy()
    {
        // Kontrola, zda je hr�� uvnit� z�ny
        if (!IsPlayerInsideZone())
        {
            Debug.Log("Player is outside the spawn zone. No enemies spawned.");
            return;
        }

        // Kontrola, zda nen� p�ekro�en maxim�ln� po�et nep��tel
        if (currentEnemyCount >= maxEnemies) return;

        // N�hodn� pozice uvnit� spawnovac� z�ny
        Vector3 spawnPosition = GetRandomPositionWithinZone();

        // Spawn nep��tele
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Zv��en� po�tu nep��tel
        currentEnemyCount++;
    }

    private Vector3 GetRandomPositionWithinZone()
    {
        if (spawnZone == null)
        {
            Debug.LogError("Spawn zone is not assigned! Please attach a BoxCollider to define the spawn area.");
            return Vector3.zero;
        }

        // Vygenerujeme n�hodnou pozici uvnit� Box Collideru
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

        // Kontrola, zda je hr�� uvnit� Box Collideru
        return spawnZone.bounds.Contains(player.position);
    }

    public void EnemyDestroyed()
    {
        // Sni�uje po�et nep��tel, kdy� je nep��tel zni�en
        currentEnemyCount--;
    }
}
