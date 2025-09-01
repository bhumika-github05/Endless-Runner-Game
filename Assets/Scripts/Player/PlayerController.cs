using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private RifleSpawner rifleSpawner;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float laneSwitchSpeed = 7f;
    [SerializeField] private float laneOffset = 3f;
    [SerializeField] private float laneJumpHeight = 2f;
    [SerializeField] private float laneJumpDuration = 0.3f;
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float[] laneXPositions = new float[3];

    [Header("Managers")]
    [SerializeField] private UIManager uiManager; // ✅ reference UI manager

    private Rigidbody rb;
    private IAnimationHandler animHandler;
    private IMovementHandler movementHandler;
    private IInputHandler inputHandler;
    private PlayerWeaponHandler weaponHandler;

    public bool isDead { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        animHandler = new PlayerAnimation(animator);
        movementHandler = new PlayerMovement(rb, animHandler,
            groundCheckPoint, groundMask,
            moveSpeed, jumpHeight, laneJumpHeight, laneJumpDuration,
            groundCheckRadius, fallMultiplier, lowJumpMultiplier, laneXPositions);

        weaponHandler = GetComponent<PlayerWeaponHandler>();

        // Assign Keyboard+Mouse input handler
        inputHandler = new KeyboardMouseInputHandler(movementHandler, weaponHandler);
    }

    private void Update()
    {
        movementHandler.UpdateGrounded();
        movementHandler.BetterJump();
        inputHandler?.ProcessInput();
    }

    private void FixedUpdate()
    {
        movementHandler.MoveForward();
        movementHandler.SwitchLane();
    }

    public void Die()
    {
        rifleSpawner.DestroyAllRifles(null);
        Invoke(nameof(TriggerGameOver), 1);
        animHandler.TriggerDie();
        isDead = true;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        enabled = false;
    }

    private void TriggerGameOver()
    {
        uiManager.ShowGameOver(); // ✅ delegate UI to UI manager
    }
}


