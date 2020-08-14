using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float speed;
    public State currentState;
    public enum State
    {
        Aggressive,
        Stationary
    }

    [SerializeField]
    private float _wallCheckDistance;
    [SerializeField]
    private LayerMask _whatIsGround;
    private bool _onWall;
    private Rigidbody2D _myrigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _myrigidbody = GetComponent<Rigidbody2D>();
        currentState = State.Stationary;
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnWall();
    }

    private void FixedUpdate()
    {
        if (currentState == State.Aggressive)
        {
            _myrigidbody.velocity = new Vector2(-speed * transform.localScale.x, _myrigidbody.velocity.y);
        }

        if (_onWall)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private void OnBecameVisible()
    {
        currentState = State.Aggressive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "KillPlane")
        {
            GetComponent<DamageEnemy>().die();
        }
    }

    private void CheckOnWall()
    {
        //onWall = Physics2D.OverlapCircle(GetComponent<BoxCollider2D>().bounds.center + new Vector3(0.25f, 0, 0), groundCheckRadius, whatIsGround);
        _onWall = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, _wallCheckDistance, _whatIsGround);
    }

    private void OnEnable()
    {
        currentState = State.Stationary;
    }
}
