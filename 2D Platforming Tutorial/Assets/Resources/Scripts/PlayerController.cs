using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    //Publics
    [Space(10)]
    [Header("Basic Variables")]
    public float walkingSpeed;
    public float duckingSpeed;
    public float speedMultiplier;
    public float movingDirection;
    public float legStrength;
    public float legMultiplier;
    public float platformSpeedModifier;
    public float groundCheckRadius;
    public float wallCheckDistance = 0.7f;
    public float knockbackForce;
    public float knockbackLength;
    public float invincibilityLength;
    [Space (10)]
    [Header("Normal Variables")]
    public float normalSpeedMultiplier;
    public float normalLegMultiplier;
    public float healDuration;
    public float maxHealCooldown;
    public int healCharges;
    public int healAmount;
    [Space(10)]
    [Header("Quick Variables")]
    public bool quickUnlocked;
    public float quickSpeedMult;
    public float quickLegMultiplier;
    public float dashForce;
    public float dashDuration;
    public float dashRecoilMaxDuration;
    public float maxDashCooldown;
    public bool isDashing = false;
    public bool collided;
    [Space(10)]
    [Header("Jumpy Variables")]
    public bool jumpyUnlocked;
    public int jumpyAmountOfJumps;
    public float jumpySpeedMult;
    public float jumpyLegMultiplier;
    public float floatStrength;
    public float maxFloatCooldown;
    public float floatDuration;
    [Space(10)]
    [Header("Sticky Variables")]
    public bool stickyUnlocked;
    public float stickySpeedMultiplier;
    public float stickyLegMultiplier;
    public float startingWallSlidingSpeed;
    public float maxWallSlidingSpeed;
    public float wallJumpStrength;
    public GameObject anchorGj;
    [Space(10)]

    public int currentAmountOfJumps;
    public int maxAmountOfJumps;

    public bool onPlatform;
    public bool invincible;
    public bool levelEnding;
    public bool canMove;
    public bool onWall;
    public bool isStatic;

    public string STransformation;

    public ParticleSystem dashParticles;
    public Rigidbody2D MyRigidBody { get; set; }
    public Transform groundCheckOne;
    public Transform groundCheckTwo;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlatform;
    public Animator MyAnim { get; set; }
    public Vector2 checkpoint;
    public Vector2 fallingCheckpoint;
    public GameObject stompBox;
    public AudioSource jumpSound;
    public AudioSource hurtSound;
    public LevelManager levelManager;
    public GroundStates currentGrdState;
    public enum GroundStates
    {
        grounded,
        airborne,
        ducking
    }

    //Privates
    private float _currentSpeed;
    private float _knockbackCounter;
    private float _invincibilityCounter;
    private float _knockbackDamagerCenter;
    private float _knockbackPlayerCenter;

    private ITransformations _currentTransformation;
    public ITransformations CurrentTransformation
    {
        get
        {
            STransformation = _currentTransformation.ToString();
            return _currentTransformation;
        }
        set
        {
            _currentTransformation = value;
        }
    }
    public List<ITransformations> transformationPool = new List<ITransformations>();

    // Start is called before the first frame update
    void Start()
    {
        MyRigidBody = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        levelManager = FindObjectOfType<LevelManager>();
        GetComponent<DistanceJoint2D>().enabled = false;
        InstantiateTransformations();

        levelManager.onEnd += levelEnd;

        canMove = true;
        levelEnding = false;
        onWall = false;
        checkpoint = transform.position;
        CurrentTransformation = getTransformations(0);
        currentAmountOfJumps = 0;
        speedMultiplier = normalSpeedMultiplier;
        legMultiplier = normalLegMultiplier;
        STransformation = "NormalTransform";
    }

    void Update()
    {
        CheckIsGrounded();
        CheckFalling();
        CheckOnWall();

        if (!levelEnding && !PauseMenu0.paused)
        {
            if (_knockbackCounter <= 0)
            {
                InputHandler();
                
            }
            else
            {
                knockedBack();
            }
        }

        if (_invincibilityCounter > 0f)
        {
            _invincibilityCounter -= Time.deltaTime;


            if (!playInvisAnimOnce)
            {
                StartCoroutine(AnimateInvincibleCo());
                playInvisAnimOnce = true;
            }
            
        }
        else
        {
            invincible = false;
            playInvisAnimOnce = false;
        }

        SetAnimations();
        _currentTransformation.TUpdate();
    }

    private bool playInvisAnimOnce = false;
    private bool jumped = false;
    private bool specialSkill = false;

    private void FixedUpdate()
    {
        if (!isStatic && canMove)
            Move();

        if (jumped)
        {
            Jump(legStrength * legMultiplier);
            jumped = false;
        }

        if (specialSkill)
        {
            _currentTransformation.SpecialSkill();
            specialSkill = false;
        }

        _currentTransformation.TFixedUpdate();
    }

    private void InputHandler()
    {
        if (canMove)
        {
            

            if (Input.GetKeyDown(KeyCode.X) || CrossPlatformInputManager.GetButtonDown(Constants.JUMP_BUTTON)) jumped = true;
            else if (Input.GetKey(KeyCode.DownArrow) || CrossPlatformInputManager.GetAxis(Constants.VERTICAL) < 0) Duck();

            if (Input.GetKeyUp(KeyCode.DownArrow) || CrossPlatformInputManager.GetAxis(Constants.VERTICAL) > 0)
            {
                GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.85f);
                GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.06f);
            }

            if ((Input.GetKey(KeyCode.X) || CrossPlatformInputManager.GetButton(Constants.JUMP_BUTTON)) && MyRigidBody.velocity.y > 0f) MyRigidBody.gravityScale = 1.3f;
            else MyRigidBody.gravityScale = 4;


            if (Input.GetKeyDown(KeyCode.Z) || CrossPlatformInputManager.GetButtonDown(Constants.SPECIAL_BUTTON))
                specialSkill = true;

            movingDirection = Input.GetAxisRaw("Horizontal");
            if(movingDirection == 0)
            {
                movingDirection = CrossPlatformInputManager.GetAxis(Constants.HORIZONTAL);
            }
        }
        
    }

    // Jumps if its not airborne
    private void Jump(float velocity)
    {
        if (STransformation == "JumpyTransform")
        {
            if (currentAmountOfJumps > 0)
            {
                _currentTransformation.Jump(velocity);
            }
        }
        else if (STransformation == "StickyTransform")
        {
            _currentTransformation.Jump(velocity);
        }
        else if(currentGrdState == GroundStates.grounded)
        {
            MyRigidBody.velocity = new Vector2(MyRigidBody.velocity.x, velocity);
            jumpSound.Play();         
        }
    }

    // Sets animations based on states
    private void SetAnimations()
    {
        MyAnim.SetFloat("Speed", Mathf.Abs(_currentSpeed));
        MyAnim.SetBool("Grounded", currentGrdState == GroundStates.grounded);
        MyAnim.SetBool("Ducking", currentGrdState == GroundStates.ducking);
    }

    //Move right or left.
    private void Move()
    {
        if (movingDirection > 0f)
        {
            CheckSpeed();

            MyRigidBody.velocity = new Vector2(_currentSpeed * speedMultiplier, MyRigidBody.velocity.y);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (movingDirection < 0f)
        {
            CheckSpeed();

            MyRigidBody.velocity = new Vector2(-_currentSpeed * speedMultiplier, MyRigidBody.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            _currentSpeed = 0;
            
                MyRigidBody.velocity = new Vector2(0, MyRigidBody.velocity.y);
        }
    }

    // Checks speed (dashing, walking or ducking)
    private void CheckSpeed()
    {
        float decidedSpeed = 0;
        if (currentGrdState == GroundStates.ducking)
        {
            decidedSpeed = duckingSpeed;
        }
        else
        {
            decidedSpeed = walkingSpeed;
        }

        if (onPlatform)
        {
            _currentSpeed = decidedSpeed * platformSpeedModifier;
        }
        else
        {
            _currentSpeed = decidedSpeed;
        }
    }

    // Updates if its grounded.
    private void CheckIsGrounded()
    {
        // Im grounded if my feet overlap something that is ground
        if (Physics2D.OverlapCircle(groundCheckOne.position, groundCheckRadius, whatIsGround) || Physics2D.OverlapCircle(groundCheckTwo.position, groundCheckRadius, whatIsGround))
        {
            currentAmountOfJumps = maxAmountOfJumps;
            currentGrdState = GroundStates.grounded;
        }
        else
        {
            CheckIsOnPlatform();
        }
    }

    // Updates if its grounded.
    private void CheckIsOnPlatform()
    {
        // Im grounded if my feet overlap something that is ground
        if (Physics2D.OverlapCircle(groundCheckOne.position, groundCheckRadius, whatIsPlatform) || Physics2D.OverlapCircle(groundCheckTwo.position, groundCheckRadius, whatIsGround))
        {
            currentAmountOfJumps = maxAmountOfJumps;
            currentGrdState = GroundStates.grounded;

        }
        else
        {
            currentGrdState = GroundStates.airborne;
        }
    }

    private void CheckFalling()
    {
        if(MyRigidBody.velocity.y < 0f)
        {
            stompBox.SetActive(true);
        }
        else
        {
            stompBox.SetActive(false);
        }
    }

    //Better to do a Raycast so you have a bigger window of wall jumping
    private void CheckOnWall()
    {
        //onWall = Physics2D.OverlapCircle(GetComponent<BoxCollider2D>().bounds.center + new Vector3(0.25f, 0, 0), groundCheckRadius, whatIsGround);
        onWall = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallCheckDistance, whatIsGround);
    }

    // Ducks down
    private void Duck()
    {
        // I can only duck if Im grounded and im pressing down.
        if (currentGrdState != GroundStates.airborne)
        {
            currentGrdState = GroundStates.ducking;
            GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.6f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.175f);
        }
    }

    public void Knockback(float damagerCenterX, float playerCenterX)
    {
         invincible = true;
         _invincibilityCounter = invincibilityLength;
         _knockbackCounter = knockbackLength;

         _knockbackDamagerCenter = damagerCenterX;
         _knockbackPlayerCenter = playerCenterX;
    }

    private void knockedBack()
    {
        _knockbackCounter -= Time.deltaTime;
        if (_knockbackPlayerCenter > _knockbackDamagerCenter)
        {
            MyRigidBody.velocity = new Vector2(knockbackForce, knockbackForce);
        }
        else
        {
            MyRigidBody.velocity = new Vector2(-knockbackForce, knockbackForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "KillPlane")
        {
            levelManager.currentHealth -= 1;
            levelManager.updateHeartUI();
            levelManager.Respawn();
        }
        if(collision.tag == "Checkpoint")
        {
            SetCheckpoint(collision);
        }
        if (collision.tag == "FallingCheckpoint")
        {
            SetFallingCheckpoint(collision);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MovingPlatform")
        {
            onPlatform = true;
            transform.parent = collision.transform;
        }

        if((collision.gameObject.tag == "BreakableWall" || collision.gameObject.tag == "Enemy") && isDashing)
        {
            collided = true;
            if (collision.gameObject.tag == "BreakableWall")
            {
                collision.gameObject.SetActive(false);
            }
        }else collided = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "BreakableWall" || collision.gameObject.tag == "Enemy") && isDashing)
        {
            collided = true;
            if (collision.gameObject.tag == "BreakableWall")
            {
                collision.gameObject.SetActive(false);
            }
        }
        else collided = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MovingPlatform")
        {
            onPlatform = false;
            transform.parent = null;
        }
    }

    private void SetCheckpoint(Collider2D collision)
    {
        checkpoint = collision.transform.position;
    }

    private void SetFallingCheckpoint(Collider2D collision)
    {
        fallingCheckpoint = collision.GetComponent<FallenCheckpoint>().checkpoint.transform.position;
    }

    public void levelEnd()
    {
        levelEnding = true;
        _invincibilityCounter = 1f;

        _currentSpeed = 0;
        MyRigidBody.velocity = Vector2.zero;

        StartCoroutine("LevelExitCo");

    }

    private IEnumerator LevelExitCo()
    {
        yield return new WaitForSeconds(1.0f);
        _currentSpeed = walkingSpeed;
        MyRigidBody.velocity = new Vector2(_currentSpeed, MyRigidBody.velocity.y);
    }

    private IEnumerator AnimateInvincibleCo()
    {
        var tempColor = GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < 8; i++)
        {
            //alpha 0
            tempColor.a = 0f;
            GetComponent<SpriteRenderer>().color = tempColor;
            yield return new WaitForSeconds(0.05f);
            //alpha1
            tempColor.a = 1f;
            GetComponent<SpriteRenderer>().color = tempColor;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public Vector2 showVector1;
    public Vector2 showVector2;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector2(transform.localScale.x, 1f) * 3.7f);
        Gizmos.DrawRay(transform.position, Vector2.right * (transform.localScale.x * wallCheckDistance));
        Gizmos.DrawRay(transform.position, showVector1 * 10);
        Gizmos.DrawRay(anchorGj.transform.position, showVector2 * 10);
    }

    /// <trans>
    public ITransformations getTransformations(int id)
    {
        foreach(ITransformations trans in transformationPool)
        {
            if(id == trans.GetId())
            {
                return trans;
            }
        }

        return new NormalTransform(this, levelManager);
    }

    public void InstantiateTransformations()
    {
        transformationPool.Add(new NormalTransform(this, levelManager));
        transformationPool.Add(new QuickTransform(this));
        transformationPool.Add(new JumpyTransform(this));
        transformationPool.Add(new StickyTransform(this));
    }

    public void ExitTransformation(int state)
    {
        CurrentTransformation.OnExit();
        CurrentTransformation = getTransformations(state);
        CurrentTransformation.OnEnter();
    }
    /// </trans>

}
