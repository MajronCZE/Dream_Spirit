using UnityEngine;

public class PlayerShootSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip shootSound; // Zvuk st�elby
    public float soundCooldown = 0.5f; // Cooldown mezi p�ehr�n�m zvuku

    [Header("Audio Source")]
    public AudioSource audioSource; // Zdroj zvuku

    private float lastSoundTime; // �as posledn�ho p�ehr�n� zvuku

    void Update()
    {
        // Kontrola stisku lev�ho tla��tka my�i
        if (Input.GetMouseButtonDown(0) && Time.time >= lastSoundTime + soundCooldown)
        {
            PlayShootSound();
        }
    }

    private void PlayShootSound()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound); // P�ehraje zvuk
            lastSoundTime = Time.time; // Ulo�� aktu�ln� �as jako �as posledn�ho p�ehr�n�
        }
        else
        {
            Debug.LogWarning("Chyb� AudioClip nebo AudioSource!");
        }
    }
}
