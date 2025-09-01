using UnityEngine;

public class PlayerMovement : IMovementHandler
{
    private readonly Rigidbody rb;
    private readonly IAnimationHandler animHandler;
    private readonly Transform groundCheckPoint;
    private readonly LayerMask groundMask;

    private readonly float moveSpeed, jumpHeight, laneJumpHeight, laneJumpDuration;
    private readonly float groundRadius, fallMultiplier, lowJumpMultiplier;
    private readonly float[] laneXPositions;

    private bool isGrounded, isJumping, isSwitchingLane;
    private int currentLane = 1;
    private float laneSwitchTime;
    private Vector3 laneStartPosition, laneTargetPosition;

    public PlayerMovement(Rigidbody rb, IAnimationHandler animHandler,
        Transform groundCheckPoint, LayerMask groundMask,
        float moveSpeed, float jumpHeight, float laneJumpHeight, float laneJumpDuration,
        float groundRadius, float fallMultiplier, float lowJumpMultiplier,
        float[] laneXPositions)
    {
        this.rb = rb;
        this.animHandler = animHandler;
        this.groundCheckPoint = groundCheckPoint;
        this.groundMask = groundMask;
        this.moveSpeed = moveSpeed;
        this.jumpHeight = jumpHeight;
        this.laneJumpHeight = laneJumpHeight;
        this.laneJumpDuration = laneJumpDuration;
        this.groundRadius = groundRadius;
        this.fallMultiplier = fallMultiplier;
        this.lowJumpMultiplier = lowJumpMultiplier;
        this.laneXPositions = laneXPositions;
    }

    public void MoveForward()
    {
        animHandler.SetRunning(true);
        Vector3 vel = rb.linearVelocity;
        vel.z = moveSpeed;
        rb.linearVelocity = vel;
    }

    public void Jump()
    {
        if (isGrounded && !isJumping && !isSwitchingLane)
        {
            animHandler.SetRunning(false);
            animHandler.TriggerJump();
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isJumping = true;
        }
    }

    public void Roll() => animHandler.TriggerRoll();

    public void MoveLeft()
    {
        if (currentLane > 0) { currentLane--; StartLaneSwitch(); }
    }

    public void MoveRight()
    {
        if (currentLane < laneXPositions.Length - 1) { currentLane++; StartLaneSwitch(); }
    }

    private void StartLaneSwitch()
    {
        animHandler.TriggerJump();
        laneStartPosition = rb.position;
        laneTargetPosition = new Vector3(laneXPositions[currentLane], rb.position.y, rb.position.z);
        laneSwitchTime = 0f;
        isSwitchingLane = true;
    }

    public void SwitchLane()
    {
        if (!isSwitchingLane) return;

        laneSwitchTime += Time.fixedDeltaTime;
        float t = laneSwitchTime / laneJumpDuration;

        if (t >= 1f)
        {
            isSwitchingLane = false;
            return;
        }

        float newX = Mathf.Lerp(laneStartPosition.x, laneTargetPosition.x, t);
        float newY = rb.position.y;
        if (isGrounded && !isJumping)
            newY = laneStartPosition.y + Mathf.Sin(t * Mathf.PI) * laneJumpHeight;

        rb.MovePosition(new Vector3(newX, newY, rb.position.z));
    }

    public void UpdateGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundRadius, groundMask);
        if (isGrounded && isJumping)
        {
            isJumping = false;
            animHandler.SetRunning(true);
        }
    }

    public void BetterJump()
    {
        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.UpArrow))
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }
}

