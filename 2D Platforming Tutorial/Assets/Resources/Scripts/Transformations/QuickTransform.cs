using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTransform : ITransformations
{
    private PlayerController _player;
    private static int id = 1;
    private float quickSpeedMultiplier;
    private float quickLegMultiplier;

    public QuickTransform(PlayerController player)
    {
        _player = player;
    }

    public void TFixedUpdate()
    {
        switch (currentState)
        {
            case DashStates.ready:
                break;
            case DashStates.dashing:
                Dash();
                break;
            case DashStates.cooldown:
                dashCooldown -= Time.deltaTime;
                if(dashCooldown < 0f)
                {
                    dashCooldown = maxDashCooldown;
                    currentState = DashStates.ready;
                }
                break;
        }
    }

    public void TUpdate()
    {
        if(dashRecoilDuration > 0f)
        {
            dashRecoilDuration -= Time.deltaTime;
        }
        else
        {
            if (!_player.isDashing)
            {
                _player.canMove = true;
            }
        }
    }

    public int GetId()
    {
        return id;
    }

    public void OnEnter()
    {
        _player.MyAnim.runtimeAnimatorController = Resources.Load("Animations/PlayerQuick") as RuntimeAnimatorController;
        
        forceThrust = _player.dashForce;
        dashDuration = _player.dashDuration;
        dashRecoilMaxDuration = _player.dashRecoilMaxDuration;
        dashRecoilDuration = 0;
        maxDashCooldown = _player.maxDashCooldown;
        quickSpeedMultiplier = _player.quickSpeedMult;
        quickLegMultiplier = _player.quickLegMultiplier;
        dashCooldown = maxDashCooldown;

        _player.speedMultiplier = quickSpeedMultiplier;
        _player.legMultiplier = quickLegMultiplier;
        _player.currentAmountOfJumps = 0;
    }

    public void OnExit()
    {
        _player.speedMultiplier = 1;
        dashDuration = _player.dashDuration;
        _player.isDashing = false;
        currentState = DashStates.ready;
    }

    private enum DashStates
    {
        ready,
        dashing,
        cooldown
    }
    private DashStates currentState;

    private float dashDuration;

    private float dashCooldown;
    private float maxDashCooldown;
    private float dashRecoilMaxDuration;
    private float dashRecoilDuration;
    private bool startedRightDash;
    private bool startedLeftDash;
    private float forceThrust;

    public void SpecialSkill()
    {
        if(DashStates.ready == currentState)
        {
            GameObject.Instantiate(_player.dashParticles, _player.transform.position, _player.transform.rotation);
            startedLeftDash = false;
            startedRightDash = false;
            _player.isDashing = true;
            _player.MyRigidBody.gravityScale = 0;
            currentState = DashStates.dashing;
        }
        
    }

    private void Dash()
    {
        _player.MyRigidBody.velocity = new Vector2(_player.MyRigidBody.velocity.x, 0);
        
        if (_player.movingDirection > 0f && !startedLeftDash)
        {
            _player.canMove = false;
            startedRightDash = true;
            _player.MyRigidBody.velocity = new Vector2(_player.walkingSpeed * forceThrust, 0);
        }

        if (_player.movingDirection < 0f && !startedRightDash)
        {
            _player.canMove = false;
            startedLeftDash = true;
            _player.MyRigidBody.velocity = new Vector2(-_player.walkingSpeed * forceThrust, 0);
        }
        
        if (_player.transform.localScale.x > 0 && !startedLeftDash)
        {
            _player.canMove = false;
            startedRightDash = true;
            _player.MyRigidBody.velocity = new Vector2(_player.walkingSpeed * forceThrust, 0);
        }
        else if (!startedRightDash)
        {
            _player.canMove = false;
            startedLeftDash = true;
            _player.MyRigidBody.velocity = new Vector2(-_player.walkingSpeed * forceThrust, 0);
        }

        dashDuration -= Time.deltaTime;
        if (dashDuration < 0f || _player.collided)
        {
            _player.MyRigidBody.velocity = Vector2.zero;
            if (_player.collided)
            {
                if (_player.transform.localScale.x > 0f)
                {
                    _player.MyRigidBody.velocity = new Vector2(-_player.knockbackForce * 2, _player.knockbackForce * 2);
                }
                else
                {
                    _player.MyRigidBody.velocity = new Vector2(_player.knockbackForce * 2, _player.knockbackForce * 2);
                }

                dashRecoilDuration = dashRecoilMaxDuration;
            }
            else
            {
                _player.canMove = true;
            }

            _player.MyRigidBody.gravityScale = 4;
            _player.isDashing = false;
            startedLeftDash = false;
            startedRightDash = false;
            dashDuration = _player.dashDuration;
            currentState = DashStates.cooldown;
        }
    }

    public void Jump(float velocity)
    {
        throw new System.NotImplementedException();
    }
}
