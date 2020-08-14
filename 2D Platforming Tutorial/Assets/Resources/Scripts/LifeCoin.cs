using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCoin : MonoBehaviour
{

    public int livesToGive;
    private LevelManager _theLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        _theLevelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _theLevelManager.addLives(livesToGive);
            Destroy(gameObject);
        }
        
    }
}
