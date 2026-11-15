using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public int collectibleValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectibleManager manager = Object.FindAnyObjectByType<CollectibleManager>();

            if (manager != null)
            {
                manager.AddScore(collectibleValue);
                Destroy(gameObject);
            }
        }
    }
}
