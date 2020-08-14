using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    private LevelManager _levelManager;
    [SerializeField]
    private Collider2D _collider;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _collider = GetComponent<Collider2D>();

        //_collider.bounds.center.x
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _levelManager.Hurt(damage, _collider.bounds.center.x, collision.bounds.center.x);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _levelManager.Hurt(damage, _collider.bounds.center.x, collision.bounds.center.x);
        }
    }
}
