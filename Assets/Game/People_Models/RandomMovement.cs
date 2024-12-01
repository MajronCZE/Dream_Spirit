using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Movement Area")]
    public Vector2 areaSize = new Vector2(10, 10); // Velikost prostoru (���ka a d�lka na XZ ose)
    public Vector3 centerOffset = Vector3.zero; // Posun od st�edu GameObjectu, ke kter�mu je tento skript p�ipojen

    [Header("People Prefabs")]
    public List<GameObject> peoplePrefabs; // Seznam prefab� lid� (p�et�hn�te sem prefaby v Inspectoru)
    public int numberOfPeople = 5; // Po�et lid�, kter� chcete vytvo�it
    public float spawnInterval = 1f; // �asov� interval mezi spawnov�n�m jednotliv�ch lid� (v sekund�ch)

    [Header("Movement Settings")]
    public float minMoveTime = 2f; // Minim�ln� �as, kter� bude osoba tr�vit pohybem
    public float maxMoveTime = 5f; // Maxim�ln� �as pohybu
    public float movementSpeed = 2f; // Rychlost pohybu
    public float rotationSpeed = 5f; // Rychlost ot��en� sm�rem k c�li

    private List<GameObject> spawnedPeople = new List<GameObject>(); // Intern� seznam vytvo�en�ch lid�

    private void Start()
    {
        // Spus�te korutinu, kter� bude postupn� spawnovat lidi
        StartCoroutine(SpawnPeopleGradually());
    }

    private IEnumerator SpawnPeopleGradually()
    {
        for (int i = 0; i < numberOfPeople; i++)
        {
            // N�hodn� vyberte prefab ze seznamu
            GameObject prefab = peoplePrefabs[Random.Range(0, peoplePrefabs.Count)];

            // N�hodn� vyberte pozici v r�mci prostoru
            Vector3 randomPosition = GetRandomPositionInArea();

            // Vytvo�te postavu v prostoru
            GameObject person = Instantiate(prefab, randomPosition, Quaternion.identity);
            person.transform.parent = transform; // Voliteln�: nastavte rodi�e na tento objekt
            spawnedPeople.Add(person); // P�idejte vytvo�enou postavu do seznamu

            // Spus�te korutinu pro pohyb postavy
            StartCoroutine(MoveRandomly(person));

            // Po�kejte p�ed spawnov�n�m dal�� osoby
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPositionInArea()
    {
        // N�hodn� pozice v r�mci definovan�ho prostoru (XZ rovina)
        float randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        float randomZ = Random.Range(-areaSize.y / 2, areaSize.y / 2);

        // Zahrnut� offsetu a vr�cen� pozice
        return new Vector3(randomX, 0, randomZ) + transform.position + centerOffset;
    }

    private IEnumerator MoveRandomly(GameObject person)
    {
        while (true)
        {
            // Zvolte n�hodn� c�l v r�mci prostoru
            Vector3 targetPosition = GetRandomPositionInArea();

            // Pohyb k c�li
            while (Vector3.Distance(person.transform.position, targetPosition) > 0.1f)
            {
                // Sm��ov�n� postavy k c�li
                Vector3 direction = (targetPosition - person.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                person.transform.rotation = Quaternion.Slerp(person.transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                // Pohyb vp�ed
                person.transform.position = Vector3.MoveTowards(person.transform.position, targetPosition, movementSpeed * Time.deltaTime);

                yield return null; // �ekejte na dal�� frame
            }

            // Po�kejte n�hodn� �as p�ed dal��m pohybem
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