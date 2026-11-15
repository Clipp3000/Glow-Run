using UnityEngine;

public class GlowKick : MonoBehaviour
{
    [Header("Kick Settings")]
    public float kickDamage = 50f;
    public float kickRange = 2f;
    public float kickRadius = 1f;
    public float cooldown = 1.5f;

    [Header("References")]
    public Animator animator;
    public Transform kickPoint;

    [Header("Glow")]
    public ParticleSystem glowEffect;

    private float nextKickTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextKickTime)
        {
            PerformKick();
        }
    }

    void PerformKick()
    {
        nextKickTime = Time.time + cooldown;

        // Play kick animation
        if (animator != null)
        {
            animator.SetTrigger("GlowKick");
        }

        // Play glow effect
        if (glowEffect != null)
        {
            glowEffect.Play();
        }

        // Detect enemies
        Collider[] hits = Physics.OverlapSphere(
            kickPoint.position,
            kickRadius
        );


    }

    void OnDrawGizmosSelected()
    {
        if (kickPoint == null)
            return;

        Gizmos.DrawWireSphere(
            kickPoint.position,
            kickRadius
        );
    }
}
