using UnityEngine;

public class GunPickup : MonoBehaviour
{
    // Reference na skript st��len� na hr��i
    public PlayerShooting playerShooting;

    // Vzd�lenost, na kterou m��e hr�� sebrat zbra�
    public float pickupRange = 2f;

    void Update()
    {
        // Kontrola vstupu hr��e (kl�vesa E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupGun();
        }
    }

    private void TryPickupGun()
    {
        // Z�sk�n� pozice hr��e a tohoto objektu
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // Zkontrolujeme, zda je hr�� dostate�n� bl�zko
            if (distanceToPlayer <= pickupRange)
            {
                PickupGun(player);
            }
        }
    }

    private void PickupGun(GameObject player)
    {
        // Aktivace skriptu PlayerShooting na hr��i
        if (playerShooting != null)
        {
            playerShooting.enabled = true;
        }

        // Deaktivace nebo zni�en� objektu GunPickup
        gameObject.SetActive(false);

        Debug.Log("Zbra� byla sebr�na!");
    }
}
