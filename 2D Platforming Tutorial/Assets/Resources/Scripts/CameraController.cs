using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private LevelManager _levelManager;

    public GameObject target;
    public float followAhead;
    public float cameraSmooth;

    public bool levelEnding;

    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _levelManager.onEnd += endLevel;
        levelEnding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelEnding)
        {
            BasicCameraControl();
        }
    }

    private void BasicCameraControl()
    {
        targetPosition = new Vector3(target.transform.position.x, transform.position.y, -10);

        if(target.transform.localScale.x > 0f)
        {
            targetPosition = new Vector3(target.transform.position.x + followAhead, target.transform.position.y + 1.7f, -10);
        }
        else
        {
            targetPosition = new Vector3(target.transform.position.x - followAhead, target.transform.position.y + 1.7f, -10);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmooth * Time.deltaTime);
    }

    public void endLevel()
    {
        levelEnding = true;
    }
}
