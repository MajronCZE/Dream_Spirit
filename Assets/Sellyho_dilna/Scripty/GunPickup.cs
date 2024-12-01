using UnityEngine;

public class GunPickup : MonoBehaviour
{
    // Reference na skript støílení na hráèi
    public PlayerShooting playerShooting;

    // Vzdálenost, na kterou mùže hráè sebrat zbraò
    public float pickupRange = 2f;

    void Update()
    {
        // Kontrola vstupu hráèe (klávesa E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupGun();
        }
    }

    private void TryPickupGun()
    {
        // Získání pozice hráèe a tohoto objektu
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // Zkontrolujeme, zda je hráè dostateènì blízko
            if (distanceToPlayer <= pickupRange)
            {
                PickupGun(player);
            }
        }
    }

    private void PickupGun(GameObject player)
    {
        // Aktivace skriptu PlayerShooting na hráèi
        if (playerShooting != null)
        {
            playerShooting.enabled = true;
        }

        // Deaktivace nebo znièení objektu GunPickup
        gameObject.SetActive(false);

        Debug.Log("Zbraò byla sebrána!");
    }
}
