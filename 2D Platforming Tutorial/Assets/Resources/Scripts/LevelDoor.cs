using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class LevelDoor : MonoBehaviour
{
    public string levelToLoad;
    [SerializeField]
    private bool unlocked = true;

    public Sprite doorBottomOpen;
    public Sprite doorTopOpen;
    public Sprite doorBottomClosed;
    public Sprite doorTopClosed;

    public SpriteRenderer doorTop;
    public SpriteRenderer doorBottom;

    public void Start()
    {
        //PlayerPrefs.SetInt("Level One", 1);

        //unlocked = PlayerPrefs.GetInt(levelToLoad) == 1;

        if (unlocked)
        {
            doorTop.sprite = doorTopOpen;
            doorBottom.sprite = doorBottomOpen;
        }
        else
        {
            doorTop.sprite = doorTopClosed;
            doorBottom.sprite = doorBottomClosed;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && (Input.GetKeyDown(KeyCode.UpArrow) || CrossPlatformInputManager.GetButtonDown(Constants.JUMP_BUTTON)) && unlocked)
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
