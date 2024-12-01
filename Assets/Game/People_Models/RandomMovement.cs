using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Movement Area")]
    public Vector2 areaSize = new Vector2(10, 10); // Velikost prostoru (šíøka a délka na XZ ose)
    public Vector3 centerOffset = Vector3.zero; // Posun od støedu GameObjectu, ke kterému je tento skript pøipojen

    [Header("People Prefabs")]
    public List<GameObject> peoplePrefabs; // Seznam prefabù lidí (pøetáhnìte sem prefaby v Inspectoru)
    public int numberOfPeople = 5; // Poèet lidí, které chcete vytvoøit
    public float spawnInterval = 1f; // Èasovı interval mezi spawnováním jednotlivıch lidí (v sekundách)

    [Header("Movement Settings")]
    public float minMoveTime = 2f; // Minimální èas, kterı bude osoba trávit pohybem
    public float maxMoveTime = 5f; // Maximální èas pohybu
    public float movementSpeed = 2f; // Rychlost pohybu
    public float rotationSpeed = 5f; // Rychlost otáèení smìrem k cíli

    private List<GameObject> spawnedPeople = new List<GameObject>(); // Interní seznam vytvoøenıch lidí

    private void Start()
    {
        // Spuste korutinu, která bude postupnì spawnovat lidi
        StartCoroutine(SpawnPeopleGradually());
    }

    private IEnumerator SpawnPeopleGradually()
    {
        for (int i = 0; i < numberOfPeople; i++)
        {
            // Náhodnì vyberte prefab ze seznamu
            GameObject prefab = peoplePrefabs[Random.Range(0, peoplePrefabs.Count)];

            // Náhodnì vyberte pozici v rámci prostoru
            Vector3 randomPosition = GetRandomPositionInArea();

            // Vytvoøte postavu v prostoru
            GameObject person = Instantiate(prefab, randomPosition, Quaternion.identity);
            person.transform.parent = transform; // Volitelnì: nastavte rodièe na tento objekt
            spawnedPeople.Add(person); // Pøidejte vytvoøenou postavu do seznamu

            // Spuste korutinu pro pohyb postavy
            StartCoroutine(MoveRandomly(person));

            // Poèkejte pøed spawnováním další osoby
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPositionInArea()
    {
        // Náhodná pozice v rámci definovaného prostoru (XZ rovina)
        float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float randomZ = Random.Range(-areaSize.y / 2, areaSize.y / 2);

        // Zahrnutí offsetu a vrácení pozice
        return new Vector3(randomX, 0, randomZ) + transform.position + centerOffset;
    }

    private IEnumerator MoveRandomly(GameObject person)
    {
        while (true)
        {
            // Zvolte náhodnı cíl v rámci prostoru
            Vector3 targetPosition = GetRandomPositionInArea();

            // Pohyb k cíli
            while (Vector3.Distance(person.transform.position, targetPosition) > 0.1f)
            {
                // Smìøování postavy k cíli
                Vector3 direction = (targetPosition - person.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                person.transform.rotation = Quaternion.Slerp(person.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                // Pohyb vpøed
                person.transform.position = Vector3.MoveTowards(person.transform.position, targetPosition, movementSpeed * Time.deltaTime);

                yield return null; // Èekejte na další frame
            }

            // Poèkejte náhodnı èas pøed dalším pohybem
            yield return new WaitForSeconds(Random.Range(minMoveTime, maxMoveTime));
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Nakreslete hranice prostoru v editoru Unity
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + centerOffset, new Vector3(areaSize.x, 1, areaSize.y));
    }
}