using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnRespawn : MonoBehaviour
{
    private Vector2 _startPosition;
    private Quaternion _startRotation;
    private Vector2 _startScale;
    private int _startHealth;

    private Rigidbody2D _myRigidbody;
    private DamageEnemy _myDamageable;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startScale = transform.localScale;
        
        if(GetComponent<Rigidbody2D>() != null)
        {
            _myRigidbody = GetComponent<Rigidbody2D>();
        }
        if (GetComponent<DamageEnemy>() != null)
        {
            _myDamageable = GetComponent<DamageEnemy>();
            _startHealth = _myDamageable.health;
        }
        else
        {
            _myDamageable = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetObject()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        transform.localScale = _startScale;
        if (_myDamageable != null)
        {
            _myDamageable.health = _startHealth;
        }
        if (_myRigidbody != null)
        {
            _myRigidbody.velocity = Vector2.zero;
        }
    }
}
