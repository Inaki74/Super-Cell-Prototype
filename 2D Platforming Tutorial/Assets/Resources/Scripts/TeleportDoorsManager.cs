using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TeleportDoorsManager : MonoBehaviour
{
    public TeleportDoor[] doors;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        doors = FindObjectsOfType<TeleportDoor>();
        player = FindObjectOfType<PlayerController>();
        doors = doors.OrderBy(d => d.id).ToArray();

        for (int i = 0; i < doors.Length; i++)
        {
            Debug.Log(doors[i].id);
        }
    }

    public void UpdateDoor(TeleportDoor newDoor)
    {
        bool found = false;
        for(int i = 0; i < doors.Length && !found; i++)
        {
            if(doors[i].id == newDoor.id)
            {
                doors[i] = newDoor;
                found = true;
            }
        }
    }

    public void Teleport(int currentDoor)
    {
        int lockedCount = 0;
        bool found = false;
        int nextDoor = currentDoor - 1;

        while (lockedCount < doors.Length - 1 && !found)
        {
            if (nextDoor < 0)
            {
                nextDoor = doors.Length - 1;
            }

            if (doors[nextDoor].isLocked == false)
            {
                player.transform.position = new Vector2(doors[nextDoor].transform.position.x, doors[nextDoor].transform.position.y);
                found = true;
            }
            else
            {
                lockedCount++;
                nextDoor--;
            }
        }
        
    }
}
