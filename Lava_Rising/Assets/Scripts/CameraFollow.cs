using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 minValues, maxValues;

    [Range(1, 10)] public float smooth;

    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        //Camera Limitation check
        Vector3 targetPos = target.position + offset;

        //Out of bound detection
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPos.x, minValues.x, maxValues.x),
            Mathf.Clamp(targetPos.y, minValues.y, maxValues.y),
            Mathf.Clamp(targetPos.z, minValues.z, maxValues.z));
        
        // transform.position = targetPos;
        Vector3 smoothPos = Vector3.Lerp(transform.position, boundPosition, smooth * Time.fixedDeltaTime);
        transform.position = smoothPos;
    }
}
