using UnityEngine;
using System.Collections; // Pøidáno pro použití IEnumerator

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float shootingCooldown = 0.5f;
    public float raycastRange = 100f;
    public float sphereRadius = 0.5f;
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
    public GameObject hitEffectPrefab;

    [Header("Sanity Manager")]
    public SanityManager sanityManager;

    [Header("Camera Shake Settings")]
    public Camera mainCamera; // Reference na hlavní kameru
    public float shakeDuration = 0.1f; // Doba tøesení
    public float shakeMagnitude = 0.1f; // Intenzita tøesení

    private float lastShotTime;
    private Vector3 gunDefaultPosition;
    private bool isAnimating = false;

    // Variables for Gizmos visualization
    private Vector3 gizmosOrigin;
    private Vector3 gizmosDirection;
    private float gizmosDistance;
    private float gizmosRadius;
    private bool showGizmos;

    // Pro uložení pùvodní pozice kamery
    private Vector3 originalCamPos;
    private Coroutine shakeCoroutine;

    void Start()
    {
        if (gunTransform != null)
        {
            gunDefaultPosition = gunTransform.localPosition;
        }

        // Uložení pùvodní pozice kamery
        if (mainCamera != null)
        {
            originalCamPos = mainCamera.transform.localPosition;
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

            // Spuštìní tøesení kamery
            if (mainCamera != null)
            {
                if (shakeCoroutine != null)
                {
                    StopCoroutine(shakeCoroutine);
                    mainCamera.transform.localPosition = originalCamPos;
                }
                shakeCoroutine = StartCoroutine(ShakeCamera());
            }
        }
    }

    private void Shoot()
    {
        // Vizuální projektily
        if (projectilePrefab != null && shootPoint != null)
        {
            for (int i = 0; i < pelletCount; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
                Vector3 baseDirection = shootPoint.forward;

                // Dynamický rozptyl støel
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

        // SphereCast pro detekci zásahu
        RaycastHit hit;
        Vector3 origin = shootPoint.position;
        Vector3 direction = shootPoint.forward;

        float sphereCastDistance = raycastRange;
        bool sphereCastHit = Physics.SphereCast(origin, sphereRadius, direction, out hit, raycastRange);

        if (sphereCastHit)
        {
            HandleHit(hit.collider, hit.point, hit.normal);

            // Upravit vzdálenost pro vizualizaci na základì místa zásahu
            sphereCastDistance = hit.distance;
        }

        // Store variables for Gizmos visualization
        gizmosOrigin = origin;
        gizmosDirection = direction;
        gizmosDistance = sphereCastDistance;
        gizmosRadius = sphereRadius;
        showGizmos = true;

        // Pokud SphereCast nic netrefil, mùžeme zkusit OverlapSphere pro blízké nepøátele (volitelné)
        float closeRangeRadius = 3f;
        Collider[] closeHits = Physics.OverlapSphere(shootPoint.position, closeRangeRadius);
        foreach (var collider in closeHits)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyGood"))
            {
                Debug.Log($"{collider.tag} hit by OverlapSphere");

                // Zavoláme HandleHit s informacemi o zásahu
                HandleHit(collider, collider.transform.position, -shootPoint.forward);
                break;
            }
        }
    }

    // Metoda pro tøesení kamery
    private IEnumerator ShakeCamera()
    {
        float elapsed = 0.0f;

        Vector3 originalPos = mainCamera.transform.localPosition;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }

    // Method to visualize SphereCast using Gizmos
    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(gizmosOrigin, gizmosOrigin + gizmosDirection * gizmosDistance);

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

            var enemyScript = collider.GetComponent<EnemyDisappear>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage();
            }

            sanityManager?.AdjustSanityOnHit(collider.tag);

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            }
        }
    }

    private IEnumerator AnimateGun()
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