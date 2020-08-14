using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // Linear movement
    public GameObject movingObject;
    public Transform start;
    public Transform end;

    public float speed;

    [SerializeField]
    private Vector2 _currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        //movingObject.transform.position = start.position;
        _currentTarget = end.position;
    }

    // Update is called once per frame
    void Update()
    {
        movingObject.transform.position = Vector2.MoveTowards(movingObject.transform.position, _currentTarget, speed * Time.deltaTime);

        if(movingObject.transform.position == end.position)
        {
            _currentTarget = start.position;
        }

        if (movingObject.transform.position == start.position)
        {
            _currentTarget = end.position;
        }
    }
}
