using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float shootingCooldown = 0.5f;
    public float raycastRange = 100f;
    public float sphereRadius = 0.5f; // Polom�r pro SphereCast
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

    // Variables for Gizmos visualization
    private Vector3 gizmosOrigin;
    private Vector3 gizmosDirection;
    private float gizmosDistance;
    private float gizmosRadius;
    private bool showGizmos;

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

                // Dynamick� rozptyl st�el
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

        // SphereCast pro detekci z�sahu
        RaycastHit hit;
        Vector3 origin = shootPoint.position;
        Vector3 direction = shootPoint.forward;

        // Pro vizualizaci pot�ebujeme vypo��tat kone�n� bod SphereCastu
        float sphereCastDistance = raycastRange;
        bool sphereCastHit = Physics.SphereCast(origin, sphereRadius, direction, out hit, raycastRange);

        if (sphereCastHit)
        {
            HandleHit(hit.collider, hit.point, hit.normal);

            // Upravit vzd�lenost pro vizualizaci na z�klad� m�sta z�sahu
            sphereCastDistance = hit.distance;
        }

        // Store variables for Gizmos visualization
        gizmosOrigin = origin;
        gizmosDirection = direction;
        gizmosDistance = sphereCastDistance;
        gizmosRadius = sphereRadius;
        showGizmos = true;

        // Pokud SphereCast nic netrefil, m��eme zkusit OverlapSphere pro bl�zk� nep��tele (voliteln�)
        float closeRangeRadius = 3f; // Polom�r pro OverlapSphere
        Collider[] closeHits = Physics.OverlapSphere(shootPoint.position, closeRangeRadius);
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

    // Method to visualize SphereCast using Gizmos
    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            // Nastav barvu pro vizualizaci
            Gizmos.color = Color.red;

            // Kresl�me linii od po��tku do koncov�ho bodu SphereCastu
            Gizmos.DrawLine(gizmosOrigin, gizmosOrigin + gizmosDirection * gizmosDistance);

            // Po�et segment�, kter� p�edstavuj� kolik sf�r se vykresl� pod�l cesty
            int segments = 10;

            for (int i = 0; i <= segments; i++)
            {
                float t = (float)i / segments;
                Vector3 position = gizmosOrigin + gizmosDirection * gizmosDistance * t;
                Gizmos.DrawWireSphere(position, gizmosRadius);
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