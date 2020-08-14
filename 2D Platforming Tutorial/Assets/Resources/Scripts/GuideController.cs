using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GuideController : MonoBehaviour
{
    public string theText;
    public Text text;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        text.text = theText;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canvas.SetActive(false);
        }
    }
}
