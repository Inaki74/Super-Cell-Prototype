using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public GameObject deathParticles;
    public Animator MyAnimator;
    public AudioSource hurtSound;
    public int health;
    [SerializeField] private string IAm;

    void Start()
    {
        MyAnimator = GetComponentInParent<Animator>();
        
    }
    // Update is called once per frame
    void Update()
    {
        if(health < 0)
        {
            die();
        }
    }

    public void damage(int dmg)
    {
        health -= dmg;
        hurtSound.Play();
        MyAnimator.Play("Base Layer."+IAm+"Damage");
    }

    public void die()
    {
        Instantiate(deathParticles, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
