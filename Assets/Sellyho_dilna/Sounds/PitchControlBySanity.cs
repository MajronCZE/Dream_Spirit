using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PitchControlBySanity : MonoBehaviour
{
    [Header("Sanity Manager Reference")]
    public SanityManager sanityManager; // Odkaz na SanityManager

    [Header("Pitch Settings")]
    public float maxPitch = 1f; // Maximální hodnota pitch
    public float minPitch = 0f; // Minimální hodnota pitch (napø. hlubší tón)

    private AudioSource audioSource;

    void Start()
    {
        // Získání AudioSource komponenty
        audioSource = GetComponent<AudioSource>();

        if (sanityManager == null)
        {
            Debug.LogError("SanityManager reference is missing. Please link it in the inspector.");
        }

        // Nastavení výchozího pitch
        if (audioSource != null)
        {
            audioSource.pitch = maxPitch;
        }
    }

    void Update()
    {
        if (sanityManager != null && audioSource != null)
        {
            // Výpoèet pitch na základì aktuální sanity
            float sanityPercentage = Mathf.InverseLerp(sanityManager.minSanity, sanityManager.maxSanity, sanityManager.currentSanity);
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, sanityPercentage);
        }
    }
}
