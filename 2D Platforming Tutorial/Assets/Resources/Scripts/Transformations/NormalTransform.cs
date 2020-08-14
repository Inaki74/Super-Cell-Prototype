using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class NormalTransform : ITransformations
{
    private PlayerController _player;
    private Debugger _debugger;
    private LevelManager _levelManager;
    private static int id = 0;
    private float normalSpeedMultiplier;
    private float normalLegMultiplier;

    public NormalTransform(PlayerController player, LevelManager levelManager){
        _player = player;
        _levelManager = levelManager;
        healAmount = _player.healAmount;
        _debugger = _player.GetComponent<Debugger>();
    }

    public void TUpdate()
    {

    }

    public void TFixedUpdate()
    {
        _debugger.updateDuration(currentHealDuration);
        _debugger.updateCooldown(healCooldown);
        switch (currentState)
        {
            case HealStates.standby:
                break;
            case HealStates.ready:
                Heal();
                break;
            case HealStates.cooldown:
                healCooldown -= Time.deltaTime;
                if (healCooldown < 0f)
                {
                    healCooldown = maxHealCooldown;
                    if (Input.GetKey(KeyCode.Z)) currentState = HealStates.ready;
                    else currentState = HealStates.standby;

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
        _player.MyAnim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Animations/Player");
        _debugger = _player.GetComponent<Debugger>();

        healDuration = currentHealDuration = _player.healDuration;
        maxHealCooldown = healCooldown = _player.maxHealCooldown;
        healAmount = _player.healAmount;
        normalLegMultiplier = _player.normalLegMultiplier;
        normalSpeedMultiplier = _player.normalSpeedMultiplier;

        _player.currentAmountOfJumps = 0;
        _player.speedMultiplier = normalSpeedMultiplier;
        _player.legMultiplier = normalLegMultiplier;
    }

    public void OnExit()
    {
        Debug.Log("Exiting Normal Transformation");
    }
    private enum HealStates
    {
        standby,
        ready,
        cooldown
    }
    private HealStates currentState;

    private float healDuration;
    private float currentHealDuration;
    private int healAmount;
    private float healCooldown;
    private float maxHealCooldown;

    public void SpecialSkill()
    {
        if (HealStates.standby == currentState && _player.currentGrdState == PlayerController.GroundStates.grounded && _levelManager.currentHealth < 6 && _player.healCharges > 0)
        {
            GameObject.Instantiate(_player.dashParticles, _player.transform.position, _player.transform.rotation);

            currentState = HealStates.ready;
        }

    }

    public void Heal() {

        if ((Input.GetKey(KeyCode.Z) || CrossPlatformInputManager.GetButton(Constants.SPECIAL_BUTTON)) && currentHealDuration > 0f && _player.currentGrdState == PlayerController.GroundStates.grounded && _levelManager.currentHealth < 6 && _player.healCharges > 0)
        {
            _player.speedMultiplier = 0.1f;
            currentHealDuration -= Time.deltaTime;
            healCooldown -= Time.deltaTime;
        }
        else
        {
            _player.speedMultiplier = 1f;
            currentHealDuration = healDuration;
            healCooldown = maxHealCooldown;
        }

        if (currentHealDuration < 0f)
        {
            GameObject.Instantiate(_player.dashParticles, _player.transform.position, _player.transform.rotation);
            _levelManager.currentHealth += healAmount;
            _player.healCharges--;

            _levelManager.updateHeartUI();
            _levelManager.updateHealthChargesUI();
            _player.speedMultiplier = 1f;
            currentHealDuration = healDuration;
            currentState = HealStates.cooldown;
        }
    }

    public void Jump(float velocity)
    {
        throw new System.NotImplementedException();
    }
}
