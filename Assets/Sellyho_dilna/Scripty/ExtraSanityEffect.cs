using UnityEngine;

public class ExtraSanityEffect : MonoBehaviour
{
    [Header("Extra Sanity Effect Settings")]
    public float additionalSanityLoss = 10f; // Kolik sanity ubrat nav�c

    private void OnCollisionEnter(Collision collision)
    {
        // Zjist�me, zda kolidujeme s hr��em
        if (collision.collider.CompareTag("Player"))
        {
            // Najdeme komponentu SanityManager na hr��i
            SanityManager sanityManager = collision.collider.GetComponent<SanityManager>();

            if (sanityManager != null)
            {
                // Ode�teme dal�� sanity podle nastaven� hodnoty
                sanityManager.currentSanity -= additionalSanityLoss;

                // Zajist�me, �e sanity z�stane v povolen�ch mez�ch
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
