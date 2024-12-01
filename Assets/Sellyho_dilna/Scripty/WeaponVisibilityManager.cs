using UnityEngine;

public class WeaponVisibilityManager : MonoBehaviour
{
    private MeshRenderer weaponMeshRenderer; // Odkaz na MeshRenderer na t�to zbrani
    private PlayerShooting playerShooting;  // Reference na PlayerShooting skript

    void Start()
    {
        // Z�sk�me MeshRenderer na zbrani
        weaponMeshRenderer = GetComponent<MeshRenderer>();
        if (weaponMeshRenderer != null)
        {
            weaponMeshRenderer.enabled = false; // Na za��tku vypneme viditelnost zbran�
        }

        // Najdeme PlayerShooting na sc�n� (na hr��i)
        playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting == null)
        {
            Debug.LogError("PlayerShooting script not found in the scene.");
        }
    }

    void Update()
    {
        // Pokud je PlayerShooting aktivn�, zapneme viditelnost zbran�
        if (playerShooting != null && playerShooting.isActiveAndEnabled)
        {
            if (weaponMeshRenderer != null)
            {
                weaponMeshRenderer.enabled = true; // Zobraz�me zbra�
                enabled = false; // Vypneme tento skript, proto�e u� nen� pot�eba
            }
        }
    }
}
