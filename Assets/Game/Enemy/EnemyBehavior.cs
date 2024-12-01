using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private bool isDestroyed = false; // Stav, zda byl nepøítel již znièen

    private void OnDestroy()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;

            // Zpráva do konzole
            Debug.Log("jakub JE KRYSA!");

            // Najít hráèe
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Najít SanityManager na hráèi
                SanityManager sanityManager = player.GetComponent<SanityManager>();
                if (sanityManager != null)
                {
                    // Poslat zprávu SanityManageru s tagem objektu
                    sanityManager.NotifyEnemyDestroyed(gameObject.tag);
                }
                else
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
}