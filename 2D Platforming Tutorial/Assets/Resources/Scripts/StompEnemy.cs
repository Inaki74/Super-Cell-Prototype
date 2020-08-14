using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour
{
    public float bounce;
    public Rigidbody2D playerRigidbody;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<DamageEnemy>().damage(damage);

            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, bounce);
        }
    }
}
