using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        //rb.AddForce(Vector3.forward * moveSpeed * Time.deltaTime);
        rb.linearVelocity = Vector3.forward * moveSpeed * Time.deltaTime;
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            Destroy(gameObject);
        }
    }
}
