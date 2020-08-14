using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public float currentHealDuration;
    public float healCooldown;
    public Debugger(float duration, float cd)
    {
        currentHealDuration = duration;
        healCooldown = cd;
    }

    public Debugger()
    {

    }

    public void updateDuration(float up)
    {
        currentHealDuration = up;
    }

    public void updateCooldown(float up)
    {
        healCooldown = up;
    } 
}
