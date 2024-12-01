using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player; // Reference na hráèe
    public float speed = 5f; // Rychlost pohybu nepøítele
    public float rotationSpeed = 10f; // Rychlost rotace smìrem k hráèi
    public float attackDistance = 1f; // Vzdálenost, ve které nepøítel "zaútoèí" (nebo je znièen)

    private SanityManager sanityManager; // Odkaz na SanityManager

    private void Start()
    {
        // Automaticky najít hráèe, pokud není pøiøazen v Inspectoru
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;

                // Najít SanityManager na hráèi
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
        // Pokud není hráè pøiøazen, nepokraèovat
        if (player == null) return;

        // Smìr k hráèi
        Vector3 direction = (player.position - transform.position).normalized;

        // Rotace smìrem k hráèi
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Pohyb smìrem k hráèi
        transform.position += direction * speed * Time.deltaTime;

        // Zkontrolovat, zda je nepøítel dost blízko na "útok" (zde se znièí)
        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        // Zpráva, že nepøítel zaútoèil na hráèe
        Debug.Log("Nepøítel zaútoèil na hráèe!");

        // Informovat SanityManager
        if (sanityManager != null)
        {
            sanityManager.NotifyEnemyDestroyed(); // Informuje o znièení nepøítele
        }

        // Znièit nepøítele
        Destroy(gameObject);

        // Informovat spawner, že byl nepøítel znièen
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.EnemyDestroyed();
        }
    }
}
