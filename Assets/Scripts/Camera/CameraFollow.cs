using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 0.25f;

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 cameraPosition = new Vector3(
                playerTransform.position.x + offset.x,
                transform.position.y,
                playerTransform.position.z + offset.z
                );
            transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothSpeed);
        }
    }
}
