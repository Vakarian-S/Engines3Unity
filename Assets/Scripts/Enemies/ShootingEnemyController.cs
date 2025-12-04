using System.Collections;
using UnityEngine;

public class ShooterEnemyController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private bool shouldRequireLineOfSight = false;
    [SerializeField] private LayerMask lineOfSightObstructionMask;

    [Header("Barrage Settings")]
    [SerializeField] private float timeBetweenBarrages = 2f;
    [SerializeField] private int bulletsPerBarrage = 5;
    [SerializeField] private float timeBetweenBulletsInBarrage = 0.15f;

    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] firePoints;   // You can have 1 or multiple
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private float bulletLifeTime = 5f;
    [SerializeField] private bool shouldAimAtTarget = true;

    private bool isShooting;

    private void Start()
    {
        if (targetTransform == null && !string.IsNullOrEmpty(playerTag))
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObject != null)
            {
                targetTransform = playerObject.transform;
            }
        }

        StartCoroutine(ShootingLoopCoroutine());
    }

    private IEnumerator ShootingLoopCoroutine()
    {
        // Small random offset so multiple shooters do not perfectly sync
        float initialOffset = Random.Range(0f, timeBetweenBarrages);
        yield return new WaitForSeconds(initialOffset);

        while (true)
        {
            if (IsTargetWithinRangeAndVisible())
            {
                yield return StartCoroutine(ShootBarrageCoroutine());
            }

            yield return new WaitForSeconds(timeBetweenBarrages);
        }
    }

    private bool IsTargetWithinRangeAndVisible()
    {
        if (!targetTransform)
        {
            return false;
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        if (distanceToTarget > detectionRange)
        {
            return false;
        }

        if (!shouldRequireLineOfSight)
        {
            return true;
        }

        Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hitInfo, detectionRange, ~0))
        {
            // If we hit the player (or their transform), we have line of sight
            if (hitInfo.transform == targetTransform)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator ShootBarrageCoroutine()
    {
        if (isShooting)
        {
            yield break;
        }

        isShooting = true;

        for (int bulletIndex = 0; bulletIndex < bulletsPerBarrage; bulletIndex++)
        {
            ShootOnce();
            yield return new WaitForSeconds(timeBetweenBulletsInBarrage);
        }

        isShooting = false;
    }

    private void ShootOnce()
    {
        if (!bulletPrefab)
        {
            Debug.LogWarning("ShooterEnemyController has no bulletPrefab assigned.", this);
            return;
        }

        if (firePoints == null || firePoints.Length == 0)
        {
            Debug.LogWarning("ShooterEnemyController has no firePoints assigned.", this);
            return;
        }

        foreach (Transform firePoint in firePoints)
        {
            if (firePoint == null)
            {
                continue;
            }

            // Decide direction
            Vector3 shootDirection;

            if (shouldAimAtTarget && targetTransform != null)
            {
                shootDirection = (targetTransform.position - firePoint.position).normalized;

                // Rotate the fire point or bullet so it visually faces the target (optional)
                if (shootDirection != Vector3.zero)
                {
                    firePoint.rotation = Quaternion.LookRotation(shootDirection);
                }
            }
            else
            {
                // Use whatever direction the firePoint is facing
                shootDirection = firePoint.forward;
            }

            // Spawn bullet
            GameObject bulletInstance = Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.LookRotation(shootDirection)
            );
            bulletInstance.GetComponent<ProjectileController>().SetDirection(shootDirection);
            bulletInstance.GetComponent<ProjectileController>().SetTargetTag("Player");
            bulletInstance.GetComponent<ProjectileController>().SetDestroyOnAnyHit(false);

            // Destroy after some time so we do not leak objects
            Destroy(bulletInstance, bulletLifeTime);
        }
    }
}
