using UnityEngine;

public class ExtraSanityEffect : MonoBehaviour
{
    [Header("Extra Sanity Effect Settings")]
    public float additionalSanityLoss = 10f; // Kolik sanity ubrat navíc

    private void OnCollisionEnter(Collision collision)
    {
        // Zjistíme, zda kolidujeme s hráèem
        if (collision.collider.CompareTag("Player"))
        {
            // Najdeme komponentu SanityManager na hráèi
            SanityManager sanityManager = collision.collider.GetComponent<SanityManager>();

            if (sanityManager != null)
            {
                // Odeèteme další sanity podle nastavené hodnoty
                sanityManager.currentSanity -= additionalSanityLoss;

                // Zajistíme, že sanity zùstane v povolených mezích
                sanityManager.currentSanity = Mathf.Clamp(
                    sanityManager.currentSanity,
                    sanityManager.minSanity,
                    sanityManager.maxSanity
                );

                Debug.Log($"Player touched by {gameObject.name} with ExtraSanityEffect! Additional sanity loss: {additionalSanityLoss}");
            }
        }
    }
}
