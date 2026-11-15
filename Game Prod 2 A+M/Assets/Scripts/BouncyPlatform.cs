using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    public float bounceForce = 20f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            // Reset falling speed
            player.velocity.y = 0f;

            // Launch player upward
            player.velocity.y = bounceForce;
        }
    }
}