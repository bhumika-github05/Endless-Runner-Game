using System;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem explosion;
    private int zombieHealth = 5;

    private const string ZOMBIE_DIE = "Zombie_Die";
    private const string ZOMBIE_IDLE = "Zombie_Idle";

    public static event Action OnZombieDeath;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        ScanZombiePosition();
    }

    private void ScanZombiePosition()
    {
        if (cameraTransform.position.z >= transform.position.z)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.z = -moveSpeed;
        rb.linearVelocity = currentVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //zombieHealth--;

            //if (zombieHealth < 0)
            //{
            //    ZombieDeath();
            //}
            ZombieDeath();
        }

    }
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool(ZOMBIE_IDLE, true);
            rb.linearVelocity = Vector3.zero;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.Die();

            transform.parent.GetComponent<ZombieSpawner>().DestroyOtherZombies(this);
        }
    }


    public void ZombieDeath()
    {
        OnZombieDeath?.Invoke();
        rb.linearVelocity = Vector3.zero;
        //anim.SetTrigger(ZOMBIE_DIE);
        explosion.Play();
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 0.5f);
    }
}
