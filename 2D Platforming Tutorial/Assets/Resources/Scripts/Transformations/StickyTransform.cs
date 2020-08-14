using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyTransform : ITransformations
{
    private PlayerController _player;
    private static int id = 3;
    private float stickySpeedMultiplier;
    private float stickyLegMultiplier;
    private float currentWallSlidingSpeed;
    private float startingWallSlidingSpeed;
    private float maxWallSlidingSpeed;
    private bool isWallSliding;
    private Vector2 jumpDirection;
    private float wallJumpStrength;

    public StickyTransform(PlayerController player)
    {
        _player = player;
    }

    public void TUpdate()
    {

    }

    public void TFixedUpdate()
    {

        switch (currentHookState)
        {
            case HookStates.ready:
                /*if (_player.onWall && _player.currentGrdState != PlayerController.GroundStates.grounded)
                {
                    if (Input.GetKey(KeyCode.X))
                    {
                        _player.MyRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                        _player.isStatic = true;
                    }
                    else
                    {
                        _player.MyRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                        _player.isStatic = false;

                        if (_player.MyRigidBody.velocity.y < 0)
                            isWallSliding = true;
                    }

                }
                else
                {
                    _player.MyRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    isWallSliding = false;
                    _player.isStatic = false;
                }*/
                if (_player.onWall && _player.currentGrdState != PlayerController.GroundStates.grounded && _player.MyRigidBody.velocity.y < 0)
                {
                    isWallSliding = true;
                }
                else
                {
                    isWallSliding = false;
                }

                if (isWallSliding)
                {
                    if(currentWallSlidingSpeed < maxWallSlidingSpeed)
                    {
                        currentWallSlidingSpeed += 0.1f;
                    }
                    WallSlide();
                }
                else
                {
                    currentWallSlidingSpeed = startingWallSlidingSpeed;
                }
                break;
            case HookStates.swinging:
                // Make line follow player
                UpdateLinePosition();

                // If the distance isnt set, set it
                if (!distanceSet)
                {
                    dj.distance = Vector2.Distance(_player.transform.position, _player.anchorGj.transform.position);
                    distanceSet = true;
                }

                // Make the vector follow the player
                lineVector = SetVectorPosition(_player.transform.position, _player.anchorGj.transform.position);

                // Gizmo
                _player.showVector1 = lineVector;

                // Apply a force perpendicular to the lineVector
                ApplyPerpendicularForce(CalculatePerpendicular(lineVector), CalculateForce(dj.distance, swingTime));

                // Subtract to the time swinging
                if (Vector2.Angle(startVector, lineVector) > 90f || _player.currentGrdState == PlayerController.GroundStates.grounded || _player.onWall)
                {
                    //_player.MyRigidBody.velocity = new Vector2(_player.MyRigidBody.velocity.x + startMomentumX, _player.MyRigidBody.velocity.y + startMomentumY/2);
                    _player.canMove = true;
                    _player.anchorGj.SetActive(false);
                    distanceSet = false;
                    dj.enabled = false;
                    _player.MyRigidBody.gravityScale = 4.0f;
                    currentHookState = HookStates.ready;
                    // Derender line
                    DerenderLine();
                }
                break;
        }
    }

    private void WallSlide()
    {
        // 0.7 - - -> 2.0
        if(_player.MyRigidBody.velocity.y < -currentWallSlidingSpeed)
        {
            _player.MyRigidBody.velocity = new Vector2(_player.MyRigidBody.velocity.x, -currentWallSlidingSpeed);
        }
    }

    public void Jump(float velocity)
    {
        //If the player is not on the wall its a normal jump
        //If its on the wall it should jump strictly to the left or right (depending on localScale) and remove its constraints
        Vector2 force;
        if (_player.onWall && _player.movingDirection != 0 && _player.currentGrdState != PlayerController.GroundStates.grounded)
        {
            force = new Vector2(jumpDirection.x * wallJumpStrength * -_player.transform.localScale.x, jumpDirection.y * wallJumpStrength);
            if (!ThereIsAWall(new Vector2(-force.x, force.y)))
            {
                _player.onWall = false;
                isWallSliding = false;
                _player.transform.localScale = new Vector2(-_player.transform.localScale.x, 1);
                _player.MyRigidBody.velocity = force;
                _player.jumpSound.Play();
            }
        }
        else if (_player.onWall && _player.currentGrdState != PlayerController.GroundStates.grounded)
        {
            force = new Vector2(jumpDirection.x * wallJumpStrength * -_player.transform.localScale.x, jumpDirection.y * wallJumpStrength);
            
            _player.onWall = false;
            isWallSliding = false;
            _player.transform.localScale = new Vector2(-_player.transform.localScale.x, 1);
            _player.MyRigidBody.velocity = force;
            _player.jumpSound.Play();
        }
        else if(_player.currentAmountOfJumps > 0)
            {
                _player.MyRigidBody.velocity = new Vector2(_player.MyRigidBody.velocity.x, velocity);
                _player.currentAmountOfJumps--;
                _player.jumpSound.Play();
            }
    }

    private bool ThereIsAWall(Vector2 direction)
    {
        return Physics2D.Raycast(_player.transform.position, direction, 1f,_player.whatIsGround).collider != null;
    }

    public int GetId()
    {
        return id;
    }

    public void OnEnter()
    {
        _player.MyAnim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/PlayerSticky");

        stickyLegMultiplier = _player.stickyLegMultiplier;
        stickySpeedMultiplier = _player.stickySpeedMultiplier;
        startingWallSlidingSpeed = _player.startingWallSlidingSpeed;
        maxWallSlidingSpeed = _player.maxWallSlidingSpeed;
        wallJumpStrength = _player.wallJumpStrength;

        currentWallSlidingSpeed = startingWallSlidingSpeed;

        _player.speedMultiplier = stickySpeedMultiplier;
        _player.legMultiplier = stickyLegMultiplier;
        _player.currentAmountOfJumps = 0;

        jumpDirection = new Vector2(1f, 1f);
        jumpDirection.Normalize();

        lr = _player.GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        dj = _player.GetComponent<DistanceJoint2D>();
        dj.enabled = false;
    }

    public void OnExit()
    {
        //_player.MyRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        isWallSliding = false;
        currentWallSlidingSpeed = startingWallSlidingSpeed;
        dj.enabled = false;
        _player.MyRigidBody.gravityScale = 4.0f;
        _player.canMove = true;
        _player.isStatic = false;
        _player.anchorGj.SetActive(false);
        distanceSet = false;
        currentHookState = HookStates.ready;
        // Derender line
        DerenderLine();
    }

    private HookStates currentHookState;
    private enum HookStates
    {
        ready,
        swinging
    }

    private LineRenderer lr;
    private DistanceJoint2D dj;
    private float swingTime = 0.5f;
    private Vector2 lineVector;
    private Vector2 startVector;
    private float startMomentumX;
    private float startMomentumY;
    private bool distanceSet = false;


    public void SpecialSkill()
    {
        RaycastHit2D hit = Physics2D.Raycast(_player.transform.position, new Vector2( _player.transform.localScale.x, 1f), 5f,_player.whatIsGround);
        if (hit)
        {
            startMomentumX = _player.MyRigidBody.velocity.x * _player.transform.localScale.x;

            if (-_player.MyRigidBody.velocity.y > 0f)
                startMomentumY = -_player.MyRigidBody.velocity.y;
            else startMomentumY = 0;

            currentHookState = HookStates.swinging;
            _player.MyRigidBody.gravityScale = 0;

            _player.canMove = false;
            _player.anchorGj.SetActive(true);
            _player.anchorGj.transform.position = hit.point;

            SetInitialLinePositions(hit.point);
            lineVector = startVector = SetVectorPosition(_player.transform.position, _player.anchorGj.transform.position);

            dj.enabled = true;
            dj.connectedBody = _player.anchorGj.GetComponent<Rigidbody2D>();
            dj.distance = Vector2.Distance(_player.transform.position, _player.anchorGj.transform.position);
        }
    }

    private void SetInitialLinePositions(Vector2 intersection) {
        lr.SetPosition(0, _player.transform.position);
        lr.SetPosition(1, intersection);
    }

    private Vector2 SetVectorPosition(Vector2 i, Vector2 f)
    {
        return new Vector2(f.x - i.x, f.y - i.y);
    }

    private void UpdateLinePosition()
    {
        lr.SetPosition(0, _player.transform.position);
    }

    private void DerenderLine()
    {
        lr.SetPosition(0, _player.transform.position);
        lr.SetPosition(1, _player.transform.position);
    }

    private void ApplyPerpendicularForce(Vector2 direction, float force)
    {
        _player.MyRigidBody.velocity = direction * force;
    }

    public Vector2 CalculatePerpendicular(Vector2 v1)
    {
        float x1 = v1.x;
        float y1 = v1.y;

        // Dot Product: x1 * x2 + y1 * y2 = 0, let x2 = 1 => y2 = -x1 * 1 / y1
        float x2 = 1;
        if (_player.transform.localScale.x < 0) x2 *= -1;
            
        float y2 = -(x1 * x2) / y1;

        return new Vector2(x2, y2);
    }

    private float CalculateForce(float distance, float time)
    {
        return distance * Mathf.PI / (2 * time);
    }
    
}
