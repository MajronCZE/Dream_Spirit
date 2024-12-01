using UnityEngine;

public class PlayerShootSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip shootSound; // Zvuk støelby
    public float soundCooldown = 0.5f; // Cooldown mezi pøehráním zvuku

    [Header("Audio Source")]
    public AudioSource audioSource; // Zdroj zvuku

    private float lastSoundTime; // Èas posledního pøehrání zvuku

    void Update()
    {
        // Kontrola stisku levého tlaèítka myši
        if (Input.GetMouseButtonDown(0) && Time.time >= lastSoundTime + soundCooldown)
        {
            PlayShootSound();
        }
    }

    private void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound); // Pøehraje zvuk
            lastSoundTime = Time.time; // Uloží aktuální èas jako èas posledního pøehrání
        }
        else
        {
            Debug.LogWarning("Chybí AudioClip nebo AudioSource!");
        }
    }
}
