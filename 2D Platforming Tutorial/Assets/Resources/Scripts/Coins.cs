using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public int value;

    private LevelManager _theLevelManager;
    // Start is called before the first frame update
    void Start()
    {
        _theLevelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _theLevelManager.addCoins(value);
            gameObject.SetActive(false);
        }
    }
}
