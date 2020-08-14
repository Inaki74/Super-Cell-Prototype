using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class JumpyTransform : ITransformations
{
    private PlayerController _player;
    private static int id = 2;
    private int amountOfJumps;
    private float jumpySpeedMultiplier;
    private float jumpyLegMultiplier;

    public JumpyTransform(PlayerController player)
    {
        _player = player;
    }

    public void TUpdate()
    {

    }

    public void TFixedUpdate()
    {
        switch (currentState)
        {
            case FloatStates.ready:
                break;
            case FloatStates.floating:
                Float();
                break;
            case FloatStates.cooldown:
                floatCooldown -= Time.deltaTime;
                if(floatCooldown < 0f)
                {
                    floatCooldown = maxFloatCooldown;
                    currentState = FloatStates.ready;
                }
                break;
        }
    }

    public int GetId()
    {
        return id;
    }

    public void OnEnter()
    {
        _player.MyAnim.runtimeAnimatorController = Resources.Load("Animations/PlayerJumpy") as RuntimeAnimatorController;

        floatDuration = _player.floatDuration;
        maxFloatCooldown = _player.maxFloatCooldown;
        floatStrength = _player.floatStrength;
        jumpyLegMultiplier = _player.jumpyLegMultiplier;
        amountOfJumps = _player.jumpyAmountOfJumps;
        jumpySpeedMultiplier = _player.jumpySpeedMult;

        _player.speedMultiplier = jumpySpeedMultiplier;
        _player.legMultiplier = jumpyLegMultiplier;
        _player.currentAmountOfJumps = 2;
        _player.maxAmountOfJumps = amountOfJumps;
    }

    public void OnExit()
    {
        _player.maxAmountOfJumps = 1;
        _player.MyRigidBody.drag = 1;
        floatCooldown = maxFloatCooldown;
        currentState = FloatStates.ready;
    }

    private enum FloatStates
    {
        ready,
        floating,
        cooldown
    }
    private FloatStates currentState;

    private float floatDuration;
    private float floatCooldown;
    private float maxFloatCooldown;
    private float floatStrength;

    public void Jump(float velocity)
    {
        // Base height of jump (tap)
        _player.MyRigidBody.velocity = new Vector2(_player.MyRigidBody.velocity.x, velocity);
        _player.jumpSound.Play();
        _player.currentAmountOfJumps--;
    }

    public void SpecialSkill()
    {
        if (FloatStates.ready == currentState)
        {
            GameObject.Instantiate(_player.dashParticles, _player.transform.position, _player.transform.rotation);
            _player.legMultiplier = 1.2f;
            currentState = FloatStates.floating;
        }

    }

    private void Float()
    {
        if (Input.GetKey(KeyCode.Z) || CrossPlatformInputManager.GetButton(Constants.SPECIAL_BUTTON))
        {
            _player.MyRigidBody.drag = floatStrength;
        }
        else
        {
            _player.MyRigidBody.drag = 1;
            floatDuration = 0;
        }

        floatDuration -= Time.deltaTime;
        if (floatDuration < 0f || _player.currentGrdState == PlayerController.GroundStates.grounded)
        {
            floatDuration = _player.floatDuration;
            _player.MyRigidBody.drag = 1;
            floatCooldown = maxFloatCooldown;
            currentState = FloatStates.cooldown;
        }
    }
}
