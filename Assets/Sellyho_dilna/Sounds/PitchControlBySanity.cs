using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PitchControlBySanity : MonoBehaviour
{
    [Header("Sanity Manager Reference")]
    public SanityManager sanityManager; // Odkaz na SanityManager

    [Header("Pitch Settings")]
    public float maxPitch = 1f; // Maxim�ln� hodnota pitch
    public float minPitch = 0f; // Minim�ln� hodnota pitch (nap�. hlub�� t�n)

    private AudioSource audioSource;

    void Start()
    {
        // Z�sk�n� AudioSource komponenty
        audioSource = GetComponent<AudioSource>();

        if (sanityManager == null)
        {
            Debug.LogError("SanityManager reference is missing. Please link it in the inspector.");
        }

        // Nastaven� v�choz�ho pitch
        if (audioSource != null)
        {
            audioSource.pitch = maxPitch;
        }
    }

    void Update()
    {
        if (sanityManager != null && audioSource != null)
        {
            // V�po�et pitch na z�klad� aktu�ln� sanity
            float sanityPercentage = Mathf.InverseLerp(sanityManager.minSanity, sanityManager.maxSanity, sanityManager.currentSanity);
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, sanityPercentage);
        }
    }
}
