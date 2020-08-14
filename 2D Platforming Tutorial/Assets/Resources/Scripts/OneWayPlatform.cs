using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D _effector;
    private BoxCollider2D _bc;
    
    private float waitTime;
    [SerializeField]
    private bool touchingPlayer;
    // Start is called before the first frame update
    void Start()
    {
        _effector = GetComponent<PlatformEffector2D>();
        _bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && touchingPlayer)
        {
            //_effector.rotationalOffset = 180;
            _bc.enabled = false;
            waitTime = 0.2f;
        }

        if(waitTime > 0f)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            if (!touchingPlayer)
            {
                _bc.enabled = true;
                //_effector.rotationalOffset = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            touchingPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            touchingPlayer = false;
        }
    }
}
