using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public string color;
    public Sprite redKey;
    public Sprite blueKey;
    public Sprite greenKey;
    public Sprite yellowKey;

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
        switch (color){
            case "Red":
                sprite.sprite = redKey;
                break;
            case "Green":
                sprite.sprite = greenKey;
                break;
            case "Yellow":
                sprite.sprite = yellowKey;
                break;
            case "Blue":
                sprite.sprite = blueKey;
                break;
            default:
                Debug.Log("Color property in key is mispelled.");
                break;
        }
    }

    private void DecideGrabbedKey()
    {
        switch (color)
        {
            case "Red":
                _levelManager.addRedKey(1);
                break;
            case "Green":
                _levelManager.addGreenKey(1);
                break;
            case "Yellow":
                _levelManager.addYellowKey(1);
                break;
            case "Blue":
                _levelManager.addBlueKey(1);
                break;
            default:
                Debug.Log("Color property in key is mispelled.");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            DecideGrabbedKey();
            Destroy(gameObject);
        }
    }
}
