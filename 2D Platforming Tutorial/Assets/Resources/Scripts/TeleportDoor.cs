using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    public int id;
    public Sprite locked;
    public Sprite unlocked;
    public SpriteRenderer sprite;
    public TeleportDoorsManager manager;
    public bool isLocked;

    [SerializeField]
    private bool _inTrigger;
    private Collider2D _collided;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        manager = FindObjectOfType<TeleportDoorsManager>();
        _collided = null;
        isLocked = true;
    }

    private void Update()
    {
        if(_collided != null)
        {
            if (_collided.tag == "Player" && Input.GetKeyDown(KeyCode.UpArrow) && !isLocked && _inTrigger)
            {
                manager.Teleport(id);
            }

            if (_collided.tag == "Player" && Input.GetKeyDown(KeyCode.UpArrow) && isLocked && _inTrigger)
            {
                isLocked = false;
                manager.UpdateDoor(this);
                sprite.sprite = unlocked;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _inTrigger = true;
        _collided = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _inTrigger = false;
        _collided = null;
    }
}
