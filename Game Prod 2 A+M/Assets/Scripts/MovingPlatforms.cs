using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float speed = 7f;

    private int currentPointIndex = 0;

    void Update()
    {
        if (points.Length == 0) return;

        // platform moves towards the targeted point
        Transform target = points[currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // platform switches direction to the other point once close enough
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % points.Length;
        }
    }
}
