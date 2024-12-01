using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private bool isDestroyed = false; // Stav, zda byl nep��tel ji� zni�en

    private void OnDestroy()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;

            // Zpr�va do konzole
            Debug.Log("jakub JE KRYSA!");

            // Naj�t hr��e
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Naj�t SanityManager na hr��i
                SanityManager sanityManager = player.GetComponent<SanityManager>();
                if (sanityManager != null)
                {
                    // Poslat zpr�vu SanityManageru s tagem objektu
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