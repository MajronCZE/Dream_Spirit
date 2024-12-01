using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player; // Reference na hr��e
    public float speed = 5f; // Rychlost pohybu nep��tele
    public float rotationSpeed = 10f; // Rychlost rotace sm�rem k hr��i
    public float attackDistance = 1f; // Vzd�lenost, ve kter� nep��tel "za�to��" (nebo je zni�en)

    private SanityManager sanityManager; // Odkaz na SanityManager

    private void Start()
    {
        // Automaticky naj�t hr��e, pokud nen� p�i�azen v Inspectoru
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;

                // Naj�t SanityManager na hr��i
                sanityManager = playerObject.GetComponent<SanityManager>();
                if (sanityManager == null)
                {
                    Debug.LogError("SanityManager not found on Player. Please attach it to the Player GameObject.");
                }
            }
            else
            {
                Debug.LogError("Player not found! Make sure the player GameObject has the 'Player' tag.");
            }
        }
    }

    private void Update()
    {
        // Pokud nen� hr�� p�i�azen, nepokra�ovat
        if (player == null) return;

        // Sm�r k hr��i
        Vector3 direction = (player.position - transform.position).normalized;

        // Rotace sm�rem k hr��i
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Pohyb sm�rem k hr��i
        transform.position += direction * speed * Time.deltaTime;

        // Zkontrolovat, zda je nep��tel dost bl�zko na "�tok" (zde se zni��)
        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        // Zpr�va, �e nep��tel za�to�il na hr��e
        Debug.Log("Nep��tel za�to�il na hr��e!");

        // Informovat SanityManager
        if (sanityManager != null)
        {
            sanityManager.NotifyEnemyDestroyed(); // Informuje o zni�en� nep��tele
        }

        // Zni�it nep��tele
        Destroy(gameObject);

        // Informovat spawner, �e byl nep��tel zni�en
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.EnemyDestroyed();
        }
    }
}