//
// using UnityEngine;
//
// public class PlayerController : MonoBehaviour
// {
//     [SerializeField] private GameObject gameOverScreen;
//     [SerializeField] private RifleSpawner rifleSpawner;
//     [SerializeField] private float moveSpeed = 5f;
//     [SerializeField] private float laneSwitchSpeed = 7f;
//     [SerializeField] private float laneOffset = 3f;
//     [SerializeField] private float laneJumpHeight = 2f;
//     [SerializeField] private float laneJumpDuration = 0.3f;
//     [SerializeField] private Animator anim;
//
//     [SerializeField] private Transform groundCheckPoint;
//     [SerializeField] private LayerMask groundLayerMask;
//     [SerializeField] private float jumpHeight = 6f;
//     [SerializeField] private float groundCheckRadius = 0.3f;
//
//     [SerializeField] private float fallMultiplier = 2.5f;
//     [SerializeField] private float lowJumpMultiplier = 2f;
//
//     private bool isGrounded = false;
//     private bool isJumping = false;
//     public bool isDead = false;
//
//     private Rigidbody rb;
//
//     private Vector3 forwardDirection;
//     private Vector3 laneTargetPosition;
//     private int currentLane = 1;
//     private Vector3 horizontalVelocity;
//
//     [SerializeField] private float[] laneXPositions = new float[3];
//     private bool isSwitchingLane = false;
//     private float laneSwitchTime = 0f;
//     private Vector3 laneStartPosition;
//
//     private Vector2 startTouchPosition;
//     private Vector2 endTouchPosition;
//     private bool isSwiping = false;
//     private float swipeAmount = 10f;
//
//     private const string IS_RUNNING = "IsRunning";
//     private const string JUMP = "Jump";
//     private const string ROLL = "Roll";
//     private const string DIE = "Die";
//     private const string IS_SHOOTING = "IsShooting";
//
//     public GameObject rifleParent;
//
//     private RifleController currentRifle;
//     private bool isHoldingRifle = false;
//
//
//
//
//
//     private void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
//
//         currentLane = 1;
//         forwardDirection = Vector3.forward;
//         laneTargetPosition = transform.position;
//     }
//
//     private void Update()
//     {
//
//         CheckIfGrounded();
//         BetterJump();
//
//         if (!isSwitchingLane)
//         {
//             KeyboardInputMovement();
//             MouseInputMovement();
//         }
//     }
//
//     private void FixedUpdate()
//     {
//         MoveForward();
//
//         if (isSwitchingLane)
//         {
//             SwitchLane();
//         }
//     }
//
//     public void AssignRifle(RifleController rifle)
//     {
//         currentRifle = rifle;
//         isHoldingRifle = true;
//         anim.SetBool(IS_SHOOTING, true);
//         Invoke(nameof(RemoveRifle), rifle.GetDestructionTime());
//     }
//
//     private void RemoveRifle()
//     {
//         if (currentRifle == null) return;
//
//         isHoldingRifle = false;
//         currentRifle = null;
//         anim.SetBool(IS_SHOOTING, false);
//     }
//
//     private void CheckIfGrounded()
//     {
//         isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayerMask);
//
//         if (isGrounded && isJumping)
//         {
//             isJumping = false;
//             anim.SetBool(IS_RUNNING, true);
//         }
//     }
//
//     private void MoveForward()
//     {
//
//         anim.SetBool(IS_RUNNING, true);
//         Vector3 currentVelocity = rb.linearVelocity;
//         currentVelocity.z = moveSpeed;
//         rb.linearVelocity = currentVelocity;
//     }
//
//     private void KeyboardInputMovement()
//     {
//
//         if (Input.GetKeyDown(KeyCode.DownArrow))
//         {
//             Roll();
//         }
//
//         if (Input.GetKeyDown(KeyCode.UpArrow))
//         {
//             Jump();
//         }
//
//         if (Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             MoveLeft();
//         }
//
//         if (Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             MoveRight();
//         }
//
//     }
//
//     public void Die()
//     {
//         rifleSpawner.DestroyAllRifles(currentRifle);
//         Invoke(nameof(ActivateGameOverScreen), 1);
//
//
//         if (isHoldingRifle && currentRifle != null)
//         {
//             currentRifle.gameObject.SetActive(false);
//         }
//
//         anim.SetTrigger(DIE);
//         isDead = true;
//         rb.linearVelocity = Vector3.zero;
//         rb.isKinematic = true;
//         this.enabled = false;
//     }
//
//
//     public void ActivateGameOverScreen()
//     {
//         gameOverScreen.SetActive(true);
//     }
//
//
//     private void MoveLeft()
//     {
//         if (currentLane > 0)
//         {
//             currentLane--;
//             StartLaneSwitch();
//         }
//     }
//
//     private void MoveRight()
//     {
//         if (currentLane < 2)
//         {
//             currentLane++;
//             StartLaneSwitch();
//         }
//     }
//
//     private void Roll()
//     {
//         anim.SetTrigger(ROLL);
//     }
//
//     private void Jump()
//     {
//         if (isGrounded && !isJumping && !isSwitchingLane)
//         {
//             anim.SetBool(IS_RUNNING, false);
//             anim.SetTrigger(JUMP);
//             rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
//             rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
//             isJumping = true;
//         }
//     }
//
//     private void MouseInputMovement()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             startTouchPosition = Input.mousePosition;
//             isSwiping = true;
//         }
//
//         if (Input.GetMouseButtonUp(0) && isSwiping)
//         {
//             endTouchPosition = Input.mousePosition;
//             isSwiping = false;
//
//             Vector2 magPos = endTouchPosition - startTouchPosition;
//
//             if (magPos.magnitude > swipeAmount)
//             {
//                 Vector2 direction = magPos.normalized;
//
//                 if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
//                 {
//                     if (direction.x > 0)
//                         MoveRight();
//                     else
//                         MoveLeft();
//                 }
//                 else
//                 {
//                     if (direction.y > 0)
//                         Jump();
//                     else
//                         Roll();
//                 }
//             }
//             else
//             {
//                 if (isHoldingRifle && currentRifle != null)
//                 {
//                     currentRifle.Shoot();
//                 }
//             }
//         }
//     }
//
//     private void BetterJump()
//     {
//         if (rb.linearVelocity.y < 0)
//         {
//             rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
//         }
//         else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.UpArrow))
//         {
//             rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
//         }
//     }
//
//     private void StartLaneSwitch()
//     {
//         anim.SetTrigger(JUMP);
//
//         laneStartPosition = rb.position;
//         float targetX = laneXPositions[currentLane];
//         laneTargetPosition = new Vector3(targetX, rb.position.y, rb.position.z);
//
//         laneSwitchTime = 0f;
//         isSwitchingLane = true;
//     }
//
//     private void SwitchLane()
//     {
//         laneSwitchTime += Time.fixedDeltaTime;
//         float t = laneSwitchTime / laneJumpDuration;
//
//         if (t >= 1f)
//         {
//             horizontalVelocity = Vector3.zero;
//             isSwitchingLane = false;
//             return;
//         }
//
//         float newX = Mathf.Lerp(laneStartPosition.x, laneTargetPosition.x, t);
//         float newY = rb.position.y;
//         if (isGrounded && !isJumping)
//         {
//             float arc = Mathf.Sin(t * Mathf.PI) * laneJumpHeight;
//             newY = laneStartPosition.y + arc;
//         }
//
//         Vector3 newPosition = new Vector3(newX, newY, rb.position.z);
//         rb.MovePosition(newPosition);
//     }
// }


