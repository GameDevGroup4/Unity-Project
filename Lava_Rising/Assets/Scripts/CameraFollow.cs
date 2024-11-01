using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    [Range(1, 10)] public float smooth;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 targetPos = target.position + offset;
        // transform.position = targetPos;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smooth * Time.fixedDeltaTime);
        transform.position = smoothPos;
    }
}
