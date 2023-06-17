using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector2 offset;

    private Vector2 velocity = Vector2.zero;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        // Calculate the desired position of the camera
        Vector2 targetPosition = (Vector2)target.position + offset;

        // Smoothly move the camera towards the target position using SmoothDamp
        transform.position = new Vector3(
            Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity.x, smoothTime),
            Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, smoothTime),
            transform.position.z
        );
    }
}
