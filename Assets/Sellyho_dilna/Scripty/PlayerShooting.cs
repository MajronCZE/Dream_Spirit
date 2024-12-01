using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float shootingCooldown = 0.5f;
    public float raycastRange = 100f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 20f;
    public int pelletCount = 5;
    public float spreadAngle = 5f;

    [Header("Gun Animation Settings")]
    public Transform gunTransform;
    public float animationSpeed = 10f;
    public float returnSpeedMultiplier = 0.5f;
    public float moveDistance = 0.2f;

    [Header("Particle Effect")]
    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab; // Efekt na m�st� z�sahu

    [Header("Sanity Manager")]
    public SanityManager sanityManager;

    private float lastShotTime;
    private Vector3 gunDefaultPosition;
    private bool isAnimating = false;

    void Start()
    {
        if (gunTransform != null)
        {
            gunDefaultPosition = gunTransform.localPosition;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShotTime + shootingCooldown)
        {
            Shoot();
            lastShotTime = Time.time;

            if (!isAnimating && gunTransform != null)
            {
                StartCoroutine(AnimateGun());
            }

            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
        }
    }

    private void Shoot()
    {
        // Vizu�ln� projektily
        if (projectilePrefab != null && shootPoint != null)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
                Vector3 baseDirection = shootPoint.forward;

                float angleX = Random.Range(-spreadAngle, spreadAngle);
                float angleY = Random.Range(-spreadAngle, spreadAngle);
                Quaternion spreadRotation = Quaternion.Euler(angleY, angleX, 0);
                Vector3 finalDirection = spreadRotation * baseDirection;

                projectile.transform.rotation = Quaternion.LookRotation(finalDirection);

                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = finalDirection * projectileSpeed;
                }

                Destroy(projectile, 4f);
            }
        }

        // Raycast pro detekci z�sahu
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, raycastRange))
        {
            HandleHit(hit.collider, hit.point, hit.normal);
        }
        else
        {
            // Pokud Raycast nic netrefil, zkus�me naj�t nep��tele bl�zko
            Collider[] closeHits = Physics.OverlapSphere(shootPoint.position, 2f); // 2f = polom�r pro bl�zk� c�le
            foreach (var collider in closeHits)
            {
                if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyGood"))
                {
                    Debug.Log($"{collider.tag} hit by OverlapSphere");

                    // Zavol�me HandleHit s informacemi o z�sahu
                    HandleHit(collider, collider.transform.position, -shootPoint.forward);
                    break; // Zastav�me po prvn�m z�sahu
                }
            }
        }
    }

    private void HandleHit(Collider collider, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyGood"))
        {
            Debug.Log($"{collider.tag} hit");

            // Z�sk�n� komponenty EnemyDisappear
            var enemyScript = collider.GetComponent<EnemyDisappear>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage();
            }

            sanityManager?.AdjustSanityOnHit(collider.tag);

            // P�id�n� vizu�ln�ho efektu na m�sto z�sahu
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            }
        }
    }

    private System.Collections.IEnumerator AnimateGun()
    {
        isAnimating = true;

        Vector3 targetPosition = gunDefaultPosition - new Vector3(0, 0, moveDistance);
        while (Vector3.Distance(gunTransform.localPosition, targetPosition) > 0.01f)
        {
            gunTransform.localPosition = Vector3.MoveTowards(
                gunTransform.localPosition,
                targetPosition,
                animationSpeed * Time.deltaTime
            );
            yield return null;
        }

        while (Vector3.Distance(gunTransform.localPosition, gunDefaultPosition) > 0.01f)
        {
            gunTransform.localPosition = Vector3.MoveTowards(
                gunTransform.localPosition,
                gunDefaultPosition,
                animationSpeed * returnSpeedMultiplier * Time.deltaTime
            );
            yield return null;
        }

        gunTransform.localPosition = gunDefaultPosition;
        isAnimating = false;
    }
}