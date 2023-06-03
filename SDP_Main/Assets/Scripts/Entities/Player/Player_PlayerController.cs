using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(CharacterController), typeof(Player_InputManager))]
public class Player_PlayerController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the main camera used for the player")]
    public Camera PlayerCamera;

    [Header("General")]
    [Tooltip("Force applied downward when in the air")]
    public float GravityForce = 20f;

    [Tooltip("Physics layer checked for to consider if the player is grounded")]
    public LayerMask GroundCheckLayer = -1;

    [Tooltip("distance from the bottom of the character controller capsule to test for grounded")]
    public float GroundCheckDistance = 0.05f;

    [Header("Movement")]
    [Tooltip("Max movement speed when grounded (when not sprinting)")]
    public float MaxSpeedOnGround = 10f;

    [Tooltip(
        "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
    public float MovementSharpnessOnGround = 15;

    [Tooltip("Max movement speed when crouching")]
    [Range(0, 1)]
    public float MaxSpeedCrouchedRatio = 0.5f;

    [Tooltip("Max movement speed when not grounded")]
    public float MaxSpeedInAir = 10f;

    [Tooltip("Acceleration speed when in the air")]
    public float AccelerationSpeedInAir = 25f;

    [Tooltip("Multiplicator for the sprint speed (based on grounded speed)")]
    public float SprintSpeedModifier = 3f;

    [Tooltip("Height at which the player dies instantly when falling off the map")]
    public float MaxNegativeY = -50f;

    [Header("Rotation")]
    [Tooltip("Rotation speed for moving the camera")]
    public float RotationSpeed = 200f;
    

    [Range(0.1f, 1f)]
    [Tooltip("Rotation speed multiplier when aiming")]
    public float AimingRotationMultiplier = 0.4f;

    [Header("Jump")]
    [Tooltip("Force applied upward when jumping")]
    public float JumpForce = 9f;

    [Header("Stance")]
    [Tooltip("Ratio (0-1) of the character height where the camera will be at")]
    public float CameraHeightRatio = 0.9f;

    [Tooltip("Height of character when standing")]
    public float CapsuleHeightStanding = 1.8f;

    [Tooltip("Height of character when crouching")]
    public float CapsuleHeightCrouching = 0.9f;

    [Tooltip("Speed of crouching transitions")]
    public float CrouchingSharpness = 10f;

    [Header("Fall Damage")]
    [Tooltip("Whether the player will recieve damage when hitting the ground at high speed")]
    public bool RecievesFallDamage;

    [Tooltip("Minimun fall speed for recieving fall damage")]
    public float MinSpeedForFallDamage = 10f;

    [Tooltip("Fall speed for recieving th emaximum amount of fall damage")]
    public float MaxSpeedForFallDamage = 30f;

    [Tooltip("Damage recieved when falling at the mimimum speed")]
    public float FallDamageAtMinSpeed = 10f;

    [Tooltip("Damage recieved when falling at the maximum speed")]
    public float FallDamageAtMaxSpeed = 50f;

    [Tooltip("Y position that causes the player to die, eg. falling off the map")]
    public float killHeight = -10;

    public UnityAction<bool> OnStanceChanged;
    public Vector3 CharacterVelocity { get; set; }
    public bool IsGrounded { get; private set; }
    public bool HasJumpedThisFrame { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsCrouching { get; private set; }

    public bool IsInputLocked;
    public PhotonView photonView;
    public Player_SoundManager soundManager;
    
    [SerializeField]private ScoreBoard _scoreBoard;
    //private List<Weapon> weapons = new List<Weapon>();
    //private Weapon currentWeapon = null;


    public float RotationMultiplier
    {
        get
        {
            //TODO: Slow rotaion when aiming
            /*if (m_WeaponsManager.IsAiming)
            {
                return AimingRotationMultiplier;
            }*/

            return 1f;
        }
    }
    private Weapon_ProjectileManager _projectMananger;
    Player_InputManager inputHandler;
    CharacterController controller;
    Vector3 m_GroundNormal;
    Vector3 m_CharacterVelocity;
    Vector3 m_LatestImpactSpeed;
    float m_LastTimeJumped = 0f;
    float m_CameraVerticalAngle = 0f;
    float m_FootstepDistanceCounter;
    float m_TargetCharacterHeight;

    const float k_JumpGroundingPreventionTime = 0.2f;
    const float k_GroundCheckDistanceInAir = 0.1f;

    void Awake()
    {
        if (Game_RuntimeData.isMultiplayer)
        {
            photonView = GetComponentInParent<PhotonView>();
            if (photonView == null)
                Debug.LogError("ERROR: Photon View is NULL for " + gameObject.name);
        }

        _projectMananger = GetComponentInParent<Weapon_ProjectileManager>();
        _scoreBoard = GetComponentInChildren<ScoreBoard>();
        soundManager = GetComponentInChildren<Player_SoundManager>();
        if (soundManager == null)
            Debug.LogError("ERROR: SoundManager is NULL for " + gameObject.name);
        //TODO: Add weapons
        /*
        Weapon[] myGuns = GetComponentsInChildren<Weapon>();
        if (myGuns.Length > 0)
        {
            for (int i = 0; i < myGuns.Length; i++)
                weapons.Add(myGuns[i]);

            currentWeapon = myGuns[0];
        }
        else
        {
            Debug.LogError("Player " + gameObject.name + " has no weapons!");
        }*.

        //TODO: Register as an actor in the scene
        /*ActorsManager actorsManager = FindObjectOfType<ActorsManager>();
        if (actorsManager != null)
            actorsManager.SetPlayer(gameObject);*/
    }

    void Start()
    {

        if (Game_RuntimeData.isMultiplayer && !photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }

        // fetch components on the same gameObject
        controller = GetComponent<CharacterController>();

        inputHandler = GetComponent<Player_InputManager>();
        _projectMananger = GetComponentInChildren<Weapon_ProjectileManager>();
        controller.enableOverlapRecovery = true;

        // force the crouch state to false when starting
        CanCrouchOrJump(false, true);
        UpdateCharacterHeight(true);
    }

    void Update()
    {
        if (Game_RuntimeData.isMultiplayer && !photonView.IsMine)
            return;

        if (IsInputLocked)
            return;

        // TODO: check for Y kill
        if (transform.position.y < killHeight)
        {
            controller.enabled = false;
            gameObject.transform.position = new Vector3(0, 30, 0);
            controller.enabled = true;

            s_DamageInfo d = new s_DamageInfo();
            d.dmgDealerTeam = -1;
            d.dmgDealerId = -1;
            d.dmgRecievedTeam = Game_RuntimeData.thisMachinesMultiplayerEntity.teamNumber;
            d.dmgRecievedId = photonView.Owner.ActorNumber;
            d.dmgValue = 101f;
            gameObject.GetComponent<Player_Health>().TakeDamage(d);
        }

        HasJumpedThisFrame = false;

        bool wasGrounded = IsGrounded;
        GroundCheck();

        // landing
        if (IsGrounded && !wasGrounded)
        {
            // Fall damage
            float fallSpeed = -Mathf.Min(CharacterVelocity.y, m_LatestImpactSpeed.y);
            float fallSpeedRatio = (fallSpeed - MinSpeedForFallDamage) /
                                    (MaxSpeedForFallDamage - MinSpeedForFallDamage);
            if (RecievesFallDamage && fallSpeedRatio > 0f)
            {
                float dmgFromFall = Mathf.Lerp(FallDamageAtMinSpeed, FallDamageAtMaxSpeed, fallSpeedRatio);
                //m_Health.TakeDamage(dmgFromFall, null);

                // fall damage SFX
                //AudioSource.PlayOneShot(FallDamageSfx);
            }
            else
            {
                // land SFX
                soundManager.PlayLand(); ;
            }
        }

        // crouching
        if (inputHandler.GetCrouchInputDown())
        {
            CanCrouchOrJump(!IsCrouching, false);
        }

        UpdateCharacterHeight(false);

        HandleCharacterMovement();

        // shooting
        if (inputHandler.GetFireInputDown())
        {
            _projectMananger.InitShoot(Weapon_Firetype.Semi);

           // _projectMananger.InitShoot(Weapon_Firetype.Semi);
        }
        //Reload
        if (inputHandler.GetReloadButtonDown())
        {
            if (!Game_RuntimeData.isMultiplayer)
            {
                _projectMananger.Reload();
                return;
            }
         //it needs pun reload ? lets check.
           _projectMananger.PhotonView.RPC(nameof(_projectMananger.RPC_Reload), RpcTarget.All);
          //  _projectMananger.Reload();
        }
        //checking Scoreboard
        if(Game_RuntimeData.isMultiplayer && inputHandler.GetScoreBoardInputDown()){
            _scoreBoard.GetScoreboard();
        }
    }
    
    void OnDie()
    {
        IsDead = true;

        //TODO: weapon
        // Tell the weapons manager to switch to a non-existing weapon in order to lower the weapon
        /*        m_WeaponsManager.SwitchToWeaponIndex(-1, true);

                EventManager.Broadcast(Events.PlayerDeathEvent);*/
    }

    void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
        float chosenGroundCheckDistance =
            IsGrounded ? (controller.skinWidth + GroundCheckDistance) : k_GroundCheckDistanceInAir;

        // reset values before the ground check
        IsGrounded = false;
        m_GroundNormal = Vector3.up;

        // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(controller.height),
                controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, GroundCheckLayer,
                QueryTriggerInteraction.Ignore))
            {
                // storing the upward direction for the surface found
                m_GroundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(m_GroundNormal))
                {
                    IsGrounded = true;

                    // handle snapping to the ground
                    if (hit.distance > controller.skinWidth)
                    {
                        controller.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    void HandleCharacterMovement()
    {
        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(
                new Vector3(0f, (inputHandler.GetLookInputsHorizontal() * RotationSpeed * RotationMultiplier),
                    0f), Space.Self);
        }

        // vertical camera rotation
        {
            // add vertical inputs to the camera's vertical angle
            m_CameraVerticalAngle += inputHandler.GetLookInputsVertical() * RotationSpeed * RotationMultiplier;

            // limit the camera's vertical angle to min/max
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            PlayerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
        }

        // character movement handling
        bool isSprinting = inputHandler.GetSprintInputHeld();
        {
            if (isSprinting)
            {
                isSprinting = CanCrouchOrJump(false, false);
            }

            float speedModifier = isSprinting ? SprintSpeedModifier : 1f;

            // converts move input to a worldspace vector based on our character's transform orientation
            Vector3 worldspaceMoveInput = transform.TransformVector(inputHandler.GetMoveInput());

            // handle grounded movement
            if (IsGrounded)
            {
                // calculate the desired velocity from inputs, max speed, and current slope
                Vector3 targetVelocity = worldspaceMoveInput * MaxSpeedOnGround * speedModifier;
                // reduce speed if crouching by crouch speed ratio
                if (IsCrouching)
                    targetVelocity *= MaxSpeedCrouchedRatio;
                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) *
                                    targetVelocity.magnitude;

                // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
                CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity,
                    MovementSharpnessOnGround * Time.deltaTime);

                // jumping
                if (IsGrounded && inputHandler.IsJumping())
                {
                    // force the crouch state to false
                    if (CanCrouchOrJump(false, false))
                    {
                        // start by canceling out the vertical component of our velocity
                        CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);

                        // then, add the jumpSpeed value upwards
                        CharacterVelocity += Vector3.up * JumpForce;

                        // play sound
                        //AudioSource.PlayOneShot(JumpSfx);

                        // remember last time we jumped because we need to prevent snapping to ground for a short time
                        m_LastTimeJumped = Time.time;
                        HasJumpedThisFrame = true;

                        // Force grounding to false
                        IsGrounded = false;
                        m_GroundNormal = Vector3.up;
                    }
                }

                // footsteps sound
                soundManager.PlayFootstep(isSprinting, CharacterVelocity.magnitude);
            }
            // handle air movement
            else
            {
                // add air acceleration
                CharacterVelocity += worldspaceMoveInput * AccelerationSpeedInAir * Time.deltaTime;

                // limit air speed to a maximum, but only horizontally
                float verticalVelocity = CharacterVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, MaxSpeedInAir * speedModifier);
                CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                // apply the gravity to the velocity
                CharacterVelocity += Vector3.down * GravityForce * Time.deltaTime;
            }
        }

        // apply the final calculated velocity value as a character movement
        Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
        Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(controller.height);
        controller.Move(CharacterVelocity * Time.deltaTime);

        // detect obstructions to adjust velocity accordingly
        m_LatestImpactSpeed = Vector3.zero;
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, controller.radius,
            CharacterVelocity.normalized, out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime, -1,
            QueryTriggerInteraction.Ignore))
        {
            // We remember the last impact speed because the fall damage logic might need it
            m_LatestImpactSpeed = CharacterVelocity;

            CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
        }
    }

    // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= controller.slopeLimit;
    }

    // Gets the center point of the bottom hemisphere of the character controller capsule
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * controller.radius);
    }

    // Gets the center point of the top hemisphere of the character controller capsule
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - controller.radius));
    }

    // Gets a reoriented direction that is tangent to a given slope
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }

    //TODO: Actor Transform
    void UpdateCharacterHeight(bool force)
    {
        // Update height instantly
        if (force)
        {
            controller.height = m_TargetCharacterHeight;
            controller.center = Vector3.up * controller.height * 0.5f;
            PlayerCamera.transform.localPosition = Vector3.up * m_TargetCharacterHeight * CameraHeightRatio;
            //m_Actor.AimPoint.transform.localPosition = controller.center;
        }
        // Update smooth height
        else if (controller.height != m_TargetCharacterHeight)
        {
            // resize the capsule and adjust camera position
            controller.height = Mathf.Lerp(controller.height, m_TargetCharacterHeight,
                CrouchingSharpness * Time.deltaTime);
            controller.center = Vector3.up * controller.height * 0.5f;
            PlayerCamera.transform.localPosition = Vector3.Lerp(PlayerCamera.transform.localPosition,
                Vector3.up * m_TargetCharacterHeight * CameraHeightRatio, CrouchingSharpness * Time.deltaTime);
            //m_Actor.AimPoint.transform.localPosition = controller.center;
        }
    }

    // Checks for collisions in a radius
    bool CanCrouchOrJump(bool crouched, bool ignoreObstructions)
    {
        // set appropriate heights
        if (crouched)
        {
            m_TargetCharacterHeight = CapsuleHeightCrouching;
        }
        else
        {
            if (!ignoreObstructions)
            {
                Collider[] standingOverlaps = Physics.OverlapCapsule(
                    GetCapsuleBottomHemisphere(),
                    GetCapsuleTopHemisphere(CapsuleHeightStanding),
                    controller.radius,
                    -1,
                    QueryTriggerInteraction.Ignore);
                foreach (Collider c in standingOverlaps)
                {
                    if (c != controller && c.tag != "Player_Model")
                    {
                        return false;
                    }
                }
            }

            m_TargetCharacterHeight = CapsuleHeightStanding;
        }

        if (OnStanceChanged != null)
        {
            OnStanceChanged.Invoke(crouched);
        }

        IsCrouching = crouched;
        return true;
    }
}