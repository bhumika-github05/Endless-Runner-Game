using UnityEngine;

public class PlayerAnimation : IAnimationHandler
{
    private readonly Animator anim;

    private const string IS_RUNNING = "IsRunning";
    private const string JUMP = "Jump";
    private const string ROLL = "Roll";
    private const string DIE = "Die";
    private const string IS_SHOOTING = "IsShooting";

    public PlayerAnimation(Animator animator)
    {
        anim = animator;
    }

    public void SetRunning(bool value) => anim.SetBool(IS_RUNNING, value);
    public void TriggerJump() => anim.SetTrigger(JUMP);
    public void TriggerRoll() => anim.SetTrigger(ROLL);
    public void TriggerDie() => anim.SetTrigger(DIE);
    public void SetShooting(bool value) => anim.SetBool(IS_SHOOTING, value);
}
