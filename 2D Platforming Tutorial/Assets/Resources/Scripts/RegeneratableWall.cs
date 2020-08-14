using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegeneratableWall : MonoBehaviour
{
    public GameObject brokenWall;
    public float time;

    private bool runOnce = false;

    private void Start()
    {
        brokenWall.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(brokenWall.activeSelf == false && !runOnce)
        {
            StartCoroutine("Regenerate");
            runOnce = true;
        }
    }

    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(time);

        brokenWall.SetActive(true);
        runOnce = false;
    }
}
