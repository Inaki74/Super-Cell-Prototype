
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private LevelManager _levelManager;
    public string LevelToLoad;
    public Sprite closedFlag;
    public Sprite openFlag;
    public string levelToUnlock;

    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = closedFlag;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _spriteRenderer.sprite = openFlag;
            _levelManager.endLevel(LevelToLoad, levelToUnlock);
        }
    }

}
