using UnityEngine;

public class RifleController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject riflePrefab;
    [SerializeField] private float rifleInHandDestructionTime = 10f;

    [SerializeField] private GameObject laserBeamPrefab;
    [SerializeField] private float laserBeamDuration = 0.1f;
    [SerializeField] private float laserBeamDistance = 100f;

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        ScanRiflePosition();
    }

    public void ScanRiflePosition()
    {
        if (cameraTransform.position.z >= transform.position.z)
        {
            Destroy(gameObject);
        }
    }

    public float GetDestructionTime()
    {
        return rifleInHandDestructionTime;
    }

    public void Shoot()
    {
        //GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        RaycastHit hit;
        Vector3 shootDirection = Vector3.forward;

        if (Physics.Raycast(shootPoint.position, shootDirection, out hit, laserBeamDistance))
        {

            SpawnLaser(shootPoint.position, hit.point);
            if (hit.collider.CompareTag("Zombie"))
            {
                ZombieController zombie = hit.collider.GetComponent<ZombieController>();
                if (zombie != null)
                {
                    zombie.ZombieDeath();
                }

            }
        }
        else
        {
            SpawnLaser(shootPoint.position, shootPoint.position + shootDirection * laserBeamDistance);
        }
    }

    private void SpawnLaser(Vector3 start, Vector3 end)
    {
        GameObject laser = Instantiate(laserBeamPrefab);
        LineRenderer lr = laser.GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Destroy(laser, laserBeamDuration);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            GameObject rifleParent = player.rifleParent;
            if (rifleParent.transform.childCount == 0)
            {
                Destroy(gameObject);

                GameObject rifle = Instantiate(riflePrefab, rifleParent.transform.position, rifleParent.transform.rotation, rifleParent.transform);
                rifle.transform.localPosition = Vector3.zero;
                rifle.transform.localRotation = Quaternion.identity;

                RifleController rifleScript = rifle.GetComponent<RifleController>();
                player.AssignRifle(rifleScript);

                Destroy(rifle, rifleInHandDestructionTime);

            }

        }
    }
}
