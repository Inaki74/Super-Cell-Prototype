using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public string color;
    public Sprite redLock;
    public Sprite blueLock;
    public Sprite greenLock;
    public Sprite yellowLock;

    private LevelManager _levelManager;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        sprite = GetComponent<SpriteRenderer>();
        DecideSprite();
    }

    private void DecideSprite()
    {
        switch (color)
        {
            case "Red":
                sprite.sprite = redLock;
                break;
            case "Green":
                sprite.sprite = greenLock;
                break;
            case "Yellow":
                sprite.sprite = yellowLock;
                break;
            case "Blue":
                sprite.sprite = blueLock;
                break;
            default:
                Debug.Log("Color property in lock is mispelled.");
                break;
        }
    }

    private void DecideOpenLock()
    {
        switch (color)
        {
            case "Red":
                if (_levelManager.getKeyCount(color) > 0)
                {
                    _levelManager.addRedKey(-1);
                    Destroy(gameObject);
                }
                break;
            case "Green":
                if (_levelManager.getKeyCount(color) > 0)
                {
                    _levelManager.addGreenKey(-1);
                    Destroy(gameObject);
                }
                break;
            case "Yellow":
                if (_levelManager.getKeyCount(color) > 0)
                {
                    _levelManager.addYellowKey(-1);
                    Destroy(gameObject);
                }
                break;
            case "Blue":
                if (_levelManager.getKeyCount(color) > 0)
                {
                    _levelManager.addBlueKey(-1);
                    Destroy(gameObject);
                }
                break;
            default:
                Debug.Log("Color property in lock is mispelled.");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            DecideOpenLock();
            
        }
    }
}
