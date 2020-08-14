using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public int fadeTime;
    public Image blackScreen;
    public bool fadeIn;
    // Start is called before the first frame update
    void Start()
    {
        blackScreen = GetComponent<Image>();
        fadeIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fadeIn)
        {
            blackScreen.CrossFadeAlpha(0f, fadeTime, false);
        }
        else
        {
            //fade in
            blackScreen.CrossFadeAlpha(255f, fadeTime, false);
        }
        
    }
}
