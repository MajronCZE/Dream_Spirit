using UnityEngine;

public class InstantDeathZone : MonoBehaviour
{
    public DeathScreenManager deathScreenManager; // Odkaz na DeathScreenManager

    private void OnTriggerEnter(Collider other)
    {
        // Kontrolujeme, zda objekt, kter� vstoupil do z�ny, m� tag "Player"
        if (other.CompareTag("Player"))
        {
            // Spust�me Death Screen
            if (deathScreenManager != null)
            {
                deathScreenManager.ShowDeathScreen();
            }

            Debug.Log("Player entered the instant death zone!");
        }
    }
}