#region unwanted
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PlayerController : MonoBehaviour
//{

//    [SerializeField] private float moveSpeed = 5f;
//    [SerializeField] private float laneSwitchSpeed = 7f;
//    [SerializeField] private float laneOffset = 3f;
//    [SerializeField] private float laneJumpHeight = 2f;
//    [SerializeField] private float laneJumpDuration = 0.3f;
//    [SerializeField] private Animator anim;

//    [SerializeField] private Transform groundCheckPoint;
//    [SerializeField] private LayerMask groundLayerMask;
//    [SerializeField] private float jumpHeight;
//    [SerializeField] private float groundCheckRadius = 0.3f;
//    [SerializeField] private float fallMultiplier = 2.5f;
//    [SerializeField] private float lowJumpMultiplier = 2f;
//    private bool isGrounded = false;
//    private bool isJumping = false;
//    private bool isDead = false;

//    private Rigidbody rb;

//    private Vector3 forwardDirection;
//    private Vector3 laneTargetPosition;
//    private int currentLane = 1;
//    private Vector3 horizontalVelocity;

//    [SerializeField] private float[] laneXPositions = new float[3];
//    private bool isSwitchingLane = false;
//    private float laneSwitchTime = 0f;
//    private Vector3 laneStartPosition;


//    private Vector2 startTouchPosition;
//    private Vector2 endTouchPosition;
//    private bool isSwiping = false;
//    private float swipeAmount = 10f;

//    private const string IS_RUNNING = "IsRunning";
//    private const string JUMP = "Jump";
//    private const string ROLL = "Roll";
//    private const string DIE = "Die";

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

//        currentLane = 1;
//        forwardDirection = Vector3.forward;
//        laneTargetPosition = transform.position;
//    }

//    public void Restart()
//    {
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//        }
//    }
//    private void Update()
//    {
//        if (isDead)
//        {
//            Restart();
//        }

//        CheckIfGrounded();

//        if (isJumping)
//        {
//            if (rb.linearVelocity.y < 0)
//            {
//                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
//            }
//            else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.UpArrow))
//            {
//                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
//            }
//        }

//        if (!isSwitchingLane)
//        {
//            KeyboardInputMovement();
//            MouseInputMovement();
//        }
//    }

//    private void FixedUpdate()
//    {
//        MoveForward();

