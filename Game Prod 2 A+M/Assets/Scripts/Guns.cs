using UnityEngine;

public class Guns : MonoBehaviour
{
    [Header("Gun Settings")]
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.2f;

    [Header("References")]
    public Camera playerCamera;
    public Transform muzzlePoint;
    public ParticleSystem muzzleFlash;
    public Animator animator;

    [Header("Hit Impact")]
    public GameObject hitImpactPrefab;

    [Header("Raycast")]
    public LayerMask hitLayers;

    private float nextTimeToFire = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

            muzzleFlash.Play();
        }

        // Shooting animation
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        // Ray from center of camera
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, hitLayers))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // =========================
            // PLATFORM SWITCH
            // =========================

            PlatformSwitch platformSwitch =
                hit.collider.GetComponentInParent<PlatformSwitch>();

            if (platformSwitch != null)
            {
                platformSwitch.ActivateSwitch();
            }

            // =========================
            // ENEMY DAMAGE
            // =========================


           // EnemyHealth enemy =
               // hit.collider.GetComponentInParent<EnemyHealth>();

           // if (enemy != null)
            //{
           //     enemy.TakeDamage(damage);
           // }

            // =========================
            // HIT IMPACT
            // =========================

            if (hitImpactPrefab != null)
            {
                GameObject impact = Instantiate(
                    hitImpactPrefab,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );

                Destroy(impact, 2f);
            }
        }
    }
}