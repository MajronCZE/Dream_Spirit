using UnityEngine;

public class SanityManager : MonoBehaviour
{
    [Header("Sanity Settings")]
    public float maxSanity = 100f; // Maximální sanity
    public float minSanity = 0f;   // Minimální sanity
    public float currentSanity = 100f; // Výchozí hodnota sanity

    [Header("Sanity Adjustment")]
    public float sanityIncreaseOnEnemy = 5f;   // Kolik sanity přidat za zásah Enemy
    public float sanityDecreaseOnEnemyGood = 5f; // Kolik sanity ubrat za zásah EnemyGood
    public float sanityDrainPerSecond = 1f;   // Kolik sanity se ubírá každou sekundu
    public float sanityLossOnTouch = 10f;     // Kolik sanity ubrat při doteku s Enemy
    public float sanityLossOnEnemyDestroyed = 15f; // Kolik sanity ubrat při zničení Enemy
    public float sanityLossOnFakeEnemyTouch = 8f; // Kolik sanity ubrat při doteku s FakeEnemy

    private bool isDead = false; // Kontrola, zda hráč "umřel"

    [Header("References")]
    public DeathScreenManager deathScreenManager; // Odkaz na DeathScreenManager

    void Start()
    {
        // Ujistit se, že sanity je v povoleném rozsahu
        currentSanity = Mathf.Clamp(currentSanity, minSanity, maxSanity);
    }

    void Update()
    {
        // Každou sekundu ubíráme sanity
        DrainSanityOverTime();

        // Kontrola, zda hráčova sanity klesla na nulu
        if (currentSanity <= minSanity && !isDead)
        {
            TriggerDeath();
        }
    }

    private void DrainSanityOverTime()
    {
        // Ujistěte se, že sanity nepadne pod minimum
        currentSanity -= sanityDrainPerSecond * Time.deltaTime;
        currentSanity = Mathf.Clamp(currentSanity, minSanity, maxSanity);
    }

    public void AdjustSanityOnHit(string tag)
    {
        if (tag == "Enemy")
        {
            // Zvýšení sanity za zásah Enemy
            currentSanity += sanityIncreaseOnEnemy;
        }
        else if (tag == "EnemyGood")
        {
            // Snížení sanity za zásah EnemyGood
            currentSanity -= sanityDecreaseOnEnemyGood;
        }
        else if (tag == "FakeEnemy")
        {
            // Zvýšení sanity za zničení FakeEnemy
            currentSanity += sanityIncreaseOnEnemy; // Stejné jako u Enemy
        }

        // Ujistěte se, že sanity zůstává v povoleném rozsahu
        currentSanity = Mathf.Clamp(currentSanity, minSanity, maxSanity);
    }

    public void NotifyEnemyDestroyed(string tag)
    {
        if (tag == "Enemy" || tag == "FakeEnemy")
        {
            // Snížení sanity při zničení nepřítele nebo FakeEnemy
            currentSanity -= sanityLossOnEnemyDestroyed;
            currentSanity = Mathf.Clamp(currentSanity, minSanity, maxSanity);

            Debug.Log($"{tag} destroyed! Sanity decreased by {sanityLossOnEnemyDestroyed}. Current sanity: {currentSanity}");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Pokud hráč detekuje kolizi s objektem s tagem "Enemy"
        if (collision.collider.CompareTag("Enemy"))
        {
            currentSanity -= sanityLossOnTouch;
            Debug.Log("Player touched by Enemy! Sanity decreased.");
        }
        // Pokud hráč detekuje kolizi s objektem s tagem "FakeEnemy"
        else if (collision.collider.CompareTag("FakeEnemy"))
        {
            currentSanity -= sanityLossOnFakeEnemyTouch;
            Debug.Log("Player touched by FakeEnemy! Sanity decreased.");
        }

        // Ujistěte se, že sanity zůstává v povoleném rozsahu
        currentSanity = Mathf.Clamp(currentSanity, minSanity, maxSanity);
    }

    private void TriggerDeath()
    {
        isDead = true;

        // Informujeme DeathScreenManager o tom, že hráč zemřel
        if (deathScreenManager != null)
        {
            deathScreenManager.ShowDeathScreen();
        }

        Debug.Log("Player sanity dropped to zero. Death screen triggered.");
    }
}