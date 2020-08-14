using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject target;
    public float followAhead;
    public float followSmooth;

    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    private void Follow()
    {
        transform.position = target.transform.position;
    }
}
