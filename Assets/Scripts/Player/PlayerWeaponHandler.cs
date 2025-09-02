using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform rifleParent;
    [SerializeField] private Animator animator;

    private RifleController currentRifle;
    private bool isHoldingRifle = false;

    private const string IS_SHOOTING = "IsShooting";

    
    public void AssignRifle(RifleController rifle)
    {
        currentRifle = rifle;
        isHoldingRifle = true;
        animator.SetBool(IS_SHOOTING, true);
        Invoke(nameof(RemoveRifle), rifle.GetDestructionTime());
    }

    private void RemoveRifle()
    {
        if (currentRifle == null) return;

        isHoldingRifle = false;
        animator.SetBool(IS_SHOOTING, false);
        currentRifle.ReturnRifleToPool();
        currentRifle = null;
        
    }

    public void Shoot()
    {
        if (isHoldingRifle && currentRifle != null)
        {
            currentRifle.Shoot();
        }
    }

    public Transform GetRifleParent() => rifleParent;
}