//        if (isSwitchingLane)
//        {
//            SwitchLane();
//        }

//    }

//    private void CheckIfGrounded()
//    {
//        //isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, groundCheckRadius + 0.1f, groundLayerMask);
//        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayerMask);

//        if (isGrounded && isJumping)
//        {
//            isJumping = false;
//            anim.SetBool(IS_RUNNING, true);
//        }
//    }

//    private void MoveForward()
//    {
//        anim.SetBool(IS_RUNNING, true);
//        Vector3 currentVelocity = rb.linearVelocity;
//        currentVelocity.z = moveSpeed;
//        rb.linearVelocity = currentVelocity;

//    }

//    private void KeyboardInputMovement()
//    {
//        if (Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            Roll();
//        }

//        if (Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            Jump();
//        }

//        if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            MoveLeft();
//        }

//        if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            MoveRight();
//        }


//    }

//    public void Die()
//    {
//        anim.SetTrigger(DIE);
//        isDead = true;
//        rb.linearVelocity = Vector3.zero;
//        rb.isKinematic = true;
//    }

//    private void MoveLeft()
//    {
//        if (currentLane > 0)
//        {
//            currentLane--;
//            StartLaneSwitch();
//        }
//    }

//    private void MoveRight()
//    {
//        if (currentLane < 2)
//        {
//            currentLane++;
//            StartLaneSwitch();
//        }
//    }

//    private void Roll()
//    {
//        anim.SetTrigger(ROLL);
//    }

//    private void Jump()
//    {

//        //if (!isJumping && (isGrounded || !isSwitchingLane))
//        if (isGrounded && !isJumping && !isSwitchingLane)
//        {
//            anim.SetBool(IS_RUNNING, false);
//            anim.SetTrigger(JUMP);

//            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
//            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
//            isJumping = true;
//        }
//    }

//    private void MouseInputMovement()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            startTouchPosition = Input.mousePosition;
//            isSwiping = true;
//        }

//        if (Input.GetMouseButtonUp(0) && isSwiping)
//        {
//            endTouchPosition = Input.mousePosition;
//            isSwiping = false;

//            Vector2 magPos = endTouchPosition - startTouchPosition;

//            if (magPos.magnitude > swipeAmount)
//            {
//                Vector2 direction = magPos.normalized;

//                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
//                {
//                    if (direction.x > 0)
//                    {
//                        MoveRight();
//                    }
//                    else
//                    {
//                        MoveLeft();
//                    }
//                }
//                else
//                {
//                    if (direction.y > 0)
//                    {
//                        Jump();
//                    }
//                    else
//                    {
//                        Roll();
//                    }
//                }
//            }
//        }
//    }

//    private void StartLaneSwitch()
//    {
//        anim.SetTrigger(JUMP);


//        laneStartPosition = rb.position;
//        //float targetX = (currentLane == 0) ? -laneOffset : laneOffset;
//        float targetX = laneXPositions[currentLane];
//        laneTargetPosition = new Vector3(targetX, rb.position.y, rb.position.z);

//        laneSwitchTime = 0f;
//        isSwitchingLane = true;
//    }

//    private void SwitchLane()
//    {
//        laneSwitchTime += Time.fixedDeltaTime;
//        float t = laneSwitchTime / laneJumpDuration;

//        if (t >= 1f)
//        {
//            //rb.MovePosition(new Vector3(laneTargetPosition.x, laneStartPosition.y, laneTargetPosition.z));
//            horizontalVelocity = Vector3.zero;
//            isSwitchingLane = false;
//            return;
//        }

//        float newX = Mathf.Lerp(laneStartPosition.x, laneTargetPosition.x, t);
//        float arc = Mathf.Sin(t * Mathf.PI) * laneJumpHeight;
//        float newY = laneStartPosition.y + arc;

//        Vector3 newPosition = new Vector3(newX, newY, rb.position.z);
//        rb.MovePosition(newPosition);
//    }
//}
#endregion



