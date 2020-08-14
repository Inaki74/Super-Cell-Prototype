using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickObtain : MonoBehaviour
{
    public ParticleSystem obtain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Instantiate(obtain, transform.position, transform.rotation);
            collision.GetComponent<PlayerController>().quickUnlocked = true;
            Destroy(transform.gameObject);
        }
    }
}
