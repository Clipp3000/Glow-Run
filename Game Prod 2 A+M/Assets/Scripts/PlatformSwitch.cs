using UnityEngine;
using System.Collections;

public class PlatformSwitch : MonoBehaviour
{
    [Header("Platforms")]
    public GameObject[] platforms;

    [Header("Settings")]
    public float activeTime = 5f;

    private Coroutine activationCoroutine;

    private void Start()
    {
        // Platforms are OFF by default
        SetPlatforms(false);
    }

    // Call this when the player shoots the switch
    public void ActivateSwitch()
    {
        Debug.Log("SWITCH SHOT!");

        // If already active, restart the timer
        if (activationCoroutine != null)
        {
            StopCoroutine(activationCoroutine);
        }

        activationCoroutine = StartCoroutine(ActivatePlatforms());
    }

    private IEnumerator ActivatePlatforms()
    {
        Debug.Log("PLATFORMS ACTIVATED!");

        // Turn platforms ON
        SetPlatforms(true);

        // Keep them active for the set amount of time
        yield return new WaitForSeconds(activeTime);

        // Turn platforms OFF again
        SetPlatforms(false);

        Debug.Log("PLATFORMS DEACTIVATED!");

        activationCoroutine = null;
    }

    private void SetPlatforms(bool state)
    {
        foreach (GameObject platform in platforms)
        {
            if (platform != null)
            {
                platform.SetActive(state);
            }
        }
    }
}