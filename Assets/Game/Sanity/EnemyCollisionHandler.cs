using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Kontrola, zda se objekt s tagem "Enemy" dotýká objektu s tagem "Player"
        if (collision.collider.CompareTag("Player"))
        {
            // Najdeme SanityManager na Playerovi
            SanityManager sanityManager = collision.collider.GetComponent<SanityManager>();

            if (sanityManager != null)
            {
                // Snížení sanity pøi doteku
                sanityManager.currentSanity -= sanityManager.sanityLossOnTouch;
                sanityManager.currentSanity = Mathf.Clamp(sanityManager.currentSanity, sanityManager.minSanity, sanityManager.maxSanity);

                Debug.Log("Enemy collided with Player! Sanity decreased.");
            }
        }
    }
}
