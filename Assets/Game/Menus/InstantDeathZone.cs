using UnityEngine;

public class InstantDeathZone : MonoBehaviour
{
    public DeathScreenManager deathScreenManager; // Odkaz na DeathScreenManager

    private void OnTriggerEnter(Collider other)
    {
        // Kontrolujeme, zda objekt, který vstoupil do zóny, má tag "Player"
        if (other.CompareTag("Player"))
        {
            // Spustíme Death Screen
            if (deathScreenManager != null)
            {
                deathScreenManager.ShowDeathScreen();
            }

            Debug.Log("Player entered the instant death zone!");
        }
    }
}