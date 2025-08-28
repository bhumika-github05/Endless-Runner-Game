//using UnityEngine;

//public class NewMonoBehaviourScript : MonoBehaviour
//{
//    [SerializeField] private GameObject laserBeamPrefab; // Assign your prefab in Inspector
//    [SerializeField] private float laserDuration = 0.1f;  // How long the laser is visible

//    public void Shoot()
//    {
//        RaycastHit hit;
//        Vector3 shootDirection = shootPoint.forward;

//        // Perform a raycast
//        if (Physics.Raycast(shootPoint.position, shootDirection, out hit, 100f))
//        {
//            // Hit something
//            SpawnLaser(shootPoint.position, hit.point);

//            // Optional: Damage logic
//            if (hit.collider.CompareTag("Enemy"))
//            {
//                // Apply damage
//            }
//        }
//        else
//        {
//            // Didn't hit anything — just show beam
//            SpawnLaser(shootPoint.position, shootPoint.position + shootDirection * 100f);
//        }
//    }

//    private void SpawnLaser(Vector3 start, Vector3 end)
//    {
//        GameObject laser = Instantiate(laserBeamPrefab);
//        LineRenderer lr = laser.GetComponent<LineRenderer>();
//        lr.SetPosition(0, start);
//        lr.SetPosition(1, end);

//        Destroy(laser, laserDuration);
//    }

//}
