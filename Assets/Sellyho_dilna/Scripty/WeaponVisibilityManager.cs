using UnityEngine;

public class WeaponVisibilityManager : MonoBehaviour
{
    private MeshRenderer weaponMeshRenderer; // Odkaz na MeshRenderer na této zbrani
    private PlayerShooting playerShooting;  // Reference na PlayerShooting skript

    void Start()
    {
        // Získáme MeshRenderer na zbrani
        weaponMeshRenderer = GetComponent<MeshRenderer>();
        if (weaponMeshRenderer != null)
        {
            weaponMeshRenderer.enabled = false; // Na zaèátku vypneme viditelnost zbranì
        }

        // Najdeme PlayerShooting na scénì (na hráèi)
        playerShooting = FindObjectOfType<PlayerShooting>();
        if (playerShooting == null)
        {
            Debug.LogError("PlayerShooting script not found in the scene.");
        }
    }

    void Update()
    {
        // Pokud je PlayerShooting aktivní, zapneme viditelnost zbranì
        if (playerShooting != null && playerShooting.isActiveAndEnabled)
        {
            if (weaponMeshRenderer != null)
            {
                weaponMeshRenderer.enabled = true; // Zobrazíme zbraò
                enabled = false; // Vypneme tento skript, protože už není potøeba
            }
        }
    }
}
