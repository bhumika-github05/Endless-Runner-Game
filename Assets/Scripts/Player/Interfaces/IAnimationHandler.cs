public interface IAnimationHandler
{
    void SetRunning(bool value);
    void TriggerJump();
    void TriggerRoll();
    void TriggerDie();
    void SetShooting(bool value);
}