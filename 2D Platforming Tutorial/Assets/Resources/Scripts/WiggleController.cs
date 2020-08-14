using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleController : MonoBehaviour
{
    public Transform left;
    public Transform right;
    public State currentState;
    public enum State
    {
        Right,
        Left
    }

    public float speed;

    private Rigidbody2D _myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == State.Right && transform.position.x > right.position.x)
        {
            currentState = State.Left;
        }
        if (currentState == State.Left && transform.position.x < left.position.x)
        {
            currentState = State.Right;
        }
        switch (currentState)
        {
            case State.Right:
                _myRigidbody.velocity = new Vector2(speed, _myRigidbody.velocity.y);
                break;
            case State.Left:
                _myRigidbody.velocity = new Vector2(-speed, _myRigidbody.velocity.y);
                break;
        }
    }
}
